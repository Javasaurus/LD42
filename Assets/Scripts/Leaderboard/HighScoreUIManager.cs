using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreUIManager : MonoBehaviour
{

    public UnityEngine.GameObject LoadingAnimation;                 // A loading indicator (for example text or a spinner)
    public TMPro.TextMeshProUGUI textField;             // A textfield where the highscores will be written to
    public Scrollbar leaderBoardScrollBar;              // The leaderboard's scrollbar (autopositioning to the submitted value)
    public string currentUser = "";                     // The current user (last one that submitted will be displayed in RED)

    void Start()
    {
        //  HighScores.INSTANCE.onDownloadSuccess += OnHighScoreDownloaded;
        timer = Time.time + updateDelay;
    }

    float timer = 0f;
    float updateDelay = 15f;

    void LateUpdate()
    {
        if (Time.time > timer)
        {
            HighScores.INSTANCE.HighScoreTable.Clear();
            HighScores.INSTANCE.GetHighScores();
            timer = Time.time + updateDelay;
        }
        if (!HighScores.INSTANCE.isReady)
        {
            LoadingAnimation.SetActive(true);
        }
        else
        {
            LoadingAnimation.SetActive(false);
            textField.text = "";
            OnHighScoreDownloaded();
        }

    }

    public void OnHighScoreDownloaded()
    {
        List<HighScore> scores = HighScores.INSTANCE.HighScoreTable;
        float scrollPosition = 0f;
        foreach (HighScore score in scores)
        {
            if (!string.IsNullOrEmpty(currentUser) && (score.username.ToLower() == currentUser.ToLower()))
            {
                //then set the value of the scroll rect to this
                if (score.rank >= 3)
                {
                    scrollPosition = (float)(score.rank - 3) / scores.Count;
                }
                else
                {
                    scrollPosition = (float)(score.rank) / scores.Count;
                }

                textField.text += "<color=red>" + score.ToString() + "</color>" + Environment.NewLine;
            }
            else
            {
                textField.text += score.ToString() + Environment.NewLine;
            }
        }
        Canvas.ForceUpdateCanvases();
        leaderBoardScrollBar.value = scrollPosition;
        Canvas.ForceUpdateCanvases();
    }


}
