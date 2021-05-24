using UnityEngine;

public class UISkinsCarousel : MonoBehaviour
{
    [SerializeField]
    private UISkinHUD[] uiSkinHUDs = null;

    [SerializeField]
    private float delayToShangeSkinIfKeepPressingInput = 0.0f;
    private float timeToMakeInputFresh;

    private int currentIndex;
    private int lastValidIndex;
    private float lastFrameInputRawAxisValue;

    public void TreatInput(float inputRawAxisValue)
    {
        bool isPressingInput = !Mathf.Approximately(inputRawAxisValue, 0.0f);
        if (isPressingInput)
        {
            if (
                // Input was zero last frame
                (Mathf.Approximately(lastFrameInputRawAxisValue, 0.0f)) 
                // Or pressed Input long enough to switch skin again
                || Time.time > timeToMakeInputFresh)
            {
                timeToMakeInputFresh = Time.time + delayToShangeSkinIfKeepPressingInput;

                uiSkinHUDs[currentIndex].ShowOrHide(show: false);

                // Calculate new currentIndex based on Input
                {
                
                    int indexIncrement = inputRawAxisValue > 0.0f ? 1 : -1;
                    // Increment index but let it inside of bounds in a circular behaviour.
                    this.currentIndex = Mod.mod(dividend: (currentIndex + indexIncrement), divisor: uiSkinHUDs.Length);
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
    }

    public Color GetLastValidColorSelected()
    {
        return uiSkinHUDs[lastValidIndex].GetColor();
    }
}
