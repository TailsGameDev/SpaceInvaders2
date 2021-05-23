using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement
    [SerializeField]
    private float speed = 0.0f;
    [SerializeField]
    private Transform rightLimit = null;
    private float y, z;

    private void Awake()
    {
        // It's important to let this script active and enable at beggining, so it can set y and x as soon as possible
        this.enabled = false;
        y = transform.position.y;
        z = transform.position.z;
    }

    // Movement logic
    private void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float newX = transform.position.x + (horizontal * speed);
        if (Mathf.Abs(newX) < rightLimit.position.x)
        {
            transform.position = new Vector3(newX, y, z);
        }
    }

    public void ResetPosition()
    {
        transform.position = new Vector3(-rightLimit.position.x, y, z);
    }
}
