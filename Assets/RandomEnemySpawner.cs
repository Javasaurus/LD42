using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemySpawner : MonoBehaviour
{

    public GameObject[] enemyPrefabs;

    // Use this for initialization
    void Start()
    {
        Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], transform.position, transform.rotation, transform.parent).transform.localScale = transform.localScale;
        GameObject.Destroy(this.gameObject);
    }

}
