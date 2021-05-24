using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: optimize shooting system

public class AliensGrid : MonoBehaviour
{
    [SerializeField]
    private List<Alien> aliens = null;

    // Movement
    [SerializeField]
    private Transform rightLimit = null;
    [SerializeField]
    private float delayToMove = 0.0f;
    [SerializeField]
    private float speedBonusPerKill = 0.0f;
    [SerializeField]
    private float moveHorizontalDistance = 0.0f;
    [SerializeField]
    private float moveVerticalDistance = 0.0f;
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
        /*
        StopCoroutine(updateCoroutine);
        updateCoroutine = null;
        */
    }

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            if (resetCoroutine == null)
            {

                if (Time.time >= timeToMove)
                {
                    timeToMove = Time.time + delayToMove - speedBonus;

                    bool reachedLimit = false;

                    float yMove;
                    if (shouldGoDown)
                    {
                        yMove = moveVerticalDistance;
                        shouldGoDown = false;
                    }
                    else
                    {
                        yMove = 0.0f;
                    }

                    shootersList = new List<Alien>();

                    for (int e = 0; e < aliens.Count; e++)
                    {
                        Alien enemy = aliens[e];
                        if (enemy.IsAlive)
                        {
                            enemy.MoveAndAnimate(x: moveHorizontalDistance, y: yMove);

                            bool isBehindOther = false;
                            int indexOfEnemyBelow = e - 11;
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
                                shootersList.Add(enemy);

                                // Take advantage of the fact there is at least a single shooter in each column to make just the
                                // shooter of the column check for the reachLimit of the screen
                                reachedLimit |= (Mathf.Abs(enemy.X) > rightLimit.position.x);
                            }

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

                    if (Time.time > timeToAllowNextSoundStep)
                    {
                        timeToAllowNextSoundStep = Time.time + minIntervalBetweenStepSounds;

                        stepsAudioSource.pitch = (8.0f - (3 * (timeToMove - Time.time) / delayToMove)) / 5;
                        stepsAudioSource.PlayOneShot(stepsAudioClips[stepSoundIndex]);
                        stepSoundIndex = (stepSoundIndex + 1) % stepsAudioClips.Length;
                    }
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

    public void DoReset(int bulletsAllowed = 1)
    {
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }
        resetCoroutine = StartCoroutine(ResetCoroutine(bulletsAllowed));

        if (updateCoroutine != null)
        {
            StopCoroutine(updateCoroutine);
            updateCoroutine = null;
        }
    }
    private IEnumerator ResetCoroutine(int bulletsAllowed)
    {
        foreach(Alien alien in aliens)
        {
            alien.DeactivateGameObject();
        }

        foreach (Alien alien in aliens)
        {
            yield return waitForFixedUpdate;
            alien.DoReset();
        }

        speedBonus = 0.0f;

        bulletInstances = new Bullet[bulletsAllowed];

        resetCoroutine = null;
    }

    public void OnAlienDied(Alien alien)
    {
        speedBonus += speedBonusPerKill;

        shootersList.Remove(alien);

        score.ScorePoints(alien.PointsToScoreOnDeath);

        bool isThereAnAlienAlive = false;
        foreach (Alien a in aliens)
        {
            if (a.IsAlive)
            {
                isThereAnAlienAlive = true;
                break;
            }
        }

        if (isThereAnAlienAlive)
        {
            // Freeze a little for better game feel
            timeToMove += stopOnDeathDelay;
        }
        else
        {
            DoReset(bulletsAllowed: bulletInstances.Length + 1);
            StartToMove();

            BarrierPiece.EnableAllPieces();
        }

        explosionsAudioSource.PlayOneShot(explosionAudioClips[Random.Range(0, explosionAudioClips.Length)]);
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
