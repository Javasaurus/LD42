using UnityEngine;
using System.Collections;

public class WaterDetector : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("You died !");
            //do something in terms of animating (a screen showing a drowning bot?)
            GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().Gameover();
        }
    }



}
