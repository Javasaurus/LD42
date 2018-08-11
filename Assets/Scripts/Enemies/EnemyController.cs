using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public Enemy en;

    public int enemyHealth;

    public GameObject heartObj;

    void Update()
    {
        if (enemyHealth <= 0)
        {
            Death();
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
