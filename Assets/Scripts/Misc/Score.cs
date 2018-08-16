using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    public int score;
    public TMPro.TextMeshProUGUI scoreField;
    public TMPro.TextMeshProUGUI inputField;

    HighScores highScores;

    private void LateUpdate()
    {
        if (scoreField != null)
        {
            score = (int)ScoreTimer.GetScore();
            scoreField.text = score.ToString();
        }
    }

    public void SubmitScore()
    {
        inputField = GameObject.FindGameObjectWithTag("PlayerHighScoreField").GetComponent<TMPro.TextMeshProUGUI>();
        highScores = HighScores.INSTANCE;
        //   highScores.gameObject.SetActive(true);
        //highScores.isReady = true;
        if (!string.IsNullOrEmpty(inputField.text))
        {
            HighScoreUIManager.currentUser = inputField.text;
            highScores.SubmitHighscore(inputField.text, score);
        }
        //  highScores.gameObject.SetActive(false);
    }

}
