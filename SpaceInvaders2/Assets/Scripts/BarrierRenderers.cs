using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierRenderers : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer[] allRenderers = null;

    public void SetAllBarriersColor(Color newColor)
    {
        foreach (SpriteRenderer renderer in allRenderers)
        {
            renderer.color = newColor;
        }
    }
}
