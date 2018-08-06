using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierFollow : MonoBehaviour
{

    public enum CurveType
    {
        LOW_S, HIGH_S, STANDARD
    }

    public CurveType type;                         //The type of the curves
    public GameObject target;                      //The target this curve is directed to
    public float curveWidth = 0.75f;               //The width of the curve
    public float MoveSpeed = 8;

    Vector3[] controlPoints;
    List<Vector3> positions;

    int currentIndex = 0;
    bool following = false;

    public bool startFollow;
    Vector3 previousPosition;
    Coroutine MoveIE;
   
    public void StartFollowing()
    {
        following = true;
        controlPoints = GetControlPoints(type);
        positions = BezierCurve.CalculateCurve(controlPoints, 50);
        currentIndex = positions.Count - 1;
        StartCoroutine(moveObject());
        startFollow = false;
    }

    private void FixedUpdate()
    {
        if (startFollow && !following)
        {
            StartFollowing();
        }
        if (following && previousPosition != target.transform.position)
        {
            controlPoints = GetControlPoints(type);
            positions = BezierCurve.CalculateCurve(controlPoints, 50);
            previousPosition = target.transform.position;
            currentIndex = positions.Count-2;
        }

    }

    Vector3[] GetControlPoints(CurveType type)
    {
        Vector3[] points = new Vector3[4];
        // points are in the 1/4 and 3/4 of the way between start and target...
        // if the Y is larger, it should bend outwards
        float deltaY = target.transform.position.y - transform.position.y;
        float deltaX = target.transform.position.x - transform.position.x;

        float widthStep = Mathf.Abs(deltaX) / 4;
        float heightStep = deltaY * curveWidth;

        points[3] = transform.position;
        switch (type)
        {
            case CurveType.LOW_S:
                points[2] = new Vector3(transform.position.x + widthStep, transform.position.y - heightStep, transform.position.z);
                points[1] = new Vector3(target.transform.position.x - widthStep, transform.position.y + heightStep, transform.position.z);
                break;

            case CurveType.HIGH_S:
                points[2] = new Vector3(transform.position.x + widthStep, transform.position.y + heightStep, transform.position.z);
                points[1] = new Vector3(target.transform.position.x - widthStep, transform.position.y - heightStep, transform.position.z);
                break;

            default:
                points[2] = new Vector3(transform.position.x + widthStep, transform.position.y + heightStep, transform.position.z);
                points[1] = new Vector3(target.transform.position.x - widthStep, transform.position.y + heightStep, transform.position.z);
                break;
        }

        points[0] = target.transform.position;

        return points;
    }

    IEnumerator moveObject()
    {
        while (currentIndex>=0)
        {
            MoveIE = StartCoroutine(Moving());
            yield return MoveIE;   
        }
        //here we are DONE ! :D
        following = false;
    }

    IEnumerator Moving()
    {
        while (transform.position != positions[currentIndex])
        {
            transform.position = Vector3.MoveTowards(transform.position, positions[currentIndex], MoveSpeed * Time.deltaTime);
            yield return null;
        }
        currentIndex--;
    }

    private void OnDrawGizmos()
    {
        if (positions == null || positions.Count == 0)
        {
            return;
        }
        for (int i = 0; i < positions.Count; i++)
        {
            Gizmos.color = Color.yellow;
            if (i == currentIndex)
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawSphere(positions[i], 0.1f);
        }
    }

}
