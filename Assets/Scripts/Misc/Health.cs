using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
public class Health : MonoBehaviour
{
    public static bool DEAD;
    Collider2D m_Collider;
    SpriteRenderer m_Renderer;
    public AudioClip soundClip;
    AudioSource m_AudioSource;
    public GameObject deathUI;

    const int startingHearts = 6;
    public int hitsPerHeart = 3;
    private int hitCounter = 0;
    public int maxHearts;
    public int score;
    public int hearts;

    protected bool flashing;

    protected Coroutine flashRoutine;
    protected Coroutine dieRoutine;

    void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_Collider = GetComponent<Collider2D>();
        m_Renderer = GetComponent<SpriteRenderer>();
        hearts = startingHearts;
    }

    public void ResetStats()
    {
        hitCounter = 0;
        hearts = startingHearts;
    }


    private void LateUpdate()
    {
        if (hearts < 1)
        {
            DEAD = true;
            HandleDeath();
        }else if (hearts > maxHearts)
        {
            hearts = maxHearts;
        }
    }

    public void DirectlyDamagePlayer(int amount)
    {
        if (!flashing)
        {
            EZCameraShake.CameraShaker.Instance.ShakeOnce(2.9f, 2.7f, 0.1f, 0.7f);
            hearts -= amount;
            if (hearts >= 0)
            {
                if (dieRoutine != null && flashRoutine != null)
                {
                    StopCoroutine(flashRoutine);
                    flashRoutine = null;
                }
                m_AudioSource.PlayOneShot(soundClip);
                flashRoutine = StartCoroutine(Flash(0.1f));
            }
            else
            {
                HandleDeath();
            }
        }
    }


    public void DamagePlayer(int amount, int hitcount)
    {
        if (!flashing)
        {
            hitCounter += hitcount;
            EZCameraShake.CameraShaker.Instance.ShakeOnce(2.9f, 2.7f, 0.1f, 0.7f);
            if (hitCounter == hitsPerHeart)
            {
                hearts -= amount;
                if (hearts > 1)
                {
                    if (dieRoutine != null && flashRoutine != null)
                    {
                        StopCoroutine(flashRoutine);
                        flashRoutine = null;
                    }
                    m_AudioSource.PlayOneShot(soundClip);
                    flashRoutine = StartCoroutine(Flash(0.1f));
                }
                else
                {
                    HandleDeath();
                }
                hitCounter = 0;
            }
        }
    }

    protected virtual void HandleDeath()
    {
        PostGameMessage.END_MESSAGE = "You died !";
        Gameover(true);
    }



    IEnumerator Flash(float x)
    {
        flashing = true;
        Color tmp = m_Renderer.color;
        for (int i = 0; i < 10; i++)
        {
            m_Renderer.enabled = false;
            m_Renderer.color = Color.red;
            yield return new WaitForSeconds(x);
            m_Renderer.enabled = true;
            m_Renderer.color = tmp;
            yield return new WaitForSeconds(x);
        }
        flashing = false;
    }


    public void Gameover(bool died)
    {
        if (!ScoreTimer.STOP)
        {
            ScoreTimer.STOP = true;
            DEAD = died;
            StartCoroutine(HandleGameOver());
        }
    }

    public void ForceGameOver()
    {
        if (!ScoreTimer.STOP)
        {
            ScoreTimer.STOP = true;
            Debug.Log("Setting " + deathUI);
            deathUI.SetActive(true);
        }
    }

    IEnumerator HandleGameOver()
    {
        yield return null;
        deathUI.SetActive(true);
        //Time.timeScale = 0;
    }

}
