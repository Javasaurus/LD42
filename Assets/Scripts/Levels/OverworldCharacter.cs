using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldCharacter : MonoBehaviour
{
    public int currentIndex = 0;                   //the current node to move to
    public float MoveSpeed = 8;                    //the movement speed
    public LineConnection[] lines;                 //the path across the world map to iterate
    public Vector3[] pointsToFollow;               //List of points to follow currently (made from bezier class, not really needed can be done with transforms later)
    public bool isMoving;                          //Boolean to indicate if the object is moving

    Coroutine MoveIE;

    public delegate void OnMoveFinished();
    public OnMoveFinished onMoveFinished;

    private void Awake()
    {
        pointsToFollow = lines[currentIndex].curvePoints;
        onMoveFinished += FinalizeMove;
    }

    private void OnEnable()
    {
        StartCoroutine(WaitInit());
    }

    void MoveToNextLevel()
    {
     //   Debug.Log("Moving on " + lines[currentIndex].name);
        MoveToPoint();
    }

    public void FinalizeMove()
    {
        currentIndex++;
        isMoving = false;
    }

    //tmp fix...ugly but eh
    private void LateUpdate()
    {
        if (transform.position == lines[currentIndex].end.transform.position)
        {
            //then wait
            StartCoroutine(Wait());
        }
    }

    IEnumerator WaitInit()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Start");
        pointsToFollow = lines[currentIndex].curvePoints;
        MoveToNextLevel();
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("Finished");
        onMoveFinished();
    }

    /// <summary>
    /// Moves to a defined position along a track
    /// </summary>
    void MoveToPoint()
    {
        StartCoroutine(MoveObject());
    }

    IEnumerator MoveObject()
    {
        isMoving = true;
        for (int i = 0; i < pointsToFollow.Length; i++)
        {
            MoveIE = StartCoroutine(Moving(i));
            yield return MoveIE;
        }
    }

    IEnumerator Moving(int currentPosition)
    {
        while (transform.position != pointsToFollow[currentPosition])
        {
            transform.position = Vector3.MoveTowards(transform.position, pointsToFollow[currentPosition], MoveSpeed * Time.deltaTime);
            yield return null;
        }


    }

}
