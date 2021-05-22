using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierPiece : MonoBehaviour
{
    /*
    [SerializeField]
    private Sprite[] maskingSpriteOptions = null;
    */
    [SerializeField]
    private SpriteMask spriteMask = null;
    [SerializeField]
    private Collider2D col = null;

    private static List<BarrierPiece> allBarrierPieces = new List<BarrierPiece>();

    private static PlayerDamageable playerDamageable;

    public static PlayerDamageable PlayerDamageable { set => playerDamageable = value; }

    private void Awake()
    {
        allBarrierPieces.Add(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "damageable")
        {
            // Game Over
            playerDamageable.LifesAmount = 1;
            playerDamageable.Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "bullet" && other.gameObject.activeInHierarchy)
        {
            spriteMask.enabled = true;
            col.enabled = false;

            other.gameObject.SetActive(false);
            Destroy(other.gameObject);
        }
    }

    public static void EnableAllPieces()
    {
        foreach (BarrierPiece piece in allBarrierPieces)
        {
            piece.spriteMask.enabled = false;
            piece.col.enabled = true;
        }
    }
}
