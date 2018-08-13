using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingScript : MonoBehaviour {

    public float rotationSpeed = 3f;
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
