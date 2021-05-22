using UnityEngine;

public class AlienDamageable : Damageable
{
    [SerializeField]
    private Alien alien = null;

    private static AliensGrid aliensGrid;

    public static AliensGrid AliensGrid { set => aliensGrid = value; }

    public override void Die()
    {
        gameObject.SetActive(false);
        aliensGrid.OnAlienDied(alien);
    }
}
