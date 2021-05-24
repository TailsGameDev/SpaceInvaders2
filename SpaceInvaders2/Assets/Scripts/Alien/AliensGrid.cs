using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliensGrid : MonoBehaviour
{
    [SerializeField]
    private List<Alien> aliens = null;

    // Movement
    [SerializeField]
    private Transform rightLimit = null;
    [SerializeField]
    private float originalDelayToMove = 0.0f;
    [SerializeField]
    private float speedBonusPerKill = 0.0f;
    [SerializeField]
    private float moveHorizontalDistance = 0.0f;
    [SerializeField]
    private float moveVerticalDistance = 0.0f;
    [SerializeField]
    private float bonusMovementPerLevel = 0.0f;
    [SerializeField]
    private float stopOnDeathDelay = 0.0f;
    private float timeToMove;
    private bool shouldGoDown;
    private float speedBonus;

    // Shooting
    [SerializeField]
    private Bullet alienBulletPrototype = null;
    [SerializeField]
    private float minCooldown = 0.0f;
    private List<Alien> shootersList;
    private Bullet[] bulletInstances = null;
    private float timeAfterMinCooldown;

    // Score
    [SerializeField]
    private Score score = null;

    [Header("Sounds")]
    [SerializeField]
    private AudioSource stepsAudioSource = null;
    [SerializeField]
    private AudioClip[] stepsAudioClips = null;
    private int stepSoundIndex;

    [SerializeField]
    private AudioSource explosionsAudioSource = null;
    [SerializeField]
    private AudioClip[] explosionAudioClips = null;

    [SerializeField]
    private float minIntervalBetweenStepSounds = 0.0f;
    private float timeToAllowNextSoundStep;

    private Coroutine updateCoroutine = null;
    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
    private Coroutine resetCoroutine = null;
    private bool isMoving;

    private void Awake()
    {
        Alien.AlienGrid = this;

        foreach (Alien alien in aliens)
        {
            alien.MemorizeOriginalPosition();
        }

        enabled = false;
    }

    public void StartToMove()
    {
        if (updateCoroutine == null)
        {
            updateCoroutine = StartCoroutine(UpdateCoroutine());
        }
        isMoving = true;
    }
    public void StopMoving()
    {
        isMoving = false;
    }

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            if (resetCoroutine == null)
            {

                if (Time.time >= timeToMove)
                {
                    timeToMove = Time.time + originalDelayToMove - speedBonus;

                    bool reachedLimit = false;

                    CalculateMoveXY(out float moveX, out float moveY);

                    // Move all aliens while populating shootersList, and maybe waiting a little
                    // bit if isMoving==false (that happens when player dies)
                    // Ah, inside the loop it's also verified if the aliens reachedLimit of the screen.
                    shootersList = new List<Alien>();
                    for (int a = 0; a < aliens.Count; a++)
                    {
                        Alien alien = aliens[a];
                        if (alien.IsAlive)
                        {
                            PopulateShootersListAndVerifyReachedLimit(alien, a, moveX, moveY, ref reachedLimit);
                            yield return null;
                        }

                        while (!isMoving)
                        {
                            yield return null;
                        }
                    }

                    if (reachedLimit)
                    {
                        shouldGoDown = true;
                        moveHorizontalDistance = -moveHorizontalDistance;
                    }

                    HandleMovementSFX();
                }

                // It's too weird when there is just a single alien ship if it can shoot like 3 bullets or more, so let's
                // limit the shooting when there is too few aliens on shooterList
                int iterationLimit = Mathf.Min(bulletInstances.Length, shootersList.Count);
                for (int b = 0; b < iterationLimit; b++)
                {
                    if (bulletInstances[b] == null && Time.time > timeAfterMinCooldown)
                    {
                        int shooterIndex = Random.Range(0, shootersList.Count);
                        Alien shooter = shootersList[shooterIndex];

                        // Shoot
                        bulletInstances[b] = Instantiate(alienBulletPrototype, shooter.transform.position - (Vector3.up * 0.8f),
                            Quaternion.LookRotation(forward: Vector3.forward, upwards: -Vector3.up));

                        timeAfterMinCooldown = Time.time + minCooldown;
                    }
                }
            }

            yield return waitForFixedUpdate;
        }
    }
    private void CalculateMoveXY(out float moveX, out float moveY)
    {
        // moveX
        float absBonusX = bulletInstances.Length * bonusMovementPerLevel;
        float bonusX = moveHorizontalDistance > 0.0f ? absBonusX : -absBonusX;
        moveX = moveHorizontalDistance + bonusX;

        // moveY
        if (shouldGoDown)
        {
            float bonusY = bulletInstances.Length * bonusMovementPerLevel;
            moveY = moveVerticalDistance + bonusY;
            shouldGoDown = false;
        }
        else
        {
            moveY = 0.0f;
        }
    }
    private void PopulateShootersListAndVerifyReachedLimit(Alien alien, int alienIndex, float moveX, float moveY, ref bool reachedLimit)
    {
        alien.MoveAndAnimate(x: moveX, y: moveY);

        bool isBehindOther = false;
        const int AMOUNT_OF_COLUMNS_IN_THE_GRID = 11;
        int indexOfEnemyBelow = alienIndex - AMOUNT_OF_COLUMNS_IN_THE_GRID;
        while (indexOfEnemyBelow > 0)
        {
            if (aliens[indexOfEnemyBelow].IsAlive)
            {
                isBehindOther = true;
                break;
            }
            indexOfEnemyBelow -= 11;
        }
        if (!isBehindOther)
        {
            shootersList.Add(alien);

            // Take advantage of the fact there is at least a single shooter in each column to make just the
            // shooter of the column check for the reachLimit of the screen
            reachedLimit |= (Mathf.Abs(alien.X) > rightLimit.position.x);
        }
    }
    private void HandleMovementSFX()
    {
        // Play Movement SFX but with a little cooldown so the player's ears don't bleed when the loop get's really very fast!
        // Also increase the pitch a little bit as the loop gets faster.
        if (Time.time > timeToAllowNextSoundStep)
        {
            timeToAllowNextSoundStep = Time.time + minIntervalBetweenStepSounds;

            // Increase the pitch in proportion to the currentMovementDelay. Note it moved just now, so we can get the delay
            // from the current time and the next timeToMove.
            float currentMovementDelay = (timeToMove - Time.time);
            stepsAudioSource.pitch = (8.0f - (3 * currentMovementDelay / originalDelayToMove)) / 5;

            // Play the stepsAudioClips in order. I mean play the one at stepSoundIndex then increment the index.
            stepsAudioSource.PlayOneShot(stepsAudioClips[stepSoundIndex]);
            stepSoundIndex = (stepSoundIndex + 1) % stepsAudioClips.Length;
        }
    }

    public void DoReset(int bulletsAllowed = 1)
    {
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }
        resetCoroutine = StartCoroutine(AnimatedResetCoroutine());

        if (updateCoroutine != null)
        {
            StopCoroutine(updateCoroutine);
            updateCoroutine = null;
        }

        speedBonus = 0.0f;

        // Note bulletInstances.Length is being used along the script to verify how many times the player killed
        // all the aliens in the grid during this match.
        bulletInstances = new Bullet[bulletsAllowed];
    }
    private IEnumerator AnimatedResetCoroutine()
    {
        foreach(Alien alien in aliens)
        {
            alien.DeactivateGameObject();
        }

        foreach (Alien alien in aliens)
        {
            // waitForFixedUpdate so they appear animated one at a time
            yield return waitForFixedUpdate;
            alien.DoReset();
        }

        resetCoroutine = null;
    }

    public void OnAlienDied(Alien alien)
    {
        speedBonus += speedBonusPerKill;

        shootersList.Remove(alien);

        score.ScorePoints(alien.PointsToScoreOnDeath);

        if (IsThereAtLeastOneAlienAlive())
        {
            // Give a tiny little advantage to player when an alien dies by making timeToMove higher
            timeToMove += stopOnDeathDelay;
        }
        else
        {
            DoReset(bulletsAllowed: bulletInstances.Length + 1);
            StartToMove();

            BarrierPiece.EnableAllPieces();
        }

        AudioClip randomExplosionSound = explosionAudioClips[Random.Range(0, explosionAudioClips.Length)];
        explosionsAudioSource.PlayOneShot(randomExplosionSound);
    }
    private bool IsThereAtLeastOneAlienAlive()
    {
        bool isThereAnAlienAlive = false;
        foreach (Alien a in aliens)
        {
            if (a.IsAlive)
            {
                isThereAnAlienAlive = true;
                break;
            }
        }
        return isThereAnAlienAlive;
    }

    public Alien GetClosestShooterAlienOrGetNull(Vector3 referencePosition)
    {
        Alien closest = null;
        if (shootersList.Count > 0)
        {
            closest = shootersList[0];
            float minSqrDistance = Vector3.SqrMagnitude(vector: referencePosition - closest.transform.position);
            for (int s = 1; s < shootersList.Count; s++)
            {
                Alien candidate = shootersList[s];
                float sqrDistance = Vector3.SqrMagnitude(vector: referencePosition - candidate.transform.position);

                if (sqrDistance < minSqrDistance)
                {
                    closest = candidate;
                    minSqrDistance = sqrDistance;
                }
            }
        }
        return closest;
    }
}
