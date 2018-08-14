using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingEnemy : BasicEnemy
{

    public Sprite fastSwingSprite;
    public Sprite waitingSprite;



    Coroutine stopAttack;

    public override void DoAI()
    {
        if (!isAttacking && Time.time > timer)
        {
            isAttacking = true;
            if (normalCount <= 3)
            {
                m_Anim.SetTrigger("Attack");
            }
            else if (normalCount == 3)
            {
                normalCount = 0;
                m_Anim.SetTrigger("SpecialAttack");
            }
            else
            {
                m_Anim.enabled = false;
                m_Renderer.sprite = waitingSprite;
            }
            if (stopAttack != null)
            {
                stopAttack = StartCoroutine(StopAttacking());
            }
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

    public override bool HandleImpact(PlayerBullet bullet)
    {
        if (isAttacking)
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

}
