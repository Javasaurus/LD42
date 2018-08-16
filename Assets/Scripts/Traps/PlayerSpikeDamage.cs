using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerSpikeDamage : MonoBehaviour
{
    public int damage = 1;
    public int attackCount = 3;
    public bool canSubmerge = false;

    const float damageInterval = 1f;
    float timer;

    void OnTriggerEnter2D(Collider2D collision)
    {
        HandleTrigger(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (canSubmerge)
        {
            HandleTrigger(collision);
        }
    }

    void HandleTrigger(Collider2D collision)
    {
        if (Time.time > timer)
        {
            if (collision.tag == "Player")
            {
                Health health = collision.GetComponent<Health>();
                if (health)
                {
                    health.DirectlyDamagePlayer(damage);
                    PlatformerMotor2D tmp = health.GetComponent<PlatformerMotor2D>();
                    if (tmp)//&& tmp.playerVelocity.y <= 0)
                    {
                        tmp.ForceJump();
                    }
                }
            }
            timer = Time.time + damageInterval;
        }
    }

}
