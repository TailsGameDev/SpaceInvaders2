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
    private List<Alien> shootersList;
    private Bullet bulletInstance = null;

    // Score
    [SerializeField]
    private UserInterface userInterface = null;
    private int score;
    private int highestScore;

    private void Awake()
    {
        Alien.EnemyGrid = this;
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

            // TODO: make aliens move along some frames instead of all in the same frame
            for (int e = 0; e < aliens.Count; e++)
            {
                Alien enemy = aliens[e];
                if (enemy.IsAlive)
                {
                    enemy.MoveAndAnimate(x: moveHorizontalDistance, y: yMove);

                    reachedLimit |= (Mathf.Abs(enemy.X) > rightLimit.position.x);

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

        if (bulletInstance == null)
        {
            int shooterIndex = Random.Range(0, shootersList.Count);
            Alien shooter = shootersList[shooterIndex];

            // Shoot
            bulletInstance = Instantiate(alienBulletPrototype, shooter.transform.position - (Vector3.up * 0.8f),
                Quaternion.LookRotation(forward: Vector3.forward, upwards: -Vector3.up));
        }
    }

    public void OnAlienDied(Alien alien)
    {
        // TODO: make aliens stop for a moment

        speedBonus += speedBonusPerKill;

        shootersList.Remove(alien);

        userInterface.OnPointsScored(alien.PointsToScoreOnDeath);
    }
}
