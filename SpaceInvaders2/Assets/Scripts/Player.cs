using UnityEngine;

public class Player : MonoBehaviour
{
    // Movement
    [SerializeField]
    private float speed = 0.0f;
    [SerializeField]
    private Transform rightLimit = null;
    private float y, z;

    // Shooting
    [SerializeField]
    private Bullet bulletPrototype = null;
    [SerializeField]
    private Transform bulletSpawnPoint = null;
    private static Bullet currentBullet;

    [SerializeField]
    private PlayerDamageable playerDamageable = null;

    public static Bullet CurrentBullet { set => currentBullet = value; }
    public int LifesAmount { get => playerDamageable.LifesAmount; }
    public bool IsDead { get => playerDamageable.IsDead; }

    // Movement logic
    private void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float newX = transform.position.x + (horizontal * speed);
        if (Mathf.Abs(newX) < rightLimit.position.x)
        {
            transform.position = new Vector3(newX, y, z);
        }
    }
    // Shoot logic
    private void Update()
    {
        if (Input.GetButtonDown("Jump") && currentBullet == null)
        {
            currentBullet = Instantiate(bulletPrototype, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        }
    }

    public void ResetPosition()
    {
        y = transform.position.y;
        z = transform.position.z;

        transform.position = new Vector3(-rightLimit.position.x, y, z);
    }
    public void ResetLifes()
    {
        playerDamageable.ResetLifes();
    }
    public void EarnALife()
    {
        playerDamageable.GainALife();
    }

    public void GetReadyForAction()
    {
        enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
        playerDamageable.IsDead = false;
    }
    public void BehaveAsDead()
    {
        enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
