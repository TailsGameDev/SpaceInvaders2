using UnityEngine;

public class EndGameCollider : MonoBehaviour
{
    private static PlayerDamageable playerDamageable = null;
    public static PlayerDamageable PlayerDamageable { set => playerDamageable = value; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // As the player has very limited movimentation, the only damageables that can hit theese
        // End Game Colliders are the alien.
        if (collision.collider.tag == "damageable")
        {
            playerDamageable.LoseAllLifesAndDie();
        }
    }
}
