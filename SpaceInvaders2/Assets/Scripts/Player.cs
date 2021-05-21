using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.0f;

    [SerializeField]
    private Transform rightLimit = null;

    [SerializeField]
    private Bullet bulletPrototype = null;

    [SerializeField]
    private Transform bulletSpawnPoint = null;

    private static Bullet currentBullet;

    private Transform Transform 
    {
        get 
        {
            return transform;
        }
    }

    public static Bullet CurrentBullet { set => currentBullet = value; }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        float newX = Transform.position.x + (horizontal * speed);

        if (Mathf.Abs(newX) < rightLimit.position.x)
        {
            Transform.position = new Vector3(newX, Transform.position.y, Transform.position.z);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && currentBullet == null)
        {
            currentBullet = Instantiate(bulletPrototype, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        }
    }
}
