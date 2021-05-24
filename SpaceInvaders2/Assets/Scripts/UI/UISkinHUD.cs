using UnityEngine;
using UnityEngine.UI;

public class UISkinHUD : MonoBehaviour
{
    [SerializeField]
    private Image skinImage = null;

    [SerializeField]
    private int requiredHighestScore = 0;

    [SerializeField]
    private Text requiredScoreText = null;

    [SerializeField]
    private GameObject lockImage = null;

    private static Score scoreReference;

    public static Score ScoreReference { set => scoreReference = value; }

    private void Awake()
    {
        // Paint coloredRequiredPoints on text
        string colorHexCode = ColorUtility.ToHtmlStringRGB(skinImage.color);
        string coloredRequiredPoints = "<color=#" + colorHexCode + ">" + requiredHighestScore + "</color>";
        string newText = requiredScoreText.text.Replace("0000", coloredRequiredPoints);
        requiredScoreText.text = newText;
    }
    private void OnEnable()
    {
        if (IsAvailable())
        {
            string newText = requiredScoreText.text.Replace("SCORE TO UNLOCK ", "");
            requiredScoreText.text = newText;
        }
    }

    public Color GetColor()
    {
        return skinImage.color;
    }

    public void ShowOrHide(bool show)
    {
        lockImage.gameObject.SetActive( show && IsLocked() );
        gameObject.SetActive(show);
    }

    public bool IsAvailable()
    {
        return scoreReference.HighestScore >= requiredHighestScore;
    }
    public bool IsLocked()
    {
        return scoreReference.HighestScore < requiredHighestScore;
    }
}
