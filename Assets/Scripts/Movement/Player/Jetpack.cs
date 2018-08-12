using UnityEngine;

[RequireComponent(typeof(CharacterInput))]
public class Jetpack : MonoBehaviour
{
    public bool JetPackActivated;

    public float jetfuel;
    public float jetup;
    public float xSpeed;

    CharacterInput m_Input;
    RigidbodyConstraints2D constraints;
    Rigidbody2D rb;

    void Start()
    {
        m_Input = GetComponent<CharacterInput>();
        rb = GetComponent<Rigidbody2D>();
        constraints = rb.constraints;
    }

    void Update()
    {
        if (jetfuel > 0 & m_Input.Vertical > 0.05f)
        {
            gameObject.GetComponent<PlatformerCharacter2D>().enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void FixedUpdate()
    {
        if ((jetfuel > 0 & m_Input.Vertical > 0.05f))
        {
            Inflight();
        }
        else
        {
            gameObject.GetComponent<PlatformerCharacter2D>().enabled = true;
            rb.constraints = constraints;
        }
    }

    void Inflight()
    {
        Vector2 desiredDirection = new Vector2();
        jetfuel -= Time.deltaTime;
        if (Mathf.Abs(m_Input.Horizontal) > 0.05)
        {
            desiredDirection.x += (Mathf.Sign(m_Input.Horizontal) * xSpeed);
        }
        else
        {
            desiredDirection.x *= 0.85f;
            if (Mathf.Abs(desiredDirection.x) < 0.05f)
            {
                desiredDirection.x = 0;
            }
        }
        JetPackActivated = true;
        desiredDirection.y = jetup * m_Input.Vertical;
        rb.MovePosition(rb.position + desiredDirection * Time.fixedDeltaTime);
    }

}