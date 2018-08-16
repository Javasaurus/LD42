using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightAudioSwap : MonoBehaviour
{
    BoxCollider2D m_collider;
    public AudioClip bossMusic;
    private PlayMusic music;
    Transform player;
    AudioSource bgmAudioSource;
    AudioClip originalClip;
    Camera gameCamera;
    bool bossFightActive;
    // Use this for initialization
    WaterRising water;

    void Start()
    {
        m_collider = GetComponent<BoxCollider2D>();
        bgmAudioSource = GameObject.FindGameObjectWithTag("BGM_AUDIO").GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        music = GameObject.FindObjectOfType<PlayMusic>();
        originalClip = bgmAudioSource.clip;
        water = GameObject.FindObjectOfType<WaterRising>();
    }

    void LateUpdate()
    {
        if (!bgmAudioSource)
        {
            return;
        }
        CheckBounds();
        if (bossFightActive && bgmAudioSource.clip != bossMusic)
        {
            bgmAudioSource.clip = bossMusic;
        }
        else if (!bossFightActive && bgmAudioSource.clip != originalClip)
        {
            bgmAudioSource.clip = originalClip;
        }
        if (bossFightActive)
        {
            if (water)
            {
                water.gameObject.SetActive(false);
            }
            else
            {
                water.gameObject.SetActive(true);
            }
        }
    }

    private void OnDisable()
    {
        if (bgmAudioSource)
        {
            bgmAudioSource.clip = originalClip;
        }
    }

    private void CheckBounds()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (player)
        {
            bossFightActive = Mathf.Abs(Vector2.Distance(player.position, transform.position)) < m_collider.bounds.size.y;
        }
    }

}
