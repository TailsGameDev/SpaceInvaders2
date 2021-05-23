using UnityEngine;
using UnityEngine.UI;

public class MainMenuAndHUD : MonoBehaviour
{
    // Top of screen
    [SerializeField]
    private Text scoreText = null;
    [SerializeField]
    private Text highestScoreText = null;

    // Bottom of screen
    [SerializeField]
    private Text playerLifesText = null;
    [SerializeField]
    private Image[] playerLifeImages = null;
    
    // Main Menu
    [SerializeField]
    private Image fader = null;
    [SerializeField]
    private Text playSpaceInvadersText = null;

    [SerializeField]
    private Text gameOverText = null;

    public void HideMenu()
    {
        playSpaceInvadersText.enabled = false;
        fader.enabled = false;
    }
    public void TurnMenuOn(int playerLifesAmount)
    {
        fader.enabled = true;
        gameOverText.enabled = false;
        playSpaceInvadersText.enabled = true;

        ShowPlayerLifes(playerLifesAmount);
    }
    public void ShowPlayerLifes(int lifesAmount)
    {
        playerLifesText.text = lifesAmount.ToString();

        // Set all playerLifeImages active so the proper amount is always displayed.
        int amountOfLifeImagesToDisplay = lifesAmount - 1;
        for (int i = 0; i < playerLifeImages.Length; i++)
        {
            bool shouldDisplayLifeImage = i < amountOfLifeImagesToDisplay;
            playerLifeImages[i].gameObject.SetActive(shouldDisplayLifeImage);
        }
    }
    public void UpdateScore(int points)
    {
        scoreText.text = points.ToString();
    }
    public void UpdateHighestScore(int highestScore)
    {
        highestScoreText.text = highestScore.ToString();
    }
    public void DisplayGameOverText()
    {
        gameOverText.enabled = true;
    }
}
