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

    // Pursuer Bullet Shooting
    [SerializeField]
    private PursuerBullet pursuerBulletPrototype = null;
    [SerializeField]
    private Transform aim = null;
    [SerializeField]
    private int amountOfShotsToEarnPursuerBullet = 0;
    private int regularShotsCounter;
    [SerializeField]
    private AliensGrid aliensGrid = null;
    private Alien closestAlien;

    [SerializeField]
    private PlayerDamageable playerDamageable = null;

    public static Bullet CurrentBullet { set => currentBullet = value; }
    public int LifesAmount { get => playerDamageable.LifesAmount; }
    public bool IsDead { get => playerDamageable.IsDead; }

    private void Awake()
    {
        // It's important to let this script active and enable at beggining, so it can set y and x as soon as possible
        this.enabled = false;
        y = transform.position.y;
        z = transform.position.z;

        PursuerBullet.Aim = aim.gameObject;
    }

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
        bool shouldShootRegularBullet = (Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire2")) && currentBullet == null;
        if (shouldShootRegularBullet)
        {
            currentBullet = Instantiate(bulletPrototype, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

            // If player deserves a pursuer bullet
            regularShotsCounter++;
            if (regularShotsCounter >= amountOfShotsToEarnPursuerBullet)
            {
                // Then focus aim in the closest alien
                closestAlien = aliensGrid.GetClosestShooterAlienOrGetNull(transform.position);
                if (closestAlien != null)
                {
                    aim.SetParent(closestAlien.transform);
                    aim.localPosition = Vector3.zero;
                    aim.gameObject.SetActive(true);
                }
            }
        }

        bool shouldShootPursuerBullet = Input.GetButtonDown("Fire1") && regularShotsCounter >= amountOfShotsToEarnPursuerBullet && closestAlien.gameObject.activeInHierarchy;
        if (shouldShootPursuerBullet)
        {
            PursuerBullet pursuerBullet = Instantiate(pursuerBulletPrototype, bulletSpawnPoint.position, Quaternion.identity);
            pursuerBullet.Target = closestAlien.transform;

            regularShotsCounter = 0;
        }
    }

    public void ResetPositionAndShotsCounter()
    {
        transform.position = new Vector3(-rightLimit.position.x, y, z);

        this.regularShotsCounter = 0;
    }
    public void ResetLifes()
    {
        playerDamageable.ResetLifes();
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
