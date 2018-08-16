using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Item : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            if (HandleItemEffect(collision.collider.gameObject))
            {
                ScoreTimer.score += 500f;
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
