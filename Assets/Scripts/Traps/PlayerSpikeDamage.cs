using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerSpikeDamage : MonoBehaviour
{
    public float timeBetweenhits = 1.5f;
    private float timer;


    public int damage = 1;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Time.time > timer && collision.tag == "Player")
        {
            Health health = collision.GetComponent<Health>();
            if (health)
            {
                health.currDamage += damage;
                health.DamagePlayer(1);
                timer = Time.time + timeBetweenhits;
                PlatformerMotor2D tmp = health.GetComponent<PlatformerMotor2D>();
                if (tmp)
                {
                    tmp.ForceJump();
                }
            }
        }
    }
}
