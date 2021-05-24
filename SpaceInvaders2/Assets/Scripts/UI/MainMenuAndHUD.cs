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
    private UIAnimatedText[] mainMenuAnimatedTexts = null;
    [SerializeField]
    private Text[] mainMenuTexts = null;

    [SerializeField]
    private UIAnimatedText gameOverText = null;

    [SerializeField]
    private Image[] selections = null;
    private int selectionIndex;
    private bool inputWasZeroLastFrame;

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        bool isPressingInput = !Mathf.Approximately(horizontal, 0.0f);
        if (isPressingInput && inputWasZeroLastFrame)
        {
            selections[selectionIndex].enabled = false;

            int increment = horizontal > 0.0f ? 1 : -1;
            selectionIndex = Mathf.Clamp(selectionIndex+increment, min: 0, max: selections.Length-1 );

            selections[selectionIndex].enabled = true;
        }

        inputWasZeroLastFrame = !isPressingInput;
    }
    public bool IsPlayButtonSelected()
    {
        return selectionIndex == 0;
    }

    public void HideMenu()
    {
        foreach (UIAnimatedText text in mainMenuAnimatedTexts)
        {
            text.Hide();
        }
        foreach (Text text in mainMenuTexts)
        {
            text.enabled = false;
        }
        foreach (Image selection in selections)
        {
            selection.gameObject.SetActive(false);
        }
        fader.enabled = false;
    }
    public void TurnMenuOn(int playerLifesAmount)
    {
        fader.enabled = true;
        gameOverText.Hide();
        foreach (UIAnimatedText text in mainMenuAnimatedTexts)
        {
            text.StartWritting();
        }
        foreach (Text text in mainMenuTexts)
        {
            text.enabled = true;
        }
        foreach (Image selection in selections)
        {
            selection.gameObject.SetActive(true);
        }

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
        gameOverText.StartWritting();
    }
}
