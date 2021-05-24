using UnityEngine;

public class Alien : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    [SerializeField]
    private Sprite otherSprite = null;

    [SerializeField]
    private int pointsToScoreOnDeath = 0;

    private Vector3 originalPosition;

    public float X { get => transform.position.x; }
    public bool IsAlive { get => gameObject.activeSelf; }
    public static AliensGrid AlienGrid { set => AlienDamageable.AliensGrid = value; }
    public int PointsToScoreOnDeath { get => pointsToScoreOnDeath; }

    public void MemorizeOriginalPosition()
    {
        this.originalPosition = transform.position;
    }

    public void MoveAndAnimate(float x, float y)
    {
        // Change sprite
        Sprite aux = spriteRenderer.sprite;
        spriteRenderer.sprite = otherSprite;
        otherSprite = aux;

        transform.position += new Vector3(x, y, 0);
    }

    public void DoReset()
    {
        transform.position = originalPosition;

        gameObject.SetActive(true);
    }

    public void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }
}
