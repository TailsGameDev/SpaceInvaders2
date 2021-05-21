using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    [SerializeField]
    private Text scoreText = null;
    // [SerializeField]
    // private Text highestScoreText = null;
    private int score;
    // private int highestScore;

    [SerializeField]
    private Text playerLifesText = null;
    [SerializeField]
    private Image[] playerLifeImages = null;

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

    public void OnPointsScored(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }
}
