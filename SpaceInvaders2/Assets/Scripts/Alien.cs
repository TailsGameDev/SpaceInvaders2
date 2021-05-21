using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    [SerializeField]
    private Sprite otherSprite = null;

    [SerializeField]
    private int pointsToScoreOnDeath = 0;

    public float X { get => transform.position.x; }
    public bool IsAlive { get => gameObject.activeSelf; }
    public static AliensGrid EnemyGrid { set => AlienDamageable.EnemyGrid = value; }
    public int PointsToScoreOnDeath { get => pointsToScoreOnDeath; }

    public void MoveAndAnimate(float x, float y)
    {
        // Change sprite
        Sprite aux = spriteRenderer.sprite;
        spriteRenderer.sprite = otherSprite;
        otherSprite = aux;

        transform.position += new Vector3(x, y, 0);
    }
}
