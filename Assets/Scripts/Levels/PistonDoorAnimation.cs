using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PistonDoorAnimation : DoorAnimation
{
    Animator animator;
    AudioSource m_Audiosource;
    public GameObject sealObject;
    public GameObject[] collidersToReplace;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    public override void SealRoom()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("Closed", true);
        }
    }

    public void PlaySoundEffect()
    {
        if (m_Audiosource == null)
        {
            m_Audiosource = GetComponent<AudioSource>();
        }
        if (!m_Audiosource.isPlaying)
        {
            m_Audiosource.PlayOneShot(m_Audiosource.clip);
        }
    }

    protected override void Resume()
    {
        if (sealObject)
        {
            foreach (GameObject collider in collidersToReplace)
            {
                foreach (Collider2D partialCollider in collider.GetComponents<Collider2D>())
                {
                    partialCollider.enabled = false;
                }
            }
            sealObject.SetActive(true);
        }
        waiting = true;
    }


    private bool waiting;
    private float waitTime = 0.15f;
    void Update()
    {
        if (waiting && waitTime > 0)
        {
            waitTime -= Time.unscaledDeltaTime;
            if (waitTime <= 0)
            {
                base.Resume();
                waiting = false;
            }
        }
    }


}
