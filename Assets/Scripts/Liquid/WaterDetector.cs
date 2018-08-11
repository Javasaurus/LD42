using UnityEngine;
using System.Collections;

public class WaterDetector : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("You died !");
        }
    }



}
