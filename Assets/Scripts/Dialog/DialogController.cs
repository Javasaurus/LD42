using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    public bool AutoScrollToSpeaker;                //boolean indicating if the camera should move to the dialog owner
    public static DialogController INSTANCE;        //the singleton instance
    public static bool IN_DIALOG_RANGE;             //boolean indicating wether the player is in range for a certain conversation
    public Canvas DialogCanvas;                     //the dialog canvas
    public TMPro.TextMeshProUGUI CharacterBox;      //Text box for the character name (add a portrait later?)
    public TMPro.TextMeshProUGUI DialogBox;         //Text box for the line to display
    private float _currentScale = 0;                //boolean for the current scale (for the expanding/shrinking effect of dialog boxes)
    public string CurrentDialogName;                //the name of the current dialog
    private int currentLineIndex = 0;               //the current index of the displayed line
    private DialogLine[] lines;                     //the lines to display
    private bool hasTextChanged;                    //bool indicating if text needs to reload
    private bool hasTextSkipped;                    //bool indicating if the text was skipped
    private bool dialogStarted;                     //bool indicating if the dialog has started
    private bool ready;                             //bool indicating if the dialog is ready to advance



    private void Awake()
    {
        if (INSTANCE == null)
        {
            INSTANCE = this;
        }
        else
        {
            UnityEngine.GameObject.Destroy(this.gameObject);
        }
    }

    void Start()
    {
        InputManager.INSTANCE.dialogProgress += ProcessDialog;
        StartCoroutine(RevealCharacters());
        GetComponent<ScaleRoutine>().Scale(Vector3.zero);
    }


    /*
     * This method moves the dialog window over the character name 
     * (which currently HAS to be the same as the gameobject's name, 
     * but we can change that later on)
     *
     **/
    public void Reposition(string characterName)
    {
        GameObject dialogTarget = GameObject.Find(characterName);
        if (dialogTarget)
        {
            if (AutoScrollToSpeaker)
            {
                CameraFollow follow = GameObject.FindObjectOfType<CameraFollow>();
                follow.paused = true;
                follow.transform.position = new Vector3(dialogTarget.transform.position.x, dialogTarget.transform.position.y, follow.transform.position.z);

            }
            transform.position = WorldToUISpace(new Vector3(dialogTarget.transform.position.x + 0.75f, dialogTarget.transform.position.y + 1.75f));
        }
        else
        {
            transform.position = Vector3.zero;
        }
    }

    /*
     * Converts the coordinates of the world to the dialog canvas, so it can display a textbox at the appropriate place
     * 
     */
    private Vector3 WorldToUISpace(Vector3 worldPos)
    {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(DialogCanvas.transform as RectTransform, screenPos, DialogCanvas.worldCamera, out movePos);
        //Convert the local point to world point
        return DialogCanvas.transform.TransformPoint(movePos);
    }

    /*
     * Loads a dialog from a file (in Resources/Dialog/) 
     * This is passed on from the dialog trigger
     */
    public void LoadDialogFile(string dialogName)
    {
        dialogStarted = false;
        if (CurrentDialogName != dialogName)
        {
            TextAsset dialogAsset = Resources.Load("Dialog/" + dialogName) as TextAsset;
            string[] fLines = dialogAsset.text.Split("\n"[0]);
            lines = new DialogLine[fLines.Length];
            int counter = 0;
            foreach (string fLine in fLines)
            {
                string[] content = fLine.Split('\t');
                DialogLine dLine = new DialogLine();
                dLine.character = content[0];
                dLine.text = content[1];
                lines[counter] = dLine;
                counter++;
            }
        }
        else
        {
            currentLineIndex = 0;
        }
    }

    /*
     * Starts the dialog (called by pressing T for example)
     */
    public void StartDialog()
    {
        currentLineIndex = 0;
        DisplayText(lines[currentLineIndex]);
        dialogStarted = true;
    }

    /*
     * This method advances the dialog in the specified direction (true = forward) 
     * (At the moment not yet using the functionality for returning)
     */
    public void ProcessDialog(bool direction)
    {
        if (!IN_DIALOG_RANGE)
        {
            return;
        }
        if (!dialogStarted)
        {
            StartDialog();
        }
        else if (direction)
        {
            if (ready)
            {
                int newIndex = currentLineIndex + 1;
                if (newIndex >= lines.Length)
                {
                    //here the camera should be unlocked again
                    if (AutoScrollToSpeaker)
                    {
                        FindObjectOfType<CameraFollow>().paused = false;
                    }
                    GetComponent<ScaleRoutine>().Scale(Vector3.zero);
                    newIndex = 0;
                }
                else if (newIndex != currentLineIndex)
                {
                    currentLineIndex = newIndex;
                    DisplayText(lines[currentLineIndex]);
                }
            }
            else
            {
                hasTextSkipped = true;
            }
        }
    }


    /// <summary>
    /// Method revealing the text one character at a time.
    /// </summary>
    /// <returns></returns>
    public void DisplayText(DialogLine line)
    {
        GetComponent<ScaleRoutine>().Scale(Vector3.zero);
        DialogBox.text = line.text;
        hasTextChanged = true;
        DialogBox.maxVisibleCharacters = 0;
        CharacterBox.text = line.character;
        Reposition(line.character);
        GetComponent<ScaleRoutine>().Scale(Vector3.one);

    }


    /// <summary>
    /// Method revealing the text one character at a time.
    /// </summary>
    /// <returns></returns>
    IEnumerator RevealCharacters()
    {
        DialogBox.ForceMeshUpdate();

        TMP_TextInfo textInfo = DialogBox.textInfo;

        int totalVisibleCharacters = textInfo.characterCount; // Get # of Visible Character in text object
        int visibleCount = 0;

        while (true)
        {
            if (hasTextSkipped)
            {
                visibleCount = textInfo.characterCount;
                totalVisibleCharacters = textInfo.characterCount;
                hasTextSkipped = false;
            }
            else if (hasTextChanged)
            {
                visibleCount = 0;
                totalVisibleCharacters = textInfo.characterCount; // Update visible character count.
                hasTextChanged = false;
            }


            DialogBox.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

            visibleCount = Mathf.Min(textInfo.characterCount, visibleCount + 1);

            ready = (visibleCount == textInfo.characterCount);

            yield return null;
        }




    }


}

[System.Serializable]
public struct DialogLine
{
    public string character;
    public string text;
}