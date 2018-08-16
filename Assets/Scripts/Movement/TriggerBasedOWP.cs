using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBasedOWP : MonoBehaviour {

    public Collider2D effectorCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            effectorCollider.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            effectorCollider.enabled = true;
        }
    }

}
