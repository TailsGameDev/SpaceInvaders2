using UnityEngine;

public class Skins : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer[] barrierRenderers = null;
    [SerializeField]
    private SpriteRenderer playerRenderer = null;

    public void SetBarriersColor(Color color)
    {
        foreach (SpriteRenderer barrier in barrierRenderers)
        {
            barrier.color = color;
        }
    }
    public void SetPlayerColor(Color color)
    {
        playerRenderer.color = color;
    }
}
