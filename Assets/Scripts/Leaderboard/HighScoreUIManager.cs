using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreUIManager : MonoBehaviour
{

    public GameObject LoadingAnimation;
    public TMPro.TextMeshProUGUI textField;
    public Scrollbar leaderBoardScrollBar;


    public string currentUser = "ElysiaGrifffin";

    void Start()
    {
        HighScores.INSTANCE.onDownloadSuccess += OnHighScoreDownloaded;
    }

    void LateUpdate()
    {
        LoadingAnimation.SetActive(!HighScores.INSTANCE.isReady);
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
