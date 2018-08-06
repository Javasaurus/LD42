using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpFollow : MonoBehaviour
{

    public float MovementSpeed;
    public GameObject target;
    public bool Return;

    private void FixedUpdate()
    {
        if (Return && target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * MovementSpeed);
        }
    }

}
