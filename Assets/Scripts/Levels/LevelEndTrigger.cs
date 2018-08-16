using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //move up the camera ! (is this still needed?)
            if (!PreferencesManager.INSTANCE.ZEN_MODE)
            {
                //TODO make this more OOP, for now copy from the transitions
                Transform gameCamera = GameObject.FindGameObjectWithTag("GameCamera").transform;
                Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
                float cameraDistanceToMove = GetComponent<BoxCollider2D>().bounds.size.y;
                gameCamera.transform.position += new Vector3(0, cameraDistanceToMove, 0);
                playerTransform.transform.position += new Vector3(0, RoomTransition.playerDistanceToMove, 0);
                GameObject.FindObjectOfType<LevelChanger>().MoveLevel();
                this.enabled = false;
            }
            else
            {
                LevelGenerator.INSTANCE.ZENROOMS_CLIMBED++;
                LevelGenerator.INSTANCE.ReloadLevel();     
            }
        }
    }
}
