using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkinsMenu : MonoBehaviour
{
    [SerializeField]
    private UISkinsCarousel barrierSkinsCarousel = null;
    [SerializeField]
    private UISkinsCarousel playerSkinsCarousel = null;

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
