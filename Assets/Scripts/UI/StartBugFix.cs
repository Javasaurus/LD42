using UnityEngine;
using System.Collections;

public class StartBugFix : MonoBehaviour
{

    public GameObject button;

    void OnEnable()
    {
        StartCoroutine(OnAwke());
    }

    IEnumerator OnAwke()
    {
        button.SetActive(false);
        yield return null;
        button.SetActive(true);
    }

}
