using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DialogOwner : MonoBehaviour {

    public string DialogFileName;           // The name of the dialog to be loaded (needs to be in Resources/Dialog)
    public UnityEngine.GameObject DialogIdentifier;     // The identifier for the game object (for example a quest icon)

    private void Awake()
    {
        DialogIdentifier.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            DialogController.INSTANCE.gameObject.SetActive(true);
            DialogController.INSTANCE.LoadDialogFile(DialogFileName);
            DialogController.IN_DIALOG_RANGE = true;
            DialogIdentifier.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            //stop a dialog
            DialogController.INSTANCE.GetComponent<ScaleRoutine>().Scale(Vector3.zero);
            DialogController.IN_DIALOG_RANGE = false;
            DialogIdentifier.SetActive(false);
        }
    }
}
