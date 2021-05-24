using UnityEngine;

public class UISkinsCarousel : MonoBehaviour
{
    [SerializeField]
    private UISkinHUD[] uiSkinHUDs = null;

    [SerializeField]
    private string inputName = null;
    
    private int currentIndex;
    private int lastValidIndex;
    private float lastFrameInputRawAxisValue;

    private void Update()
    {
        float inputRawAxisValue = Input.GetAxisRaw(inputName);
        bool isInputFresh = Mathf.Approximately(lastFrameInputRawAxisValue, 0.0f) && ( ! Mathf.Approximately(inputRawAxisValue, 0.0f));
        if (isInputFresh)
        {
            uiSkinHUDs[currentIndex].ShowOrHide(show: false);

            // Calculate new currentIndex based on Input
            {
                
                int indexIncrement = inputRawAxisValue > 0.0f ? 1 : -1;
                // Increment index but let it inside of bounds in a circular behaviour.
                this.currentIndex = Mod(dividend: (currentIndex + indexIncrement), divisor: uiSkinHUDs.Length);
            }

            UISkinHUD skinToDisplay = uiSkinHUDs[currentIndex];
            skinToDisplay.ShowOrHide(show: true);
            
            if (skinToDisplay.IsAvailable())
            {
                lastValidIndex = currentIndex;
            } 
        }
        this.lastFrameInputRawAxisValue = inputRawAxisValue;
    }
    private int Mod(int dividend, int divisor)
    {
        return (dividend % divisor + divisor) % divisor;
    }

    public Color GetLastValidColorSelected()
    {
        return uiSkinHUDs[lastValidIndex].GetColor();
    }
}
