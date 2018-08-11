using UnityEngine;

public class Jetpack : MonoBehaviour
{

    public KeyCode jetkey;
    public KeyCode left;
    public KeyCode right;
    public float jetfuel;
    public float jetup;
    public float xSpeed;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (jetfuel > 0 && Input.GetKeyDown(jetkey))
        {
            //gameObject.GetComponent<Player>().enabled = false;
            //rb.constraints = RigidbodyConstraints2D.None;
            //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void FixedUpdate()
    {
        if (jetfuel > 0 && Input.GetKey(jetkey))
        {
            Inflight();
        }
        else
        {
            //gameObject.GetComponent<Player>().enabled = true;
            //rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    void Inflight()
    {
        Vector2 desiredDirection = new Vector2();
        jetfuel -= Time.deltaTime;
        if (Input.GetKey(left))
        {
            desiredDirection.x -= xSpeed;
        }
        if (Input.GetKey(right))
        {
            desiredDirection.x += xSpeed;
        }
        desiredDirection.y = jetup;
        rb.MovePosition(rb.position + desiredDirection * Time.fixedDeltaTime);
    }

}