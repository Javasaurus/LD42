using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITag : MonoBehaviour
{
    public UnityEngine.GameObject Owner;                    // The owner of this tag
    private Canvas tagCanvas;                   // The canvas that is used to display this tag
    private TMPro.TextMeshProUGUI tagField;

    private void Start()
    {
        tagCanvas = UnityEngine.GameObject.FindGameObjectWithTag("UI_Canvas").GetComponent<Canvas>();
        tagField = GetComponent<TMPro.TextMeshProUGUI>();
        gameObject.name = Owner.name + "_TAG";
        tagField.text = Owner.name;
    }

    private void Update()
    {
        Reposition();
    }


    /// <summary>
    /// Moves the tag to be located over the game object
    /// </summary>
    public void Reposition()
    {

        transform.position = WorldToUISpace(Owner.transform.position+new Vector3(0,-1.2f,0));
    }

    /// <summary>
    /// Repositions a world position to the display canvas
    /// </summary>
    /// <param name="worldPos">The world vector</param>
    /// <returns></returns>
    private Vector3 WorldToUISpace(Vector3 worldPos)
    {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(tagCanvas.transform as RectTransform, screenPos, tagCanvas.worldCamera, out movePos);
        //Convert the local point to world point
        return tagCanvas.transform.TransformPoint(movePos);
    }
}
