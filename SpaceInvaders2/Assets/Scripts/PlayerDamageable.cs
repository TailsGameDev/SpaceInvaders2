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

    public int LifesAmount { set => lifesAmount = value; }

    private void Awake()
    {
        waitWhileDead = new WaitForSecondsRealtime(timeDead);

        userInterface.ShowPlayerLifes(lifesAmount);

        BarrierPiece.PlayerDamageable = this;
    }

    public override void Die()
    {
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        aliensGrid.enabled = false;
        spriteRenderer.enabled = false;
        player.enabled = false;

        yield return waitWhileDead;

        lifesAmount--;
        userInterface.ShowPlayerLifes(lifesAmount);

        if (lifesAmount <= 0)
        {
            userInterface.OnGameOver();
        }
        else
        {
            player.enabled = true;
            spriteRenderer.enabled = true;
            aliensGrid.enabled = true;
        }
    }

    public void GainALife()
    {
        lifesAmount++;
        userInterface.ShowPlayerLifes(lifesAmount);
    }
}
