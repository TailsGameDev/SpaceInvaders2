using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovement = null;

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
        PursuerBullet.Aim = aim.gameObject;
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
        playerMovement.ResetPosition();

        this.regularShotsCounter = 0;
    }
    public void ResetLifes()
    {
        playerDamageable.ResetLifes();
    }

    public void GetReadyForAction()
    {
        enabled = true;
        playerMovement.enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
        playerDamageable.IsDead = false;
    }
    public void BehaveAsDead()
    {
        enabled = false;
        playerMovement.enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
