using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerBullet : MonoBehaviour
{

    public Vector3 direction;
    public float speed;
    public float maxDistanceTravelled = 5f;
    public bool rebounded;

    void Start()
    {
        GameObject.Destroy(this.gameObject, maxDistanceTravelled / speed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnEnable()
    {
        if (!PlayerShooting.currentBullets.Contains(this))
        {
            PlayerShooting.currentBullets.Add(this);
        }
    }

    private void OnDestroy()
    {
        PlayerShooting.currentBullets.Remove(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool destroy = true;
        if (collision.collider.GetComponent<Item>() 
            || collision.collider.GetComponent<PlayerBullet>())
        {
            return;
        }

        if (collision.collider.tag == "CrusherBoss")
        {
            Debug.Log(collision.collider.name);
            CrusherTarget target = collision.collider.GetComponentInParent<CrusherTarget>();
            if (target)
            {
                target.OnDamage();
            }
        }
        else if (collision.collider.tag == "Enemy")
        {
            BasicEnemy enemyController = collision.collider.GetComponent<BasicEnemy>();
            if (enemyController)
            {
                destroy = enemyController.HandleImpact(this);
            }
        }
        else if (collision.collider.tag == "Player" && rebounded)
        {
            Health health = collision.collider.GetComponent<Health>();
            if (health)
            {
                health.DamagePlayer(1);
                destroy = true;
            }
        }

        //handle this if the thing we hit has a particular tag/script/whatever it is 
        if (destroy)
        {
            PlayerShooting.currentBullets.Remove(this);
            GameObject.Destroy(this.gameObject);
        }

    }

}
