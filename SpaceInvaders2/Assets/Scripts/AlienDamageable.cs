using UnityEngine;

public class AlienDamageable : Damageable
{
    [SerializeField]
    private Alien alien = null;

    private static AliensGrid enemyGrid;

    public static AliensGrid EnemyGrid { set => enemyGrid = value; }

    public override void Die()
    {
        enemyGrid.OnAlienDied(alien);
        gameObject.SetActive(false);
    }
}
