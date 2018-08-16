using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTimer : MonoBehaviour
{

    private static float score;
    public static float finalScore;
    public static bool STOP;

    float interval = 10f;
    float timer;



    public static void AddScore(int amount)
    {
        if (!STOP)
        {
            score += amount;
        }
    }

    public static float GetScore()
    {
        return score;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!STOP)
        {
            if (Time.time > timer)
            {
                score += Time.deltaTime * 100f;
                timer = Time.time + interval;
            }
        }

    }
}
