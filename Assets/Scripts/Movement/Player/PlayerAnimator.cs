using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlatformerMotor2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAnimator : MonoBehaviour
{

    public PlayerAnimationSprites jumpSprites;
    public PlayerAnimationSprites runSprites;
    public PlayerAnimationSprites wallSprites;
    public PlayerAnimationSprites idleSprites;

    public bool shooting;
    public bool grounded;
    public bool walled;
    public bool jumping;
    public float horizontal;
    public float vertical;

    SpriteRenderer m_renderer;
    Animator m_Animator;
    PlatformerMotor2D m_PlatformerMotor2D;
    Rigidbody2D m_Rigidbody;
    // Use this for initialization
    void Start()
    {
        m_renderer = GetComponent<SpriteRenderer>();
        m_Animator = GetComponent<Animator>();
        m_PlatformerMotor2D = GetComponent<PlatformerMotor2D>();
        m_Rigidbody = GetComponent<Rigidbody2D>();

    }




    private void LateUpdate()
    {
        Sprite currentSprite = m_renderer.sprite;
        Sprite[] spriteSheet;
        //we are running?
        if (walled)
        {
            spriteSheet = shooting ? wallSprites.shoot_Sprites : wallSprites.regular_Sprites;
            currentSprite = GetSpriteByName(spriteSheet);
        }
        else if (jumping | vertical > 0.05f)
        {
            spriteSheet = shooting ? jumpSprites.shoot_Sprites : jumpSprites.regular_Sprites;
            currentSprite = GetSpriteByName(spriteSheet);
        }
        else if (horizontal > 0.05f)
        {
            spriteSheet = shooting ? runSprites.shoot_Sprites : runSprites.regular_Sprites;
            currentSprite = GetSpriteByName(spriteSheet);
        }
        else
        {
            if (shooting)
            {
                currentSprite = idleSprites.shoot_Sprites[0];
            }
        }

        m_renderer.sprite = currentSprite;
    }

    private Sprite GetSpriteByName(Sprite[] spriteSheet)
    {
        foreach (Sprite sprite in spriteSheet)
        {
            if (sprite.name == m_renderer.sprite.name)
            {
                return sprite;
            }
        }
        return spriteSheet[0];
    }

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

[System.Serializable]
public class PlayerAnimationSprites
{
    public Sprite[] regular_Sprites;
    public Sprite[] shoot_Sprites;
}