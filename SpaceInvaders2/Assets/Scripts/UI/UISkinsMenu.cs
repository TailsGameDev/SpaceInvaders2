using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkinsMenu : MonoBehaviour
{
    [SerializeField]
    private UISkinsCarousel barrierSkinsCarousel = null;
    [SerializeField]
    private UISkinsCarousel playerSkinsCarousel = null;

    private void Update()
    {
        // Filter a little bit the input passed for the UISkinsCarousels so only one of them execute at a time
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool isHorizontalGreatest = Mathf.Abs(horizontal) > Mathf.Abs(vertical);
        if (isHorizontalGreatest)
        {
            barrierSkinsCarousel.TreatInput(0.0f);
            playerSkinsCarousel.TreatInput(horizontal);
        }
        else
        {
            barrierSkinsCarousel.TreatInput(vertical);
            playerSkinsCarousel.TreatInput(0.0f);
        }

    }

    public void ShowUp()
    {
        gameObject.SetActive(true);
    }
    public void Disappear()
    {
        gameObject.SetActive(false);
    }

    public Color GetBarrierColor()
    {
        return barrierSkinsCarousel.GetLastValidColorSelected();
    }
    public Color GetPlayerColor()
    {
        return playerSkinsCarousel.GetLastValidColorSelected();
    }
}
