using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    public GameObject deathUI;
    public GameObject heartIcon;

    public Sprite good, bad;

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

        for (int i = 0; i < maxHearts; i++)
        {
            if (i <= hearts - 1)
            {
                heartsParent.GetChild(i).GetComponent<Image>().sprite = good;
            }
            else
            {
                Debug.Log("asd");
                heartsParent.GetChild(i).GetComponent<Image>().sprite = bad;
            }
        }

        EZCameraShake.CameraShaker.Instance.ShakeOnce(2.9f, 2.7f, 0.1f, 0.7f);

    }

    [ContextMenu("Recover")]
    public void HeartPickup()
    {
        hearts++;
        hearts = Mathf.Clamp(hearts, 1, maxHearts);
        for (int i = 0; i < maxHearts; i++)
        {
            if (i <= hearts - 1)
            {
                heartsParent.GetChild(i).GetComponent<Image>().sprite = good;
            }
            else
            {
                Debug.Log("asd");
                heartsParent.GetChild(i).GetComponent<Image>().sprite = bad;
            }
        }
    }

    public void Gameover()
    {
        deathUI.SetActive(true);
        //Time.timeScale = 0;
    }

}
