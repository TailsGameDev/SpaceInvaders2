using System.Collections.Generic;
using UnityEngine;

public class BarrierPiece : MonoBehaviour
{
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
    public static void EnableAllPieces()
    {
        foreach (BarrierPiece piece in allBarrierPieces)
        {
            piece.spriteMask.enabled = false;
            piece.col.enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "damageable")
        {
            playerDamageable.LoseAllLifesAndDie();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "bullet" && other.gameObject.activeInHierarchy)
        {
            spriteMask.enabled = true;
            col.enabled = false;

            // SetActive(false) so other OnTriggerEnters in this frame can check for active, 
            // and then they'll know if the bullet collision was already treated or not.
            // NOTE: Unity didn't let me DestroyImmediate(bullet) inside this method.
            other.gameObject.SetActive(false);
            Destroy(other.gameObject);
        }
    }
}
