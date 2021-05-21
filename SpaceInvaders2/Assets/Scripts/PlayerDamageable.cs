using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageable : Damageable
{
    [SerializeField]
    private AliensGrid aliensGrid = null;

    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    [SerializeField]
    private Player player = null;

    [SerializeField]
    private float timeDead = 0.0f;
    private WaitForSecondsRealtime waitWhileDead;

    [SerializeField]
    private int lifesAmount = 0;
    [SerializeField]
    private UserInterface userInterface = null;

    private void Awake()
    {
        waitWhileDead = new WaitForSecondsRealtime(timeDead);

        userInterface.ShowPlayerLifes(lifesAmount);
    }

    public override void Die()
    {
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        // Time.timeScale = 0.0f;
        aliensGrid.enabled = false;
        spriteRenderer.enabled = false;
        player.enabled = false;

        yield return waitWhileDead;

        player.enabled = true;
        spriteRenderer.enabled = true;
        aliensGrid.enabled = true;
        // Time.timeScale = 1.0f;

        lifesAmount--;
        userInterface.ShowPlayerLifes(lifesAmount);
    }
}
