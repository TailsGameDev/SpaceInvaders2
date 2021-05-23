using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkinsUser : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer = null;
    [SerializeField]
    private Image image = null;

    private static readonly List<PlayerSkinsUser> allPlayerSkinsUsers = new List<PlayerSkinsUser>();

    private static Color currentSkinColor;

    public static void SetAllPlayerSkinsUserColor (Color newColor)
    { 
        currentSkinColor = newColor; 
        UpdateAllPlayerSkinsUserColor();
    }

    private void Awake()
    {
        allPlayerSkinsUsers.Add(this);
    }
    private void OnDestroy()
    {
        allPlayerSkinsUsers.Remove(this);
    }

    private void OnEnable()
    {
        UpdateColor();
    }

    private static void UpdateAllPlayerSkinsUserColor()
    {
        foreach (PlayerSkinsUser user in allPlayerSkinsUsers)
        {
            user.UpdateColor();
        }
    }

    private void UpdateColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = currentSkinColor;
        }
        else
        {
            image.color = currentSkinColor;
        }
    }
}
