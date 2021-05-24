using UnityEngine;

public class AlienDamageable : Damageable
{
    [SerializeField]
    private Alien alien = null;

    private static AliensGrid aliensGrid;

    public static AliensGrid AliensGrid { set => aliensGrid = value; }

    public override void Die()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            aliensGrid.OnAlienDied(alien);
        }
        else
        {
            Debug.LogWarning("[AlienDamageable] What is dead may never die", this);
        }
    }
}
