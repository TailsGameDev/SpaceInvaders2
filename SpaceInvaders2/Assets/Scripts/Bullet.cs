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
                    // SetActive(false) so other scripts can check for this.gameObject.actifeSelf, as destruction can't be immediate.
                    gameObject.SetActive(false);
                    break;
                case "screen_end":
                    Instantiate(redExplosionPrototype, transform.position, Quaternion.identity);
                    break;
                case "bullet":
                    Vector3 averagePosition = (transform.position + other.transform.position)/2;
                    Instantiate(redExplosionPrototype, averagePosition, Quaternion.identity);
                    // Set other active to false so it does not execute it's OntriggerEnter or anything else.
                    other.gameObject.SetActive(false);
                    Destroy(other.gameObject);
                    break;
            }
            Destroy(gameObject);
        }
    }
}
