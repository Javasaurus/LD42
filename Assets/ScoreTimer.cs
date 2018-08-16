using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTimer : MonoBehaviour
{

    public static float score;

    // Update is called once per frame
    void Update()
    {
        if (!Health.DEAD)
        {
            score += Time.deltaTime * 100f;
        }
    }
}
