using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    [SerializeField]
    private Text scoreText = null;
    [SerializeField]
    private Text highestScoreText = null;
    private int score;
    private int highestScore;

    [SerializeField]
    private Text playerLifesText = null;
    [SerializeField]
    private Image[] playerLifeImages = null;

    [SerializeField]
    private Text gameOverText = null;
    [SerializeField]
    private float gameOverTimeBeforeFade = 0.0f;
    [SerializeField]
    private Image fader = null;
    [SerializeField]
    private Text playSpaceInvadersText = null;
    private WaitForSeconds waitAfterGameOver;

    [SerializeField]
    private Player player = null;
    [SerializeField]
    private AliensGrid aliensGrid = null;

    private void Awake()
    {
        waitAfterGameOver = new WaitForSeconds(gameOverTimeBeforeFade);
    }

    private void Update()
    {
        if (playSpaceInvadersText.enabled && Input.GetButtonDown("Jump"))
        {
            playSpaceInvadersText.enabled = false;
            fader.enabled = false;
            // TODO: spawn aliens then wait before spawning the player
            aliensGrid.ResetAndEnable();
            player.ResetPositionAndEnable();
        }
    }

    public void ShowPlayerLifes(int lifesAmount)
    {
        playerLifesText.text = lifesAmount.ToString();

        int amountOfLifeImagesToDisplay = lifesAmount - 1;
        for (int i = 0; i < playerLifeImages.Length; i++)
        {
            bool shouldDisplayLifeImage = i < amountOfLifeImagesToDisplay;
            playerLifeImages[i].gameObject.SetActive(shouldDisplayLifeImage);
        }
    }

    // TODO: clean code
    public void OnPointsScored(int points)
    {
        int remainder = score % 500;
        score += points;
        int secondRemainder = score % 500;

        if (secondRemainder < remainder)
        {
            player.GetComponent<PlayerDamageable>().GainALife();
        }

        scoreText.text = score.ToString();
    }

    public void OnGameOver()
    {
        player.GetComponent<PlayerDamageable>().LifesAmount = 3;

        if (score > highestScore)
        {
            highestScore = score;
            highestScoreText.text = highestScore.ToString();
        }

        score = 0;
        scoreText.text = "0000";

        // TODO: animate gameOverText
        gameOverText.enabled = true;

        StartCoroutine(GameOverCoroutine());
    }
    private IEnumerator GameOverCoroutine()
    {
        yield return waitAfterGameOver;
        
        // TODO: animate fader
        fader.enabled = true;

        gameOverText.enabled = false;

        // TODO: animate playerSpaceInvadersText
        playSpaceInvadersText.enabled = true;

        // TODO: get rid of magic number
        ShowPlayerLifes(3);
    }
}
