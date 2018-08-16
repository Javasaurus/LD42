using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRising : MonoBehaviour {

    public float risingSpeed;       //The speed of the rising liquid
    public float speedIncrease;     //The acceleration (depending on the level we are at I presume)


    public void Update()
    {
        risingSpeed += speedIncrease * Time.deltaTime;
        transform.position += new Vector3(0, risingSpeed * Time.deltaTime, 0);
    }


}
