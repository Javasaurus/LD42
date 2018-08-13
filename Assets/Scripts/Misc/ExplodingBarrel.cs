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
    void SetExploding()
    {
        Debug.Log("BOOOM");
        m_Animator.SetBool("Explode", true);
    }

    Animator m_Animator;

    // Use this for initialization
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        spawn = transform.position;
    }

    IEnumerator ReActivate(float time)
    {
        yield return new WaitForSeconds(time);
        Instantiate(this, spawn, Quaternion.identity, transform.parent);
        GameObject.Destroy(this.gameObject);
    }

    void ReSpawnMe()
    {
        m_Animator.SetBool("Explode", false);
        explode = false;
        exploding = false;
        StartCoroutine(ReActivate(Random.Range(3f, 5f)));
    }

    // Update is called once per frame
    void Update()
    {


        if (timeTillDetonation < 0)
        {
            SetExploding();
            GameObject.Destroy(this, 2f);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if a boss collides with the barrel itself OR if the big collider explodes

        if (exploding)
        {
            if (collision.tag == "Player")
            {
                collision.GetComponent<Health>().DamagePlayer();
            }

        }
        // crusher boss 
        if (/*isActive &&*/ collision.tag == "CrusherBoss")
        {
            SetExploding();
            collision.GetComponent<CrusherBoss>().health--;
        }
    }

}
