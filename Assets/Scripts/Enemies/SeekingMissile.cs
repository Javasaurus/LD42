using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class SeekingMissile : MonoBehaviour
{
    Animator m_Animator;
    Quaternion m_initialRotation;
    Transform m_Target;
    public float speed;
    public float lifetime;

    bool exploded;
    bool exploding;
    private void Start()
    {
        m_initialRotation = transform.rotation;
        m_Animator = GetComponent<Animator>();
        m_Target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (exploded | exploding)
        {
            transform.rotation = m_initialRotation;
            return;
        }
        if (m_Target)
        {
            // Vector2 should work just as well if 2D only game
            Vector2 lookTarget = m_Target.transform.position - transform.position;

            float angle = Mathf.Atan2(lookTarget.y, lookTarget.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.position += new Vector3(lookTarget.x, lookTarget.y, 0).normalized * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" & (!exploded | exploding))
        {
            collision.GetComponent<Health>().DamagePlayer(1);
            m_Animator.SetTrigger("Explode");
            SelfDestruct();
        }
        else if (collision.GetComponent<PlayerBullet>())
        {
            Explode();
        }
    }

    public void Explode()
    {
        exploding = true;
        transform.rotation = m_initialRotation;
        m_Animator.SetTrigger("Explode");
    }

    void SelfDestruct()
    {
        exploded = true;
        GameObject.Destroy(this.gameObject, 1f);
        ScoreTimer.score += 500f;
    }

}
