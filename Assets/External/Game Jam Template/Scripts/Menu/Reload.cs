using UnityEngine;
using UnityEngine.SceneManagement;

public class Reload : MonoBehaviour
{
    public static int currentLevel;

    public GameObject DeathUI;

    public void Rld()
    {
        ScoreTimer.STOP = false;
        LevelGenerator.INSTANCE.ReloadLevel();

        //Destroy all missiles ---> player bullets dissapear fast enough, no worries
        foreach (SeekingMissile missile in GameObject.FindObjectsOfType<SeekingMissile>())
        {
            GameObject.Destroy(missile);
        }

      //  ---> what the heck is going on with the camera ????

        Time.timeScale = 1;
        GameObject.FindObjectOfType<Health>().ResetStats();
        DeathUI.SetActive(false);
    }



}
