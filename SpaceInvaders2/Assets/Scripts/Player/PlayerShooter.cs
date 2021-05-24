using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    // Regular Shooting
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
    private Pause pause = null;

    private void Awake()
    {
        PursuerBullet.Aim = aim.gameObject;
    }

    private void Update()
    {
        if (!pause.IsGamePaused())
        {
            // Regular Shooting and Aiming
            bool shouldShootRegularBullet = (Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire2")) && currentBullet == null;
            if (shouldShootRegularBullet)
            {
                currentBullet = Instantiate(bulletPrototype, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

                // If player got a Pursuer Bullet and didn't shoot it yet
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

            // Pursuer Bullet Shooting
            bool shouldShootPursuerBullet = Input.GetButtonDown("Fire1") && regularShotsCounter >= amountOfShotsToEarnPursuerBullet && closestAlien.gameObject.activeInHierarchy;
            if (shouldShootPursuerBullet)
            {
                PursuerBullet pursuerBullet = Instantiate(pursuerBulletPrototype, bulletSpawnPoint.position, Quaternion.identity);
                pursuerBullet.Target = closestAlien.transform;

                regularShotsCounter = 0;
            }
        }
    }

    public void ResetAimAndShootsCounter()
    {
        this.regularShotsCounter = 0;
        aim.gameObject.SetActive(false);
    }
}
