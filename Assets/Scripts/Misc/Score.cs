using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    public int score;
    public InputField inputField;

    public void SubmitScore()
    {
        GameObject.Find("References").GetComponent<References>().hs.SubmitHighscore(inputField.text, score);
    }

}
