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
    private float delayToMove;
    [SerializeField]
    private float speedBonusPerKill;
    [SerializeField]
    private float moveHorizontalDistance;
    [SerializeField]
    private float moveVerticalDistance;
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
    private PlayerScore playerScore = null;

    private void Awake()
    {
        Alien.AlienGrid = this;

        foreach (Alien alien in aliens)
        {
            alien.MemorizeOriginalPosition();
        }

        enabled = false;
    }

    private void FixedUpdate()
    {
        if (Time.time >= timeToMove)
        {
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
                }
            }

            if (reachedLimit)
            {
                shouldGoDown = true;
                moveHorizontalDistance = -moveHorizontalDistance;
            }
            
            timeToMove = Time.time + delayToMove - speedBonus;
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

    public void ResetAndEnable(int bulletsAllowed = 1)
    {
        foreach (Alien alien in aliens)
        {
            alien.DoReset();
        }

        speedBonus = 0.0f;

        bulletInstances = new Bullet[bulletsAllowed];

        enabled = true;
    }
    public void NewReset(int bulletsAllowed = 1)
    {
        foreach (Alien alien in aliens)
        {
            alien.DoReset();
        }

        speedBonus = 0.0f;

        bulletInstances = new Bullet[bulletsAllowed];
    }

    public void OnAlienDied(Alien alien)
    {
        speedBonus += speedBonusPerKill;

        shootersList.Remove(alien);

        playerScore.ScorePoints(alien.PointsToScoreOnDeath);

        bool isThereAnAlienAlive = false;
        foreach (Alien a in aliens)
        {
            if (a.IsAlive)
            {
                isThereAnAlienAlive = true;
                break;
            }
        }

        if (!isThereAnAlienAlive)
        {
            ResetAndEnable(bulletsAllowed: bulletInstances.Length + 1);
            BarrierPiece.EnableAllPieces();
        }
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
