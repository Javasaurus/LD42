using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMimic : MonoBehaviour {

    Transform m_Player;
   public Vector3 offset = new Vector3(0,0.25f,0);

	// Use this for initialization
	void Start () {
        m_Player = GameObject.FindObjectOfType<PlayerAnimator>().transform;

	}
	
	// Update is called once per frame
	void Update () {
        transform.position = m_Player.position+ offset;
	}
}
