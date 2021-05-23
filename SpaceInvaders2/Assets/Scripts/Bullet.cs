using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    protected float speed = 0.0f;

    [SerializeField]
    private GameObject redExplosionPrototype = null;

    [SerializeField]
    private GameObject whiteExplosionPrototype = null;

    protected virtual void FixedUpdate()
    {
        transform.position += (transform.up * speed);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.activeInHierarchy)
        {
            switch (other.tag)
            {
                case "damageable":
                    Instantiate(whiteExplosionPrototype, other.transform.position, Quaternion.identity);
                    other.GetComponent<Damageable>().Die();
                    gameObject.SetActive(false);
                    Destroy(gameObject);
                    break;
                case "screen_end":
                    Instantiate(redExplosionPrototype, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                    break;
            }
        }
    }
}
