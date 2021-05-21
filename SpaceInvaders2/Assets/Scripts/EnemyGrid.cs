using System.Collections.Generic;
using UnityEngine;

public class EnemyGrid : MonoBehaviour
{
    [SerializeField]
    private List<Enemy> enemies = null;

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

    [SerializeField]
    private Bullet enemyBulletPrototype = null;

    [SerializeField]
    private float chanceToShoot = 0.0f;

    private float timeToMove;

    private bool shouldGoDown;

    private float speedBonus;

    private void Awake()
    {
        Enemy.EnemyGrid = this;
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

            for (int e = 0; e < enemies.Count; e++)
            {
                Enemy enemy = enemies[e];
                if (enemy.IsAlive)
                {
                    enemy.MoveAndAnimate(x: moveHorizontalDistance, y: yMove);

                    reachedLimit |= (Mathf.Abs(enemy.X) > rightLimit.position.x);

                    int indexOfEnemyBelow = e - 11;
                    if (indexOfEnemyBelow < 0 || (!enemies[indexOfEnemyBelow].IsAlive))
                    {
                        if (Random.Range(0.0f, 1.0f) < chanceToShoot)
                        {
                            // Shoot
                            Instantiate(enemyBulletPrototype, enemy.transform.position - (Vector3.up * 0.8f), 
                                Quaternion.LookRotation(forward: Vector3.forward, upwards: -Vector3.up));
                        }
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
    }

    public void Accelerate()
    {
        speedBonus += speedBonusPerKill;
    }
}
