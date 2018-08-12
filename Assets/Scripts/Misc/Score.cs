using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    public int score;
    public TMPro.TMP_InputField inputField;

    HighScores highScores;

    public void SubmitScore()
    {
        if (highScores == null)
        {
            highScores = GameObject.Find("References").GetComponent<References>().hs;
        }
        highScores.gameObject.SetActive(true);
        highScores.isReady = true;
        highScores.SubmitHighscore(inputField.text, score);
        highScores.gameObject.SetActive(false);
    }

}
