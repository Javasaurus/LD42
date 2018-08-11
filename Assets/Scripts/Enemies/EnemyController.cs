using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public Enemy en;

    public Transform player;

    public GameObject projectile;
    public GameObject heartObj;

    public int enemyHealth;

    float timeSinceLastFire;
    int direction;

    void Start()
    {
        enemyHealth = en.health;
     }

    void Update()
    {

        if (enemyHealth <= 0)
        {
            Death();
            return;
        }

        if (player.position.x > transform.position.x)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

        if ((en.combatType == CombatType.ThreeDirectionRanged || en.combatType == CombatType.OneDirectionRanged) && timeSinceLastFire + en.fireRate < Time.time)
        {
            timeSinceLastFire = Time.time;
            RangedFire();
        }

    }

    void RangedFire()
    {
        if (en.combatType == CombatType.OneDirectionRanged)
        {
            GameObject instance = Instantiate(projectile, transform.position, Quaternion.identity);
            instance.GetComponent<Projectile>().velocity = new Vector2(direction, 0);
            instance.GetComponent<Projectile>().damage = en.dmg;
        }
        else
        {

            GameObject instance = Instantiate(projectile, transform.position, Quaternion.identity);
            instance.GetComponent<Projectile>().velocity = new Vector2(1, 0);
            instance.GetComponent<Projectile>().damage = en.dmg;

            instance = Instantiate(projectile, transform.position, Quaternion.identity);
            instance.GetComponent<Projectile>().velocity = new Vector2(-1, 0);
            instance.GetComponent<Projectile>().damage = en.dmg;

            instance = Instantiate(projectile, transform.position, Quaternion.identity);
            instance.GetComponent<Projectile>().velocity = new Vector2(0, 1);
            instance.GetComponent<Projectile>().damage = en.dmg;

        }
    }

    public void Death()
    {
        if (Random.value > en.dropChance)
        {
            Instantiate(heartObj, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

}
