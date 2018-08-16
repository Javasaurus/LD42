using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootyGuyEnemy : BasicEnemy
{

    private AudioSource m_Audiosource;
    float interval = 5f;
    float m_Timer;
    bool hasShot;

    void Start()
    {
        m_Anim = GetComponent<Animator>();
        m_Renderer = GetComponent<SpriteRenderer>();
        m_Collider = GetComponent<Collider2D>();
        m_Timer = Time.time + interval;
        interval = Random.Range(3f, 6f);
    }

    public override void DoAI()
    {
        if (Time.time > m_Timer)
        {
            hasShot = true;
            m_Anim.SetTrigger("Attack");
            m_Timer = Time.time + interval;
        }
    }

    public void ResetShot()
    {
        hasShot = false;
        m_Timer = Time.time + interval;
    }


    public void PlayAudio()
    {
        if (!m_Audiosource)
        {
            m_Audiosource = GetComponent<AudioSource>();
        }
        // if (!m_Audiosource.isPlaying)
        {
            m_Audiosource.PlayOneShot(m_Audiosource.clip);
        }
    }

}
