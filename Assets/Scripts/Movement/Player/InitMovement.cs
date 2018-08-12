using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitMovement : MonoBehaviour
{

    Rigidbody2D rb;
    Vector3 poslast;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (transform.position == poslast)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                rb.velocity += new Vector2(-10, 0);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                rb.velocity += new Vector2(10, 0);
            }
            Debug.Log("a");
        }
        else
            Debug.Log("b");
        if (Input.GetKeyDown(KeyCode.D) && rb.velocity.x < 0)
        {
            rb.velocity = new Vector2(0, 0);
        }
        if (Input.GetKeyDown(KeyCode.A) && rb.velocity.x > 0)
        {
            rb.velocity = new Vector2(0, 0);
        }
        rb.velocity = rb.velocity * 0.9f;
        if (rb.velocity.x < 0.3 && rb.velocity.x > 0)
        {
            rb.velocity = new Vector2(0, 0);
        }
        if (rb.velocity.x > -0.3 && rb.velocity.x < 0)
        {
            rb.velocity = new Vector2(0, 0);
        }
        poslast = transform.position;
    }
}
