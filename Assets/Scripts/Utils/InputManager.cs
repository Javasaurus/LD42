using UnityEngine;

public class InputManager : MonoBehaviour
{

    public static InputManager INSTANCE;

    public float sprintSpeed;

    //movement
    public Vector2 directionalInput = Vector2.zero;
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode jump = KeyCode.Space;
    public KeyCode sprint = KeyCode.LeftShift;

    //dialog
    public KeyCode dialog = KeyCode.T;

    //aiming and fire
    public KeyCode aimUp = KeyCode.UpArrow;
    public KeyCode aimDown = KeyCode.DownArrow;
    public KeyCode aimLeft = KeyCode.LeftArrow;
    public KeyCode aimRight = KeyCode.RightArrow;

    public Vector2 aimingInput = Vector2.zero;

    public bool handlingDialog;
    public delegate void DialogProcess(bool advance);
    public DialogProcess dialogProgress;

    public delegate void OnJump(bool inputDown);
    public OnJump onJump;

    void Awake()
    {
        if (INSTANCE != null)
        {
            Destroy(gameObject);
        }
        else
        {
            INSTANCE = this;
        }
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleAiming();
        HandleDialog();
    }

    void HandleMovement()
    {
        directionalInput = Vector2.zero;
        if (Input.GetKey(moveDown))
        {
            directionalInput.y -= 1;
        }
        if (Input.GetKey(moveUp))
        {
            directionalInput.y += 1;
        }
        if (Input.GetKey(moveLeft))
        {
            directionalInput.x -= 1;
        }
        if (Input.GetKey(moveRight))
        {
            directionalInput.x += 1;
        }
        if (Input.GetKey(sprint))
        {
            directionalInput.x *= sprintSpeed;
            directionalInput.y *= sprintSpeed;
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyUp(jump))
        {
            onJump(false);
        }
        else if (Input.GetKey(jump))
        {
            onJump(true);
        }
    }

    void HandleDialog()
    {
        if (handlingDialog && (Input.GetKeyUp(dialog)))
        {
            handlingDialog = false;
        }

        if (!handlingDialog)
        {
            if (Input.GetKeyDown(dialog))
            {
                handlingDialog = true;
                if (dialogProgress != null)
                {
                    dialogProgress(true);
                }
            }
        }
    }

    void HandleAiming()
    {
        aimingInput = Vector2.zero;
        if(Input.GetKey(aimDown))
        {
            aimingInput.y -= 1;
        }
        if (Input.GetKey(aimUp))
        {
            aimingInput.y += 1;
        }
        if (Input.GetKey(aimLeft))
        {
            aimingInput.x -= 1;
        }
        if (Input.GetKey(aimRight))
        {
            aimingInput.x += 1;
        }
    }

}
