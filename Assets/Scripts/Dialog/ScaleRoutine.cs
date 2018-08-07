using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleRoutine : MonoBehaviour {


    /// <summary>
    /// Scales the object to the specified scale
    /// </summary>
    /// <param name="targetScale">The scale that is the end point</param>
    public void Scale(Vector3 targetScale)
    {
        StartCoroutine(LerpUp(targetScale));
    }


    private float t;

    IEnumerator LerpUp(Vector3 targetScale)
    {
        float progress = 0;
        Vector3 InitialScale = transform.localScale;
        while (progress <= 1)
        {
            transform.localScale = Vector3.Lerp(InitialScale, targetScale, progress);
            progress += Time.deltaTime * 10;
            yield return null;
        }
        transform.localScale = targetScale;

    }
}
