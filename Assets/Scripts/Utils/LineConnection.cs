using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineConnection : MonoBehaviour
{

    public CurveType type;                         //The type of the curves
    public GameObject start;                      //The target this curve is directed to
    public GameObject end;                          //The target this curve is directed to
    public float curveWidth = 4f;                  //The width of the curve
    private Vector3[] points = new Vector3[4];     //The control points for the curve (anchor points)
    public int segments = 50;                      //The amount of points in a curve
    public Vector3[] curvePoints;                  //The current bezier curve points

    public Color startColor = Color.red;
    public Color endColor = Color.magenta;
    LineRenderer _LineRenderer;

    public enum CurveType
    {
        LOW_S, HIGH_S, STANDARD
    }

    // Use this for initialization
    void Start()
    {
        _LineRenderer = gameObject.GetComponent<LineRenderer>();
        transform.localPosition = Vector3.zero;
    }

    void LateUpdate()
    {
        if (start != null && end != null)
        {
            GetControlPoints();
            curvePoints = (BezierCurve.CalculateCurve(points, segments));
            DrawLine(curvePoints);
        }
    }

    public void DrawLine(Vector3[] points)
    {
        _LineRenderer.startWidth = 0.25f;
        _LineRenderer.endWidth = 0.25f;
        _LineRenderer.positionCount = (segments);
        _LineRenderer.startColor = startColor;
        _LineRenderer.endColor = endColor;
        _LineRenderer.SetPositions(points);

    }

    void GetControlPoints()
    {
        GetControlPoints(CurveType.STANDARD);
    }

    void GetControlPoints(CurveType type)
    {

        // points are in the 1/4 and 3/4 of the way between start and target...
        // if the Y is larger, it should bend outwards
        float deltaY = start.transform.position.y - end.transform.position.y;
        float deltaX = start.transform.position.x - end.transform.position.x;

        float widthStep = Mathf.Abs(deltaX) / 4;
        float heightStep = deltaY * curveWidth;

        points[0] = start.transform.position;
        switch (type)
        {
            case CurveType.LOW_S:
                points[1] = new Vector3(end.transform.position.x - widthStep, end.transform.position.y - heightStep, end.transform.position.z);
                points[2] = new Vector3(start.transform.position.x + widthStep, start.transform.position.y + heightStep, start.transform.position.z);
                break;

            case CurveType.HIGH_S:
                points[1] = new Vector3(end.transform.position.x - widthStep, end.transform.position.y + heightStep, end.transform.position.z);
                points[2] = new Vector3(start.transform.position.x + widthStep, start.transform.position.y - heightStep, start.transform.position.z);
                break;

            default:
                points[1] = new Vector3(end.transform.position.x - widthStep, end.transform.position.y + heightStep, end.transform.position.z);
                points[2] = new Vector3(start.transform.position.x + widthStep, start.transform.position.y + heightStep, start.transform.position.z);
                break;
        }

        points[3] = end.transform.position;


    }


}
