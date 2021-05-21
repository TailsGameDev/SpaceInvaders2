using UnityEngine;

public class SimplifiedAnimator : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    [SerializeField]
    private float delayToChangeSprite = 0.0f;

    [SerializeField]
    private Sprite[] sprites = null;

    private float timeToChangeSprite;
    private int spriteIndex;

    private void FixedUpdate()
    {
        if (Time.time > timeToChangeSprite)
        {
            spriteRenderer.sprite = sprites[spriteIndex];

            spriteIndex = (spriteIndex + 1) % sprites.Length;

            timeToChangeSprite = Time.time + delayToChangeSprite;
        }
    }
}
