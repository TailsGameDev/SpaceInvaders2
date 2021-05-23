using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField]
    private UserInterface userInterface = null;
    [SerializeField]
    private PlayerDamageable player = null;
    [SerializeField]
    private int scoreToEarnALife = 0;

    private int score;
    private int highestScore;

    public void ScorePoints(int points)
    {
        // Check if score is enough to player.EarnALife() by some modulus calculus. And update score value in the proccess
        int remainder = score % scoreToEarnALife;
        score += points;
        int secondRemainder = score % scoreToEarnALife;
        if (secondRemainder < remainder)
        {
            player.GainALife();
        }

        userInterface.UpdateScore(score);
    }

    public void UpdateHighestScoreAndClearScore()
    {
        if (score > highestScore)
        {
            highestScore = score;
            userInterface.UpdateHighestScore(highestScore);
        }

        score = 0;
        userInterface.UpdateScore(score);
    }
}
