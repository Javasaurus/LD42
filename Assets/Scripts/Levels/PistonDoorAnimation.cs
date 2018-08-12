using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PistonDoorAnimation : DoorAnimation
{
    Animator animator;
    public GameObject sealObject;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    public override void SealRoom()
    {
        if (animator != null)
        {
            animator.SetBool("Closed", true);
        }
    }

    protected override void Resume()
    {
        if (sealObject)
        {
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
