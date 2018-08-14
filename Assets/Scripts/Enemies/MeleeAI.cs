using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MeleeAI : MonoBehaviour
{

    public float minTime = 2f;
    public float maxTime = 4f;
    private float timer;
    private Animator m_Anim;
    // Update is called once per frame
    void Update()
    {
        if (Time.time > timer)
        {
            m_Anim.SetTrigger("Attack");
            timer = Time.time + Random.Range(minTime, maxTime);
        }
    }
}
