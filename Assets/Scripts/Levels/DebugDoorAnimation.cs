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
        yield return new WaitForSecondsRealtime(0.5f);
       
    }

    IEnumerator WaitForFloorClose()
    {
        yield return new WaitForSecondsRealtime(1f);
        if (sealObject)
        {
            sealObject.SetActive(true);
        }
        Resume();
        StartCoroutine(WaitForAnimation());
    }

}
