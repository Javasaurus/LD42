using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CharacterInput : MonoBehaviour {


    public float Horizontal;
    public float Vertical;
    public bool Jump;
    public bool AnticipatingJump;

	// Update is called once per frame
	void Update () {
        Horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        Vertical = CrossPlatformInputManager.GetAxis("Vertical");

        if (!Jump)
        {
            Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            if (Jump)
            {
                AnticipatingJump = false;
            }
        }

        if (!AnticipatingJump)
        {
            AnticipatingJump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

    }

}
