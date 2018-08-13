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
            direction = -1;
            Vector3 theScale = transform.localScale;
            theScale.x = 3;
            transform.localScale = theScale;

        }
        else
        {
            direction = 1;
            Vector3 theScale = transform.localScale;
            theScale.x = -3;
            transform.localScale = theScale;
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
            if(player.position.x > transform.position.x)
            {
                Vector3 theScale = instance.transform.localScale;
                theScale.x = 2;
                instance.transform.localScale = theScale;

            }
            else
            {
                Vector3 theScale = instance.transform.localScale;
                theScale.x = -2;
                instance.transform.localScale = theScale;
            }
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
            instance.GetComponent<Projectile>().velocity = new Vector2(0, -1);
            instance.GetComponent<Projectile>().damage = en.dmg;

        }
    }

    [ContextMenu("Death")]
    public void Death()
    {
        if (Random.value > en.dropChance)
        {
            Instantiate(heartObj, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

}
