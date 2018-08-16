using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizationSpawn : MonoBehaviour
{
    [Range(0f, 1f)]
    public float odds;

    private void OnEnable()
    {
        if (Random.value <= (1- odds))
        {
            this.gameObject.SetActive(false);
        }
    }

}
