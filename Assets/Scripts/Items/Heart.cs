using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Item
{


    public override bool HandleItemEffect(GameObject collider)
    {
        Health health = collider.GetComponent<Health>();
        if (health && health.hearts < health.maxHearts)
        {
            health.hearts++;
            return true;
        }
        else
        {
            GetComponent<Collider2D>().enabled = false;

        }
        return false;
    }



}
