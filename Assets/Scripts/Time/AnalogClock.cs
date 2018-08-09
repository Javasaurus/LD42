using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalogClock : MonoBehaviour
{
    private const float hoursToDegrees = 360f / 12f;
    private const float minutesToDegrees = 360f / 60f;
    private const float secondsToDegrees = 360f / 60f;

    public Transform hours, minutes, seconds;

    void Update()
    {
        TimeSpan gameTime = TimeKeeper.Instance.GameTime;
        hours.localRotation =
            Quaternion.Euler(0f, 0f, (float)gameTime.TotalHours * -hoursToDegrees);
        minutes.localRotation =
            Quaternion.Euler(0f, 0f, (float)gameTime.TotalMinutes * -minutesToDegrees);
        seconds.localRotation =
            Quaternion.Euler(0f, 0f, (float)gameTime.TotalSeconds * -secondsToDegrees);
    }


}
