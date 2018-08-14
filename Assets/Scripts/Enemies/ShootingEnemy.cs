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

    public virtual void LaunchBullet()
    {
        if (currentProjectile == null)
        {
            currentProjectile = Instantiate(projectile, launchingSpot.position, Quaternion.identity).GetComponent<SeekingMissile>();
            missileLifeTimer = Time.time + currentProjectile.lifetime;
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
