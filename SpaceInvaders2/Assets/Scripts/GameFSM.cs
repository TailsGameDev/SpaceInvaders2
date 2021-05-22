using UnityEngine;

public class GameFSM : MonoBehaviour
{
    public enum GameState
    {
        MENU,
        STARTING,
        ACTION,
        PLAYER_DEAD,
        OVER,
    }

    [SerializeField]
    private Player player = null;
    [SerializeField]
    private AliensGrid aliensGrid = null;
    [SerializeField]
    private UserInterface userInterface = null;
    [SerializeField]
    private PlayerScore playerScore = null;

    [SerializeField]
    private float playerDeathDelay = 0.0f;
    [SerializeField]
    private float gameOverDelay = 0.0f;

    private GameState currentState = GameState.MENU;
    private float currentTimeToWaitFor;

    private void Awake()
    {
        player.ResetLifes();
    }

    private void Update()
    {
        GameState nextState = currentState;
        
        // Update state logic
        switch (currentState)
        {
            case GameState.MENU:
                if (Input.GetButtonDown("Jump"))
                {
                    nextState = GameState.STARTING;
                }
                break;
            case GameState.STARTING:
                nextState = GameState.ACTION;
                break;
            case GameState.ACTION:
                if (player.IsDead)
                {
                    nextState = GameState.PLAYER_DEAD;
                }
                break;
            case GameState.PLAYER_DEAD:
                if (Time.time > currentTimeToWaitFor)
                {
                    currentTimeToWaitFor = Mathf.Infinity;

                    if (player.LifesAmount > 0)
                    {
                        nextState = GameState.ACTION;
                    }
                    else
                    {
                        nextState = GameState.OVER;
                    }
                }
                break;
            case GameState.OVER:
                if (Time.time > currentTimeToWaitFor)
                {
                    nextState = GameState.MENU;
                }
                break;
        }

        if (currentState != nextState)
        {
            currentState = nextState;

            // Out logic
            switch (currentState)
            {
                case GameState.MENU:
                    player.ResetLifes();
                    userInterface.TurnMenuOn(player.LifesAmount);
                    break;
                case GameState.STARTING:
                    BarrierPiece.EnableAllPieces();
                    aliensGrid.NewReset();
                    player.ResetPosition();
                    userInterface.HideMenu();
                    break;
                case GameState.ACTION:
                    aliensGrid.enabled = true;
                    player.GetReadyForAction();

                    userInterface.ShowPlayerLifes(player.LifesAmount);
                    break;
                case GameState.PLAYER_DEAD:
                    aliensGrid.enabled = false;
                    player.BehaveAsDead();

                    currentTimeToWaitFor = Time.time + playerDeathDelay;
                    break;
                case GameState.OVER:
                    playerScore.UpdateHighestScoreAndClearScore();
                    userInterface.DisplayGameOverText();

                    currentTimeToWaitFor = Time.time + gameOverDelay;
                    break;
            }
        }
    }
}
