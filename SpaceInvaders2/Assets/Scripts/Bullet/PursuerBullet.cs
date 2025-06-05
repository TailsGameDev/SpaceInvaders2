using UnityEngine;

public class PursuerBullet : Bullet
{
    [SerializeField]
    private Rigidbody2D rb2d = null;
    [SerializeField]
    private float initialSpeed = 0.0f;
    private Transform target;
    private static GameObject aim;

    public Transform Target { set => target = value; }
    public static GameObject Aim { set => aim = value; }

    private void Awake()
    {
        rb2d.linearVelocity = transform.up * initialSpeed;
    }

    protected override void FixedUpdate()
    {
        // Point to target if it's alive
        if (target.gameObject.activeInHierarchy)
        {
            Vector3 direction = target.position - transform.position;
            transform.up = direction;
        }

        rb2d.AddForce(transform.up * speed);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        aim.SetActive(false);

        base.OnTriggerEnter2D(other);
    }
}
