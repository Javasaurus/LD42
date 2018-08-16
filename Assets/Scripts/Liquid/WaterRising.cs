using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WaterRising : MonoBehaviour
{

    private Transform m_Player;
    private AudioSource m_AudioSource;

    [Range(1, 100)]
    public float maxDistance = 50f;
    public float multiplier;

    public float risingSpeed;       //The speed of the rising liquid
    public float speedIncrease;     //The acceleration (depending on the level we are at I presume)

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_Player = GameObject.FindObjectOfType<PlayerAnimator>().transform;
    }

    public void Update()
    {
        risingSpeed += speedIncrease * Time.deltaTime;
        transform.position += new Vector3(0, risingSpeed * Time.deltaTime, 0);
    }


    public void LateUpdate()
    {
        m_AudioSource.volume = 1-((m_Player.position.y - transform.position.y) / maxDistance);

    }

}
