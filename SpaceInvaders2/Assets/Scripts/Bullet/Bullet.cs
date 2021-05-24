using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    protected float speed = 0.0f;

    [SerializeField]
    private GameObject redExplosionPrototype = null;

    [SerializeField]
    private GameObject whiteExplosionPrototype = null;

    [SerializeField]
    private int immuneLayer = -1;

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
                    // Check for active because as the as destruction can't be immediate, we also do SetActive(false)
                    // That covers the case of the bullet entering two damageable colliders in the same frame: 
                    // only one of them should trigger the proccessing
                    if (gameObject.activeInHierarchy 
                        // Check immune layer to prevent aliens from accidentaly damage aliens
                        && other.gameObject.layer != immuneLayer)
                    {
                        Instantiate(whiteExplosionPrototype, other.transform.position, Quaternion.identity);
                        other.GetComponent<Damageable>().Die();
                        gameObject.SetActive(false);
                    }
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
