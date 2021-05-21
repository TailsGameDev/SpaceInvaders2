using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.0f;

    [SerializeField]
    private GameObject redExplosionPrototype = null;

    [SerializeField]
    private GameObject whiteExplosionPrototype = null;

    private void FixedUpdate()
    {
        transform.position += (transform.up * speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "damageable":
                Instantiate(whiteExplosionPrototype, other.transform.position, Quaternion.identity);
                other.GetComponent<Enemy>().Die();
                Destroy(gameObject);
                break;
            case "screen_end":
                Player.CurrentBullet = null;
                Instantiate(redExplosionPrototype, transform.position, Quaternion.identity);
                Destroy(gameObject);
                break;
        }
    }
}
