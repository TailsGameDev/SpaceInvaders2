using UnityEngine;

public class UISkinsCarousel : MonoBehaviour
{
    [SerializeField]
    private UISkinHUD[] uiSkinHUDs = null;

    [SerializeField]
    private string inputName = null;
    
    private int currentIndex;
    private int lastValidIndex;

    private void Update()
    {
        // Input and decision logic
        bool inputPressed = Input.GetButtonDown(inputName);
        if (inputPressed)
        {
            uiSkinHUDs[currentIndex].ShowOrHide(show: false);

            // Calculate new currentIndex based on Input
            {
                float inputDirection = Input.GetAxisRaw(inputName);
                int indexIncrement = inputDirection > 0.0f ? 1 : -1;
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
