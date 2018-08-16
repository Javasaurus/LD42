using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class ExplodingBarrel : MonoBehaviour
{
    public float timeTillDetonation = 10f;
    public bool isActive;

    public bool explode;

    private bool exploding;
    Vector3 spawn;

    private bool respawning;

    void SetExploding()
    {
        m_Animator.SetBool("Explode", true);
    }

    Animator m_Animator;

    // Use this for initialization

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        spawn = transform.position;
    }

    IEnumerator RandomActivate()
    {
        yield return new WaitForSeconds(Random.Range(5f, 15f));
        isActive = true;
    }

    IEnumerator ReActivate(float time)
    {
        yield return new WaitForSeconds(time);
        transform.position = spawn;
        GetComponent<SpriteRenderer>().enabled = true;
        respawning = false;
    }

    void ReSpawnMe()
    {
        if (!respawning)
        {
            respawning = true;
            GetComponent<SpriteRenderer>().enabled = false;
            m_Animator.SetTrigger("Reset");
            m_Animator.SetBool("Explode", false);
            m_Animator.SetBool("Activate", false);
            m_Animator.SetFloat("TimeLeft", timeTillDetonation);
            explode = false;
            exploding = false;
            StartCoroutine(ReActivate(Random.Range(3f, 5f)));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!respawning)
        {
            if (timeTillDetonation < 0)
            {
                SetExploding();
                this.enabled = false;
            }
            else
            {
                m_Animator.SetBool("Activate", isActive);
                if (isActive)
                {
                    timeTillDetonation -= Time.deltaTime;
                    m_Animator.SetFloat("TimeLeft", timeTillDetonation);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if a boss collides with the barrel itself OR if the big collider explodes
        if (respawning)
        {
            return;
        }

        if (exploding)
        {
            if (collision.tag == "Player")
            {
                collision.GetComponent<Health>().DamagePlayer(3, 1);
            }
        }
        else
        {
            if (collision.tag == "PlayerProjectile")
            {
                GameObject.Destroy(collision.gameObject);
                isActive = true;
            }
        }
        // crusher boss 
        if (/*isActive &&*/ collision.tag == "CrusherBoss")
        {
            SetExploding();
        }
    }
}

