using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovement = null;
    [SerializeField]
    private PlayerShooter playerShooter = null;
    [SerializeField]
    private PlayerDamageable playerDamageable = null;
    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    public int LifesAmount { get => playerDamageable.LifesAmount; }
    public bool IsDead { get => playerDamageable.IsDead; }

    public void ResetPositionAndShotsCounter()
    {
        playerMovement.ResetPosition();

        playerShooter.DoReset();
    }
    public void ResetLifes()
    {
        playerDamageable.ResetLifes();
    }

    public void GetReadyForAction()
    {
        enabled = true;
        playerMovement.enabled = true;
        playerShooter.enabled = true;
        spriteRenderer.enabled = true;

        playerDamageable.IsDead = false;
    }
    public void BehaveAsDead()
    {
        enabled = false;
        playerMovement.enabled = false;
        playerShooter.enabled = false;
        spriteRenderer.enabled = false;
    }
}
