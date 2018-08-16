using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Health : MonoBehaviour
{
    public static bool DEAD;
    Collider2D m_Collider;
    SpriteRenderer m_Renderer;

    public GameObject deathUI;
    public GameObject heartIcon;

    public Sprite good, bad;

    public Transform heartsParent;

    public int currDamage;

    public int startingHearts;
    public int maxHearts;

    public int score;

    public int hearts;

    protected bool flashing;

    protected Coroutine flashRoutine;
    protected Coroutine dieRoutine;

    void Awake()
    {
        m_Collider = GetComponent<Collider2D>();
        m_Renderer = GetComponent<SpriteRenderer>();
        hearts = startingHearts;
    }

    private void LateUpdate()
    {
        if (hearts < 1)
        {
            DEAD = true;
            HandleDeath();
        }
    }

    public void DamagePlayer(int amount)
    {
        if (!flashing)
        {
            EZCameraShake.CameraShaker.Instance.ShakeOnce(2.9f, 2.7f, 0.1f, 0.7f);
            hearts -= amount;
            if (hearts > 1)
            {
                if (dieRoutine != null && flashRoutine != null)
                {
                    StopCoroutine(flashRoutine);
                    flashRoutine = null;
                }
                flashRoutine = StartCoroutine(Flash(0.1f));
            }
            else
            {
                HandleDeath();
            }
        }
    }

    protected virtual void HandleDeath()
    {
        Gameover();
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


    public void Gameover()
    {
        DEAD = true;
        StartCoroutine(HandleGameOver());
    }

    IEnumerator HandleGameOver()
    {
        yield return null;
        deathUI.SetActive(true);
        //Time.timeScale = 0;
    }

}
