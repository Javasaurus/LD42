using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    public static float playerDistanceToMove = 3f;

    public DoorAnimation doorAnimation;

    public Vector3 transitionDirection = new Vector3(0, 1f, 0);                 // The movement direction to transition to
    public bool inTransition;                                                   // Boolean indicating we are transitioning screens
    public float transitionSpeed = 0.5f;                                        // The transition speed
    public Transform playerTransform;                                           // The player 
    public Transform gameCamera;                                                // The game camera
    public Transform waterRising;                                               // The rising liquid

    private float cameraDistanceToMove;                                         // The total distance the camera will move
                                                                                // arbitrary value for now  
    float deltaDistance;                                                        // The distance difference with the last frame
    private float totalDistance = 0;                                            // The total travelled distance (might lead to SOME issues, debug when arise --> solution = to clamp to the totaldistance)

    float liquidRubberBandingOffset = 0;                                        // the distance the liquid starts from at the current rooms


    private void OnEnable()
    {
        gameCamera = GameObject.FindGameObjectWithTag("GameCamera").transform;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        waterRising = GameObject.FindObjectOfType<WaterRising>().transform;
        liquidRubberBandingOffset = Mathf.Abs(waterRising.position.y - gameCamera.position.y);
    }

    private void Update()
    {
        if (inTransition)
        {

            if (totalDistance < cameraDistanceToMove)
            {
                deltaDistance += (Time.unscaledDeltaTime * transitionSpeed);
                gameCamera.transform.position += transitionDirection * deltaDistance;
                if (totalDistance < playerDistanceToMove)
                {
                    playerTransform.position += transitionDirection * deltaDistance;
                }
                totalDistance += deltaDistance;
            }
            else
            {
                waterRising.transform.position = new Vector3(waterRising.transform.position.x, gameCamera.transform.position.y - liquidRubberBandingOffset, waterRising.transform.position.z);
                inTransition = false;
                if (doorAnimation)
                {
                    doorAnimation.SealRoom();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !inTransition)
        {
            //check if we initialized a room
            if (LevelTrigger.currentRoom)
            {
                //we should remove all stored speeds for the rigidbody of the player
                cameraDistanceToMove = LevelTrigger.currentRoom.GetComponent<BoxCollider2D>().bounds.size.y;
                inTransition = true;
                playerTransform = collision.transform;
                Time.timeScale = 0;
                Time.fixedDeltaTime = float.MaxValue;
            }
        }
    }



}
