using UnityEngine;

public class PlayerDamageable : Damageable
{
    [SerializeField]
    private int initialLifesAmount = 0;
    private int lifesAmount = 0;
    
    [SerializeField]
    private UserInterface userInterface = null;

    private bool isDead;

    public int LifesAmount { get => lifesAmount; }
    public bool IsDead { get => isDead; set => isDead = value; }

    private void Awake()
    {
        EndGameCollider.PlayerDamageable = this;
    }
    public void ResetLifes()
    {
        lifesAmount = initialLifesAmount;
    }
    public override void Die()
    {
        // NOTE: GameFSM.cs checks this.isDead on 'Update()' so it can control the game flow
        isDead = true;
        // NOTE: By the way, when GameFSM notices isDead==true,
        // it also updates the userInterface for lifesAmount after a little time interval (2 seconds)
        // together with other userInterface elements
        lifesAmount--;
    }
    public void GainALife()
    {
        lifesAmount++;
        userInterface.ShowPlayerLifes(lifesAmount);
    }
    public void LoseAllLifesAndDie()
    {
        // Let's give the player exactly 1 life, so it can Die and lose it. This way after dying zero lifes remain.
        lifesAmount = 1;
        Die();
    }
}
