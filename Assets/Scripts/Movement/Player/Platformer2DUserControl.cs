using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Jetpack))]
[RequireComponent(typeof(PlatformerCharacter2D))]
[RequireComponent(typeof(CharacterInput))]
public class Platformer2DUserControl : MonoBehaviour
{
    private PlatformerCharacter2D m_Character;
    private CharacterInput m_Input;
    private Jetpack m_Jetpack;

    private float timeOutTimer;
    private float timeOutDuration = 0.25f;

    private void Awake()
    {
        m_Character = GetComponent<PlatformerCharacter2D>();
        m_Input = GetComponent<CharacterInput>();
        m_Jetpack = GetComponent<Jetpack>();
    }

    private void Update()
    {
        if (Time.time < timeOutTimer)
        {
            m_Input.Horizontal = 0;
            m_Input.Vertical = 0;
            m_Input.Jump = false;
            m_Input.AnticipatingJump = false;
        }
    }

    private void FixedUpdate()
    {
        if (Time.time < timeOutTimer)
        {
            return;
        }
        // Pass all parameters to the character control script.
        if (m_Jetpack.JetPackActivated)
        {

            if ((m_Character.Grounded | m_Character.Walled) && Mathf.Abs(m_Input.Vertical) < 0.05f)
            {
                m_Jetpack.JetPackActivated = false;
            }
            else
            {
                m_Character.Walled = false;
                m_Character.Grounded = false;
                m_Character.Aircontrol = true;
                m_Character.Move(m_Input.Horizontal, m_Jetpack.xSpeed, m_Input.Jump);
            }
        }
        else if (m_Character.WallJumping)
        {
            m_Input.Horizontal = 0;
            m_Character.WallJumping = !m_Character.Walled && !m_Character.Grounded;
        }
        else
        {
            m_Character.Aircontrol = true;
            //if the jetpack is not activated we could be on the floor OR on a wall...
            if (m_Character.Walled && m_Input.Jump)
            {
                //check if the player is pushing the wall
                if ((m_Character.FacingRight && m_Input.Horizontal > 0) | (!m_Character.FacingRight && m_Input.Horizontal < 0))
                {
                    m_Character.WallJumping = true;
                    //disable the input for a fraction of a second after this?
                    m_Character.Move(-m_Input.Horizontal, m_Input.Jump);
                    timeOutTimer = Time.time + timeOutDuration;
                }
            }
            else if (m_Character.Grounded)
            {
                m_Character.Move(m_Input.Horizontal, m_Input.Jump);
            }

        }
        m_Input.Jump = false;
        m_Input.AnticipatingJump = false;
    }
}

