using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ShootingEnemy : BasicEnemy
{

    public GameObject projectile;
    SeekingMissile currentProjectile;
    public Transform launchingSpot;
    public Vector3 initialShootingDirection;

    private AudioSource m_Audiosource;
    private float missileLifeTimer;


    Coroutine stopAttack;

    public override void DoAI()
    {
        if (currentProjectile == null)
        {
            if (Time.time > timer)
            {
                m_Anim.SetBool("Attack", true);
                timer = Time.time + Random.Range(5f, 10f);
            }
        }
        else if (Time.time > missileLifeTimer)
        {
            currentProjectile.Explode();
            launchTimer = Time.time + launchTimer;
        }

    }


    public void PlayAudio()
    {
        if (!m_Audiosource)
        {
            m_Audiosource = GetComponent<AudioSource>();
        }
        if (!m_Audiosource.isPlaying)
        {
            m_Audiosource.PlayOneShot(m_Audiosource.clip);
        }
    }

    public override bool HandleImpact(PlayerBullet bullet)
    {
        Debug.Log("Hit");
        ReceiveDamage(1);
        return true;
    }


    private float launchDelay = 7.5f;
    private float launchTimer;
    public virtual void LaunchBullet()
    {
        if (currentProjectile == null)
        {
            currentProjectile = Instantiate(projectile, launchingSpot.position, Quaternion.identity).GetComponent<SeekingMissile>();
            missileLifeTimer = Time.time + currentProjectile.lifetime;
            m_Anim.SetBool("Attack", false);
        }
    }

    IEnumerator StopAttacking()
    {
        yield return new WaitForSeconds(5f);
        timer = Time.time + 5f;

        isAttacking = false;
        stopAttack = null;
    }

    IEnumerator ResetAnimator()
    {
        yield return new WaitForSeconds(0.1f);
        m_Anim.enabled = true;
    }

}
