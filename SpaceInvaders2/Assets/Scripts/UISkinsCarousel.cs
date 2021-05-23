using UnityEngine;
using UnityEngine.UI;

public class UISkinsCarousel : MonoBehaviour
{
    [SerializeField]
    private Image[] images = null;

    [SerializeField]
    private string inputName = null;
    
    private int currentIndex;
    private float desiredRotation;
    /*
    [SerializeField]
    private float tweenPeriod = 0.0f;
    private float initialTweenRotation;
    private float currentRotation;
    private bool isRotating;
    private float timeToEndRotationTween;
    */

    private void Update()
    {
        // Input and decision logic
        bool inputPressed = Input.GetButtonDown(inputName);
        if (inputPressed)
        {
            float inputDirection = Input.GetAxisRaw(inputName);
            if (inputDirection > 0.0f)
            {
                StartRotationToRight();
            }
            else if (inputDirection < 0.0f)
            {
                StartRotationToLeft();
            }
        }
        
        /* // Rotation logic
        if (isRotating)
        {
            currentRotation = LeanTween.easeInOutElastic(start: initialTweenRotation, end: desiredRotation, val: currentRotation);

            isRotating = (Time.time < timeToEndRotationTween);
        }
        */
    }
    private void StartRotationToRight()
    {
        IncrementIndexThenRotate(indexIncrement: 1);
    }
    private void StartRotationToLeft()
    {
        IncrementIndexThenRotate(indexIncrement: -1);
    }
    private void IncrementIndexThenRotate(int indexIncrement)
    {
        this.currentIndex = Mod( dividend:(currentIndex + indexIncrement), divisor: images.Length );
        this.desiredRotation = (360.0f / images.Length) * this.currentIndex;

        Vector3 angles = transform.localEulerAngles;
        angles.z = desiredRotation;
        transform.localEulerAngles = angles;

        /*
        this.initialTweenRotation = currentRotation;
        this.timeToEndRotationTween = Time.time + tweenPeriod;

        this.isRotating = true;
        */
    }
    private int Mod(int dividend, int divisor)
    {
        return (dividend % divisor + divisor) % divisor;
    }

    public Color GetCurrentColor()
    {
        return images[currentIndex].color;
    }
}
