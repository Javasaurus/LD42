using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDoorAnimation : DoorAnimation
{
    public GameObject sealObject;

    private void Awake()
    {
        sealObject.SetActive(false);
    }

    public override void SealRoom()
    {
        StartCoroutine(WaitForFloorClose());
    }

    IEnumerator WaitForAnimation()
    {
        yield return new WaitForSecondsRealtime(2f);
        Resume();
    }

    IEnumerator WaitForFloorClose()
    {
        yield return new WaitForSecondsRealtime(2f);
        sealObject.SetActive(true);
        StartCoroutine(WaitForAnimation());
    }

}
