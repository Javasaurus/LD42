using UnityEngine;

public class Projectile : MonoBehaviour
{

    public Vector2 velocity;
    public float bulletSpeed = 2.5f;
    public int damage;

    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x - (velocity.x * Time.fixedDeltaTime * bulletSpeed), transform.position.y - (velocity.y * Time.fixedDeltaTime * bulletSpeed), 0);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Health h = col.GetComponent<Health>();
            h.DamagePlayer(1);
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "World")
        {
            Destroy(gameObject);
        }
    }

}
