using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoad : MonoBehaviour
{
    public Color lockedColor = Color.red;           //The color for a locked block
    public Color unlockedColor = Color.green;       //The color for an unlocked block
    public int LevelSceneIndex;                     //The scene index for the level that needs to be loaded with this

    public bool Locked;                             //Bool indicating if this level is locked (save unlocked levels in player prefs?)


    void Update()
    {
             //change this later to, this is for testing
            if (!Locked)
            {
                Unlock();
            }
            else
            {
                Lock();
            }   
    }

    void Lock()
    {
        GetComponent<Renderer>().materials[0].color = lockedColor;
        Locked = true;
    }

    void Unlock()
    {
        GetComponent<Renderer>().materials[0].color = unlockedColor;
        Locked = false;
    }

    private void OnMouseDown()
    {
        OverworldCharacter owc = GameObject.FindObjectOfType<OverworldCharacter>();
        owc.OnLevelSelected(this);
    }

}
