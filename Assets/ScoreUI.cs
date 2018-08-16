using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI scoreField;
    public TMPro.TextMeshProUGUI roomsField;

    // Update is called once per frame
    void LateUpdate()
    {
        roomsField.enabled = PreferencesManager.INSTANCE && PreferencesManager.INSTANCE.ZEN_MODE;
        roomsField.text = "Room " + LevelGenerator.INSTANCE.ZENROOMS_CLIMBED;
        scoreField.text = String.Format("{0:000000000}", ScoreTimer.GetScore());

    }



}
