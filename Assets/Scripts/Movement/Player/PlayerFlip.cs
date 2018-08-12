using UnityEngine;

public class PlayerFlip : MonoBehaviour
{

    bool facingRight = true;

    void Flip()
    {

        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

    }

    void FixedUpdate()
    {
        if (Input.GetAxis("Horizontal") > 0 && !facingRight)
        {
            Flip();
        }
        else if (Input.GetAxis("Horizontal") < 0 && facingRight)
        {
            Flip();
        }
    }

}
