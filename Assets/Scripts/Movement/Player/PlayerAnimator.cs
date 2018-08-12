using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlatformerMotor2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAnimator : MonoBehaviour
{

    Animator m_Animator;
    PlatformerMotor2D m_PlatformerMotor2D;
    Rigidbody2D m_Rigidbody;
    // Use this for initialization
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_PlatformerMotor2D = GetComponent<PlatformerMotor2D>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    public bool grounded;
    public bool walled;
    public bool jumping;
    public float horizontal;
    public float vertical;

    // Update is called once per frame
    void Update()
    {
        walled = (m_PlatformerMotor2D.motorState == PlatformerMotor2D.MotorState.WallSliding |
            m_PlatformerMotor2D.motorState == PlatformerMotor2D.MotorState.WallSticking);

        jumping = (m_PlatformerMotor2D.motorState == PlatformerMotor2D.MotorState.Jumping |
            m_PlatformerMotor2D.motorState == PlatformerMotor2D.MotorState.Falling |
            m_PlatformerMotor2D.motorState == PlatformerMotor2D.MotorState.FallingFast);

        horizontal = Mathf.Abs(m_PlatformerMotor2D.velocity.x);
        vertical = Mathf.Abs(m_PlatformerMotor2D.velocity.y);

        grounded = m_PlatformerMotor2D.motorState == PlatformerMotor2D.MotorState.OnGround;

        m_Animator.SetBool("Grounded", grounded);

        m_Animator.SetBool("Walled", walled);

        m_Animator.SetBool("Jumping", jumping);

        m_Animator.SetFloat("VerticalSpeed", vertical);

        m_Animator.SetFloat("HorizontalSpeed", horizontal);
    }
}
