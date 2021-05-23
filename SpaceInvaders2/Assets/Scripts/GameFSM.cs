using UnityEngine;

public class GameFSM : MonoBehaviour
{
    public enum GameState
    {
        MENU,
        CHOOSING_SKINS,
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
    private AlienBonusShip alienBonusShip = null;
    [SerializeField]
    private MainMenuAndHUD mainMenuAndHud = null;
    [SerializeField]
    private Score score = null;
    [SerializeField]
    private UISkinsMenu uiSkinsMenu = null;
    [SerializeField]
    private BarrierRenderers barrierRenderers = null;

    [SerializeField]
    private float playerDeathDelay = 0.0f;
    [SerializeField]
    private float gameOverDelay = 0.0f;

    private GameState currentState = GameState.MENU;
    private float currentTimeToWaitFor;

    private void Awake()
    {
        player.ResetLifes();

        Cursor.visible = false;

        score.LoadHighestScoreThenDislpayIt();
    }

    private void Update()
    {
        GameState nextState = currentState;
        
        // Update state logic
        switch (currentState)
        {
            case GameState.MENU:
                if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
                {
                    nextState = GameState.CHOOSING_SKINS;
                }
                break;
            case GameState.CHOOSING_SKINS:
                if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
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
                    mainMenuAndHud.TurnMenuOn(player.LifesAmount);
                    player.ResetLifes();
                    alienBonusShip.enabled = false;
                    break;
                case GameState.CHOOSING_SKINS:
                    uiSkinsMenu.ShowUp();
                    break;
                case GameState.STARTING:
                    uiSkinsMenu.Disappear();

                    // Set skin colors in game elements
                    PlayerSkinsUser.SetAllPlayerSkinsUserColor(uiSkinsMenu.GetPlayerColor());
                    barrierRenderers.SetAllBarriersColor(uiSkinsMenu.GetBarrierColor());

                    // Enable/Reset game fundamental elements
                    BarrierPiece.EnableAllPieces();
                    aliensGrid.DoReset();
                    alienBonusShip.enabled = true;
                    player.ResetPositionAndShotsCounter();
                    mainMenuAndHud.HideMenu();
                    break;
                case GameState.ACTION:
                    aliensGrid.enabled = true;
                    player.GetReadyForAction();

                    mainMenuAndHud.ShowPlayerLifes(player.LifesAmount);
                    break;
                case GameState.PLAYER_DEAD:
                    aliensGrid.enabled = false;
                    player.BehaveAsDead();

                    currentTimeToWaitFor = Time.time + playerDeathDelay;
                    break;
                case GameState.OVER:
                    score.UpdateHighestScoreAndClearScore();
                    mainMenuAndHud.DisplayGameOverText();

                    currentTimeToWaitFor = Time.time + gameOverDelay;
                    break;
            }
        }
    }
}
