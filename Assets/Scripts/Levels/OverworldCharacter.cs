using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldCharacter : MonoBehaviour
{
    public float MoveSpeed = 8;                     //the movement speed
    public LevelLoad TargetLevel;                   //the level we are moving to
    public LevelLoad currentLevel;                 //the current trigger 
    private LineConnection activeLine;              //the line we will be using to move along
    private Vector3[] pointsToFollow;               //List of points to follow currently (made from bezier class, not really needed can be done with transforms later)
    private bool isMoving;                          //Boolean to indicate if the object is moving

    Coroutine MoveIE;


    private void OnEnable()
    {
        InputManager.INSTANCE.onJump += LoadLevel;
    }

    private void OnDisable()
    {
        InputManager.INSTANCE.onJump -= LoadLevel;
    }

    /// <summary>
    /// Moves to a defined position along a track
    /// </summary>

    void MoveToPoint()
    {
        if (activeLine != null)
        {
            DetermineDirection();
            StartCoroutine(MoveObject());
        }
        else
        {
            Debug.Log("No direct path heading there...");
        }
    }

    /// <summary>
    /// Loads the currently selected level
    /// </summary>
    /// 
    void LoadLevel(bool delegateBool)
    {
        if (!delegateBool & currentLevel != null)
        {
            SceneManager.LoadScene(currentLevel.LevelSceneIndex);
        }
    }

    /// <summary>
    /// Determines the direction we are moving along the path
    /// </summary>
    void DetermineDirection()
    {
        pointsToFollow = new Vector3[activeLine.curvePoints.Length];
        activeLine.curvePoints.CopyTo(pointsToFollow, 0);
        bool forward = (currentLevel == activeLine.start.GetComponent<LevelLoad>());
        if (!forward)
        {
            Debug.Log("Reversing the path");
            //reverse the points
            Array.Reverse(pointsToFollow);
        }
    }

    /// <summary>
    /// Gets called on level select (for now with mouse but can be done for example by pressing right / left)
    /// </summary>
    /// <param name="selectedLevel"></param>
    public void OnLevelSelected(LevelLoad selectedLevel)
    {

        if (!isMoving && selectedLevel != currentLevel)
        {
            foreach (LineConnection line in GameObject.FindObjectsOfType<LineConnection>())
            {
                //prevent moving from locked to another locked ...
                if (line.start.GetComponent<LevelLoad>().Locked && line.end.GetComponent<LevelLoad>().Locked)
                {
                    //then should we do something such as play an audio effect here?
                }
                else if (
                    (line.start == selectedLevel.gameObject & line.end == currentLevel.gameObject)
                 || (line.start == currentLevel.gameObject & line.end == selectedLevel.gameObject))
                {
                    activeLine = line;
                    break;
                }

            }

            if (activeLine != null)
            {
                MoveToPoint();
                TargetLevel = selectedLevel;
            }
            else
            {
                Debug.Log("There was no path here");
            }
        }
    }

    IEnumerator MoveObject()
    {
        isMoving = true;
        for (int i = 0; i < pointsToFollow.Length; i++)
        {
            MoveIE = StartCoroutine(Moving(i));
            yield return MoveIE;
        }
        currentLevel = TargetLevel;
        activeLine = null;
        isMoving = false;
    }

    IEnumerator Moving(int currentPosition)
    {
        while (transform.position != pointsToFollow[currentPosition])
        {
            transform.position = Vector3.MoveTowards(transform.position, pointsToFollow[currentPosition], MoveSpeed * Time.deltaTime);
            yield return null;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        LevelLoad loadTrigger = other.GetComponent<LevelLoad>();
        if (loadTrigger)
        {
            currentLevel = loadTrigger;
        }
    }
}
