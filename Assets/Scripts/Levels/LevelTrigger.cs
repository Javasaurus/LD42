using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelTrigger : MonoBehaviour {

    public static LevelTrigger currentRoom;                         //The room we are currently in

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            currentRoom = this;
        }
    }
}
