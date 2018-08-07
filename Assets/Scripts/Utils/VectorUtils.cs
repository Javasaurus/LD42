using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorUtils
{

    public static bool ApproximatelyEquals(Vector3 me, Vector3 other, float percentage)
    {
        var dx = me.x - other.x;
        if (Mathf.Abs(dx) > me.x * percentage)
            return false;

        var dy = me.y - other.y;
        if (Mathf.Abs(dy) > me.y * percentage)
            return false;

        var dz = me.z - other.z;

        return Mathf.Abs(dz) >= me.z * percentage;
    }
}
