using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class BasicEnemy : MonoBehaviour
{
    public bool invisible;

    public int enemyHealth;
    public ItemDrop[] drops;

    protected float nextDelay;
    protected float timer;
    protected float stopDelay;
    protected float stopTimer;

    protected Animator m_Anim;
    protected SpriteRenderer m_Renderer;
    protected Collider2D m_Collider;
    protected bool DamageAnimation;

    public bool isAttacking;

    protected Coroutine flashRoutine;
    protected Coroutine dieRoutine;

    protected int normalCount;



    public virtual void DoAI()
    {

        if (Time.time > timer)
        {
            isAttacking = true;
            if (normalCount <= 3)
            {
                m_Anim.SetTrigger("Attack");
            }
            else
            {
                normalCount = 0;
                m_Anim.SetTrigger("SpecialAttack");
            }
            timer = Time.time + 3f;
            stopTimer = Time.time + 5;
        }

    }

    protected virtual void HandleDeath()
    {
        StartCoroutine(Die());
    }

    void HandleEnemyStats()
    {
        if (dieRoutine == null && enemyHealth <= 0)
        {
            dieRoutine = StartCoroutine(Die());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (invisible)
        {
            timer += Time.deltaTime;
            return;
        }
        else
        {
            if (DamageAnimation)
            {
                return;
            }
            else
            {
                DoAI();
                HandleEnemyStats();
            }
        }
    }


    void Start()
    {
        m_Anim = GetComponent<Animator>();
        m_Renderer = GetComponent<SpriteRenderer>();
        m_Collider = GetComponent<Collider2D>();
    }


    public void ReceiveDamage(int amount)
    {
        enemyHealth -= amount;
        if (enemyHealth > 1)
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

    IEnumerator Flash(float x)
    {
        DamageAnimation = true;
        m_Collider.enabled = false;
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
        m_Collider.enabled = true;
        DamageAnimation = false;
    }

    IEnumerator Die()
    {
        DoDrops();
        Sprite currentSprite = m_Renderer.sprite;
        m_Anim.enabled = false;
        DamageAnimation = true;
        m_Collider.enabled = false;
        m_Renderer.color = Color.red;
        for (int i = 0; i < 10; i++)
        {
            Color tmp = m_Renderer.color;
            tmp.a *= 0.1f;
            m_Renderer.color = tmp;
            yield return new WaitForSeconds(0.1f);
        }
        m_Collider.enabled = true;
        DamageAnimation = false;
        ScoreTimer.score += 5000f;
        GameObject.Destroy(this.gameObject);
    }

    public virtual bool HandleImpact(PlayerBullet bullet)
    {
        ReceiveDamage(1);
        return true;
    }

    public virtual void DoDrops()
    {
        foreach (ItemDrop drop in drops)
        {
            for (int i = 0; i < (drop.amount + 1); i++)
            {
                float check = Random.value;
                if (check >= drop.odds)
                {
                    GameObject instance = Instantiate(drop.prefab);
                    instance.transform.position = new Vector3(transform.position.x, transform.position.y + m_Collider.bounds.extents.y, transform.position.z);
                    Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
                    if (rb)
                    {
                        rb.AddForce(new Vector2(0.5f - (2f * Random.value), Random.value) * Random.Range(5f, 30f));
                    }
                }
            }
        }
    }

    void OnBecameInvisible()
    {
        invisible = true;
        m_Anim.enabled = false;
        m_Renderer.enabled = false;
        m_Collider.enabled = false;
    }

    void OnBecameVisible()
    {
        invisible = false;
        m_Anim.enabled = true;
        m_Renderer.enabled = true;
        m_Collider.enabled = true;
    }

    [System.Serializable]
    public struct ItemDrop
    {
        public GameObject prefab;
        [Range(0, 1)]
        public float odds;
        public int amount;
    }
}
