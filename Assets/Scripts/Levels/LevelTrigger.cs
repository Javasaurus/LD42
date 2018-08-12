using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelTrigger : MonoBehaviour
{

    public static LevelTrigger currentRoom;                         //The room we are currently in
    Transform m_Player;
    BoxCollider2D m_Collider;

    private void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").transform;
        m_Collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
    //    Debug.Log("We are in room " + currentRoom.name);
        if (isInRoom())
        {
            currentRoom = this;
        }
    }

    //on trigger enter broke with new controller?
    /*  private void OnTriggerEnter2D(Collider2D collision)
      {
          if (collision.tag == "Player")
          {
              currentRoom = this;
          }
      }*/

    private bool isInRoom()
    {
        return m_Collider.bounds.Contains(m_Player.position);
    }

}
