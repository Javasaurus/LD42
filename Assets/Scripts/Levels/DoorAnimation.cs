using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DoorAnimation : MonoBehaviour
{
    /// <summary>
    /// This method is intended to close off a room (for example a boss room can have a special type)
    /// </summary>
    public abstract void SealRoom();        

    /// <summary>
    /// Resumes the game
    /// </summary>
    protected void Resume()
    {
        Time.timeScale = LevelGenerator.initialTimeScale;
        Time.fixedDeltaTime = LevelGenerator.initialFixedTimeScale;
        this.enabled = false;
    }

}
