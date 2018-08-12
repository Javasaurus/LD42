using System;
using UnityEngine;


public class PlatformerCharacter2D : MonoBehaviour
{
    [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
    public bool Aircontrol = false;                 // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
    [SerializeField] private LayerMask m_WhatIsWall;                  // A mask determining what is a wall to the character
    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .5f; // Radius of the overlap circle to determine if grounded
    public bool Grounded;                 // Whether or not the player is grounded.
    public bool Walled;                 // Wether or not the player is walled
    private Transform m_CeilingCheck;   // A position marking where to check for ceilings
    private Transform m_WallCheck;      // A position marking where the wall is
    private Rigidbody2D m_Rigidbody2D;
    public bool WallJumping;           // for determining if the player is walljumping
    public bool FacingRight = true;  // For determining which way the player is currently facing.

    private Animator m_Anim;        //the animator for this character
    private CharacterInput m_Input;

    private void Awake()
    {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");
        m_WallCheck = transform.Find("WallCheck");
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Anim = GetComponent<Animator>();
        m_Input = GetComponent<CharacterInput>();
    }

    private void LateUpdate()
    {
        m_Anim.SetBool("AnticipatingJump", m_Input.AnticipatingJump);
        m_Anim.SetFloat("VerticalSpeed", Mathf.Abs(m_Rigidbody2D.velocity.y));
        m_Anim.SetBool("Jumping", !Grounded && !Walled);
    }

    private void FixedUpdate()
    {

        Grounded = false;
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                Grounded = true;

        }
        if (!Grounded)
        {
            m_Rigidbody2D.gravityScale = 4f;
        }
        else
        {
            m_Rigidbody2D.gravityScale = 1f;
        }

        //Do the same for walls
        Walled = false;
        colliders = Physics2D.OverlapCircleAll(m_WallCheck.position, k_GroundedRadius, m_WhatIsWall);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                Walled = true;
        }
    }

    public void Move(float move, float maxMovementSpeed, bool jump)
    {

        //only control the player if grounded or airControl is turned on
        if (Grounded || Aircontrol)
        {
            // Move the character
            m_Rigidbody2D.velocity = new Vector2(move * maxMovementSpeed, m_Rigidbody2D.velocity.y);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (jump)
        {
            if (Walled)
            {
                // Add a vertical force to the player.
                Walled = false;
                m_Rigidbody2D.AddForce(new Vector2(0f, 1.5f * m_JumpForce));
            }
            else if (Grounded)
            {
                // Add a vertical force to the player.
                Grounded = false;
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }
    }


    public void Move(float move, bool jump)
    {
        Move(move, m_MaxSpeed, jump);
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        FacingRight = !FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}

