using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingEnemy : BasicEnemy
{

    public Sprite fastSwingSprite;
    public Sprite waitingSprite;

    bool isReflecting;
    float reflectTimer = 0f;
    float reflectDelay = 5f;
    float waitingTimer = 0f;
    float waitingDelay = 3f;

    float attackTimer = 0f;
    float attackDelay = 2f;

    Coroutine stopAttack;

    public void FixedUpdate()
    {
        if (Time.time > reflectTimer)
        {
            waitingTimer = Time.time + waitingTimer;
            isReflecting = false;
        }

        if (Time.time > waitingTimer)
        {
            reflectTimer = Time.time + reflectDelay;
            isReflecting = true;
        }

        if (Time.time > attackTimer)
        {
            isAttacking = true;
            StartCoroutine(DisableAttacking());
        }

        m_Anim.SetBool("Attack", isAttacking);
    }

    public IEnumerator DisableAttacking()
    {
        yield return new WaitForSecondsRealtime(attackDelay);
        attackTimer = Time.time + attackDelay;
        isAttacking = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && isAttacking)
        {
            collision.collider.GetComponent<Health>().DamagePlayer(2, 3);
        }
    }

    public override bool HandleImpact(PlayerBullet bullet)
    {
        if (isReflecting)
        {
            m_Anim.enabled = false;
            Debug.Log("Rebound");
            m_Renderer.sprite = fastSwingSprite;
            bullet.direction = new Vector3(bullet.direction.x * -1F, bullet.direction.y, bullet.direction.z);
            bullet.rebounded = true;
            bullet.gameObject.layer = this.gameObject.layer;
            StartCoroutine(ResetAnimator());
            return false;
        }
        else
        {
            return base.HandleImpact(bullet);
        }
    }
    IEnumerator StopAttacking()
    {
        yield return new WaitForSeconds(5f);
        timer = Time.time + 3f;
        isAttacking = false;
        stopAttack = null;
    }

    IEnumerator ResetAnimator()
    {
        yield return new WaitForSeconds(0.1f);
        m_Anim.enabled = true;
    }


}
