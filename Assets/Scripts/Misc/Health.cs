using UnityEngine;

public class Health : MonoBehaviour
{

    public GameObject deathUI;
    public GameObject heartIcon;

    public Transform heartsParent;

    public int currDamage;

    public int startingHearts;
    public int maxHearts;

    public int score;

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

        EZCameraShake.CameraShaker.Instance.ShakeOnce(2.9f, 2.7f, 0.1f, 0.7f);

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
        //Time.timeScale = 0;
    }

}
