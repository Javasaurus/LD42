using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Item : MonoBehaviour
{

    private void OnEnable()
    {
        GameObject.Destroy(this.gameObject, 5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            if (HandleItemEffect(collision.collider.gameObject))
            {
                ScoreTimer.AddScore(500);
                Chime();
                GameObject.Destroy(this.gameObject);
            }
            else
            {

            }
        }
    }

    public abstract bool HandleItemEffect(GameObject collider);

    public void Chime()
    {
        Debug.Log("Plink");
    }

}
