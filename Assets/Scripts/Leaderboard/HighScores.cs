using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the link to your "specific" leaderboard is http://dreamlo.com/lb/7lmmwoSpW0C6uK-RHArZ8gAYB1_OBJm0SSnt8foJH6PQ, it has a console 
//you can use to manage high scores etc

public class HighScores : MonoBehaviour
{
    //the static instance (to make sure there is only one of these things contacting the server)
    public static HighScores INSTANCE;
    //Get this from the website
    const string privateCode = "7lmmwoSpW0C6uK-RHArZ8gAYB1_OBJm0SSnt8foJH6PQ";
    //Get this from the website
    const string publicCode = "5b674642191a8b0bcc92edaf";
    //the link to the actual repository of scores
    const string webURL = "http://dreamlo.com/lb/";
    //a list of highscore entries
    public List<HighScore> HighScoreTable;
    //a targetted highscore
    public HighScore userHighScore;
    //boolean indicating weather the upload / download of highscores has finished
    public bool isReady;

    //DELEGATES IN CASE ACTIONS ARE FINISHED OR FAILED (upload/download etc)
    public delegate void OnUploadSuccess();
    public OnUploadSuccess onUploadSuccess;

    public delegate void OnUploadFail();
    public OnUploadFail onUploadFail;

    public delegate void OnDownloadSuccess();
    public OnDownloadSuccess onDownloadSuccess;

    public delegate void OnDownloadFail();
    public OnDownloadFail onDownloadFail;


    private void Awake()
    {
        if (INSTANCE == null)
        {
            INSTANCE = this;
            //set the delegates
            onUploadSuccess += OnUploadCompleted;
            onDownloadSuccess += OnDownloadCompleted;
            onUploadFail += OnUploadFailed;
            onDownloadFail += OnDownloadFailed;
            //retrieve the highscores at the beginning (this can be done later as well, just seemed handy here)
            GetHighScores();
        }
        else
        {
            UnityEngine.GameObject.Destroy(this);
        }
    }

    /// <summary>
    /// Gets called when the scores are succesfully uploaded
    /// </summary>
    public void OnUploadCompleted()
    {
        Debug.Log("Highscore was submitted !");
    }

    /// <summary>
    /// Gets called when a download was completed (single or multiple highscores)
    /// </summary>
    public void OnDownloadCompleted()
    {
        Debug.Log("HighScoresRetrieved were retrieved !");
    }

    /// <summary>
    /// Called when the upload failed
    /// </summary>
    public void OnUploadFailed()
    {
        Debug.Log("Highscore was NOT submitted !");
    }

    /// <summary>
    /// Called when the download failed
    /// </summary>
    public void OnDownloadFailed()
    {
        Debug.Log("HighScoresRetrieved were NOT retrieved !");
    }

    /// <summary>
    /// Submis a highscore (no login system in place so theoretically
    /// everyone can post under the same name,but only the best one 
    /// gets saved by the server)
    /// </summary>
    /// <param name="username">The username that is posting the score</param>
    /// <param name="score">The integer value of the score</param>
    public void SubmitHighscore(string username, int score)
    {
        if (isReady)
        {
            StartCoroutine(UploadNewHighscore(username, score));
        }
    }

    IEnumerator UploadNewHighscore(string username, int score)
    {
        isReady = false;
        //this construct a link to the speficic url to add a score to your repository
        WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score);
        yield return www;
        //if there is no error (www.error string is empty)...
        if (string.IsNullOrEmpty(www.error))
        {
            onUploadSuccess();
        }
        else
        {
            onUploadFail();
            Debug.Log("Error uploading ! " + www.error);
        }
        //at this point we are ready with the upload!
        isReady = true;

    }

    /// <summary>
    /// Retrieves the high scores
    /// </summary>
    public void GetHighScores()
    {
        StartCoroutine(DownloadHighscores());
    }

    IEnumerator DownloadHighscores()
    {
        isReady = false;
        WWW www = new WWW(webURL + publicCode + "/pipe/");
        yield return www;
        //we yield the www until it has fully loaded, then we check for error to be empty
        if (string.IsNullOrEmpty(www.error))
        {
            LoadHighscores(www.text);
            onDownloadSuccess();
        }
        else
        {
            onDownloadFail();
            //something went wrong and I want to print it
            Debug.Log("Error downloading ! " + www.error);
        }
        isReady = true;
    }

    /// <summary>
    /// Retrieves a specific highscore
    /// </summary>
    /// <param name="username"></param>
    public void GetHighScore(string username)
    {
        StartCoroutine(DownloadSingleHighscore(username));
    }

    IEnumerator DownloadSingleHighscore(string username)
    {
        isReady = false;
        WWW www = new WWW(webURL + publicCode + "/pipe-get/" + username);
        yield return www;
        //we yield the www until it has fully loaded, then we check for error to be empty
        if (string.IsNullOrEmpty(www.error))
        {
            userHighScore = ParseHighscore(www.text);
            onDownloadSuccess();
        }
        else
        {
            //something went wrong and I want to print it
            onDownloadFail();
            Debug.Log("Error downloading ! " + www.error);
        }
        isReady = true;
    }

    void LoadHighscores(string textStream)
    {
        //this method parses the received stream (the webpage content basically) into highscore objects (see below)
        string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        HighScoreTable = new List<HighScore>();
        for (int i = 0; i < entries.Length; i++)
        {
            HighScoreTable.Add(ParseHighscore(entries[i]));
        }
    }

    HighScore ParseHighscore(string line)
    {
        string[] entryInfo = line.Split(new char[] { '|' });
        string username = WWW.UnEscapeURL(entryInfo[0]);
        int score = int.Parse(entryInfo[1]);
        int rank = int.Parse(entryInfo[entryInfo.Length - 1])+1;
        HighScore highScore = new HighScore
        {
            username = username,
            score = score,
            rank = rank
        };
        return highScore;
    }

}

[System.Serializable]
public class HighScore
{
    public string username = "";
    public int score = 0;
    public int rank = int.MaxValue;

    public override string ToString()
    {
        return rank + "\t" + username + "\t" + score;
    }
}