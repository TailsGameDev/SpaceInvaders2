using UnityEngine;
using UnityEngine.UI;

public class UIAnimatedText : MonoBehaviour
{
    [SerializeField]
    private Text text = null;

    [SerializeField]
    [TextArea]
    private string originalText = null;

    [SerializeField]
    private float delayForEachLetter = 0.0f;
    private float timeToShowNextLetter = 0.0f;
    private int textIndex;

    public void StartWritting()
    {
        text.enabled = true;
        text.text = "";
        textIndex = 0;
        this.enabled = true;
    }
    public void Hide()
    {
        text.enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Time.time > timeToShowNextLetter)
        {
            timeToShowNextLetter = Time.time + delayForEachLetter;

            text.text += originalText[textIndex];
            textIndex++;

            if (textIndex >= originalText.Length)
            {
                this.enabled = false;
            }
        }
    }
}
