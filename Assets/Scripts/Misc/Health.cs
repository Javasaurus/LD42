using UnityEngine;

public class Health : MonoBehaviour
{

    public GameObject deathUI;
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
        deathUI.SetActive(true);
        Time.timeScale = 0;
    }

}
