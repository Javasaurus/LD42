using UnityEngine;

public class PlayerCollide : MonoBehaviour
{

    Jetpack jetpack;
    Health health;

    void Start()
    {
        jetpack = GetComponent<Jetpack>();
        health = GetComponent<Health>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Heart")
        {
            Destroy(collision.gameObject);
            health.HeartPickup();
        }
        if (collision.gameObject.tag == "Jetpack")
        {
            jetpack.jetfuel += collision.gameObject.GetComponent<JetpackRecharge>().fuel;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            health.currDamage = 1;
            health.DamagePlayer();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Door")
        {
            collision.transform.GetComponent<RoomTransition>().DoPlayerTransition(this.transform);
        }
    }

}
