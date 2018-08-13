using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerBullet : MonoBehaviour
{

    public Vector3 direction;
    public float speed;
    public float maxDistanceTravelled = 5f;


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
    if (collision.collider.tag == "CrusherBoss")
        {
            Debug.Log(collision.collider.name);
            CrusherTarget target = collision.collider.GetComponentInParent<CrusherTarget>();
            if (target)
            {
                target.OnDamage();
            }
        }

        //handle this if the thing we hit has a particular tag/script/whatever it is 
        PlayerShooting.currentBullets.Remove(this);
        GameObject.Destroy(this.gameObject);

        //TODO decide where to put this code, for now it's fine here...

    }

}
