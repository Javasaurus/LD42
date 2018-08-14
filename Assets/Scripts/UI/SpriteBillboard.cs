using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{

    public Camera referenceCamera;

    void Update()
    {
        transform.LookAt(referenceCamera.transform.position, -Vector3.up);
    }

}
