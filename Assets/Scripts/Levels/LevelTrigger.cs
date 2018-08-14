﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelTrigger : MonoBehaviour
{
    public Transform spawn;
    public static LevelTrigger currentRoom;                         //The room we are currently in
    Transform m_Player;
    BoxCollider2D m_Collider;

    bool wasActivated;


    private void Start()
    {

        m_Player = GameObject.FindGameObjectWithTag("Player").transform;
        m_Collider = GetComponent<BoxCollider2D>();
        DisableChildren();
    }

    void DisableChildren()
    {
        wasActivated = false;
        foreach (Transform child in transform)
        {
            if ((child.name != "WALLS" && child.name != "DOOR"))
            {

                child.gameObject.SetActive(false);
            }
        }
    }


    void EnableChildren()
    {
        wasActivated = true;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        //    Debug.Log("We are in room " + currentRoom.name);
        if (!wasActivated & (isInRoom() | currentRoom == this))
        {
            EnableChildren();
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
