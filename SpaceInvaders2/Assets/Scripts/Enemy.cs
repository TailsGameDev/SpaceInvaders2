using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    [SerializeField]
    private Sprite otherSprite = null;

    private static EnemyGrid enemyGrid;

    public float X { get => transform.position.x; }
    public bool IsAlive { get => gameObject.activeSelf; }
    public static EnemyGrid EnemyGrid { set => enemyGrid = value; }

    public void MoveAndAnimate(float x, float y)
    {
        // Change sprite
        Sprite aux = spriteRenderer.sprite;
        spriteRenderer.sprite = otherSprite;
        otherSprite = aux;

        transform.position += new Vector3(x, y, 0);
    }

    public void Die()
    {
        enemyGrid.Accelerate();
        gameObject.SetActive(false);
    }
}
