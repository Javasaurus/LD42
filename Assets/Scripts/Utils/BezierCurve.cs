using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve
{

    public static List<Vector3> CalculateCurve(Vector3[] controlPoints, int segments)
    {
        List<Vector3> curvePoints = new List<Vector3>();
        curvePoints.Add(controlPoints[0]);
        for (int i = 1; i <= segments; i++)
        {
            float t = i / (float)segments;
            curvePoints.Add(CalculateCubicBezierPoint(t, controlPoints[0], controlPoints[1], controlPoints[2], controlPoints[3]));
        }
        curvePoints.Add(controlPoints[3]);
        return curvePoints;
    }

    public static Vector3 CalculateCurvePoint(Vector3[] controlPoints, float timeOnCurve)
    {
        return (CalculateCubicBezierPoint(timeOnCurve, controlPoints[0], controlPoints[1], controlPoints[2], controlPoints[3]));
    }

    static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }
}
