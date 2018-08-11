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
    }

}
