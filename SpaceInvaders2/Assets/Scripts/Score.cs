using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField]
    private MainMenuAndHUD mainMenuAndHUD = null;
    [SerializeField]
    private PlayerDamageable player = null;
    [SerializeField]
    private int[] scoresToEarnLife = null;
    private int scoresToEarnLifeIndex;

    private int points;
    private int highestScore;
    private const string PLAYER_PREFS_KEY = "HIGHEST_SCORE";

    public int HighestScore { get => highestScore; }

    public void LoadHighestScoreThenDislpayIt()
    {
        highestScore = PlayerPrefs.GetInt(PLAYER_PREFS_KEY);
        mainMenuAndHUD.UpdateHighestScore(highestScore);
    }

    public void ScorePoints(int pointsIncrement)
    {
        // Check if score is enough to player.EarnALife() by some modulus calculus. And update score value in the proccess
        this.points += pointsIncrement;
        if (this.points > scoresToEarnLife[scoresToEarnLifeIndex])
        {
            scoresToEarnLifeIndex++;
            player.EarnALife();
        }

        mainMenuAndHUD.UpdateScore(this.points);
    }

    public void UpdateHighestScoreAndClearScore()
    {
        if (points > highestScore)
        {
            highestScore = points;
            mainMenuAndHUD.UpdateHighestScore(highestScore);

            PlayerPrefs.SetInt(key: PLAYER_PREFS_KEY, value: highestScore);
            PlayerPrefs.Save();
        }

        points = 0;
        mainMenuAndHUD.UpdateScore(points);
    }
}
