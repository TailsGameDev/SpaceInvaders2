using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseSymbol = null;

    private void Update()
    {
        if (Input.GetButtonDown("Submit") && Time.timeScale > 0.1f)
        {
            Time.timeScale = 0.0f;
            pauseSymbol.SetActive(true);
        }
        else if (Time.timeScale < 0.1f && (Input.GetButtonDown("Submit")||Input.GetButtonDown("Cancel")))
        {
            Time.timeScale = 1.0f;
            pauseSymbol.SetActive(false);
        }
    }

    public bool IsGamePaused()
    {
        return Time.timeScale < 0.1f;
    }
}
