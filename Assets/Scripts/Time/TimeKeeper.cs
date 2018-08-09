using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeKeeper : MonoBehaviour
{
    public static TimeKeeper Instance;

    public float inGameSpeed = 5;                           // The factor to speed up the game with
    public TimeSpan GameTime = new TimeSpan();              // Current adjusted game time
    private float gameSeconds = 0;                          // Accumulated 'Game Time' in seconds ---> LOAD THIS FROM A SAVE FILE LATER

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            GameObject.Destroy(this);
        }
    }

    public void Update()
    {
        if (inGameSpeed != 0)
        {
            gameSeconds += Time.deltaTime / (1f/ inGameSpeed);
        }
        GameTime = TimeSpan.FromSeconds(gameSeconds);       // Convert to a usable format
    }


}
