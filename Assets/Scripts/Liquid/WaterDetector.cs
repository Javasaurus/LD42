using UnityEngine;
using System.Collections;

public class WaterDetector : MonoBehaviour
{
    public bool onEnter = true;
    public bool onStay = false;

    void EnableRenderer(Collider2D other, bool enable)
    {
        Renderer renderer = other.GetComponent<Renderer>();
        if (renderer)
        {
            renderer.enabled = enable;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (onEnter)
        {
            Handle(other);
        }
        EnableRenderer(other, false);
    }



    void OnTriggerExit2D(Collider2D other)
    {
        if (onEnter)
        {
            Handle(other);
        }
        EnableRenderer(other, true);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (onStay)
        {
            Handle(other);
        }
        EnableRenderer(other, false);
    }

    void Handle(Collider2D other)
    {
        PostGameMessage.END_MESSAGE = "The oil engulfed you !";
    }

}
