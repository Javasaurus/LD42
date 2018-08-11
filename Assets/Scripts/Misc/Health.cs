using UnityEngine;

public class Health : MonoBehaviour
{

    public GameObject heartIcon;

    public Transform heartsParent;

    public int currDamage;

    public int startingHearts;
    public int maxHearts;

    int hearts;

    void Awake()
    {
        hearts = startingHearts;
    }

    [ContextMenu("Damage")]
    public void DamagePlayer()
    {

        hearts -= currDamage;

        if (hearts <= 0)
        {
            Gameover();
            return;
        }

        for (int i = 0; i < currDamage; i++)
        {
            Destroy(heartsParent.GetChild(i).gameObject);
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Heart")
        {
            Destroy(collision.gameObject);
            HeartPickup();
        }
        if (collision.gameObject.tag == "Projectile")
        {
            currDamage = collision.gameObject.GetComponent<Projectile>().damage;
            Destroy(collision.gameObject);
            DamagePlayer();
        }
        if (collision.gameObject.tag == "Enemy")
        {
            currDamage = 1;
            DamagePlayer();
        }
    }

    [ContextMenu("Recover")]
    public void HeartPickup()
    {
        hearts++;
        hearts = Mathf.Clamp(hearts, 1, maxHearts);
        if (heartsParent.childCount < 8)
        {
            Instantiate(heartIcon, heartsParent);
        }
    }

    public void Gameover()
    {
        Debug.Log("You ded lol");
    }

}
