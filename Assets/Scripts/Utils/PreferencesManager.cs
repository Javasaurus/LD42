using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferencesManager : MonoBehaviour
{

    public static PreferencesManager INSTANCE;  //THE singleton instance for the preference manager

    public float musicVol;                      //The music volume
    public float sfxVol;                        //The sfx volume

    public int videoQualityIndex;               //The currently selected quality index
    public int videoResolutionIndex;            //The currently selected resolution index
    public bool videoFullScreen;                //Toggle that enables/disables fullscreen

    public bool ZEN_MODE;

    public void SetZenMode(bool zen)
    {
        ZEN_MODE = zen;
    }

    void print()
    {
        Debug.Log("Zen mode " + ZEN_MODE);
        Debug.Log("Audio Music volume " + musicVol);
        Debug.Log("Audio SFX Volume " + sfxVol);
        Debug.Log("Video Quality " + videoQualityIndex);
        Debug.Log("Video Resolution " + videoResolutionIndex);
        Debug.Log("Video FullScreen " + videoFullScreen);
    }


    private void Awake()
    {
        if (INSTANCE == null)
        {
            INSTANCE = this;
            Load();
            //      print();
            GameObject.FindObjectOfType<SetAudioLevels>().Init();
            GameObject.FindObjectOfType<SetVideoSettings>().Init();
        }
        else
        {
            GameObject.Destroy(this);
        }
    }

    /// <summary>
    /// Loads the stored player prefs. Note that these are only really loaded at startup (which is fine)
    /// </summary>
    public void Load()
    {
        //LOAD AUDIO
        musicVol = PlayerPrefs.GetFloat("musicVol");
        sfxVol = PlayerPrefs.GetFloat("sfxVol");

        //LOAD VIDEO
        videoQualityIndex = PlayerPrefs.GetInt("videoQualityIndex");
        videoResolutionIndex = PlayerPrefs.GetInt("videoResolutionIndex");
        videoFullScreen = PlayerPrefs.GetInt("videoFullScreen") == 1;

        Debug.Log("Loaded settings");
    }

    /// <summary>
    /// Saves the player prefs. This happens when the preference manager object is disabled OR if the application quits
    /// </summary>
    public void Save()
    {
        //SAVE AUDIO
        PlayerPrefs.SetFloat("musicVol", musicVol);
        PlayerPrefs.SetFloat("sfxVol", sfxVol);

        //SAVE VIDEO
        PlayerPrefs.SetInt("videoQualityIndex", videoQualityIndex);
        PlayerPrefs.SetInt("videoResolutionIndex", videoResolutionIndex);
        if (videoFullScreen)
        {
            PlayerPrefs.SetInt("videoFullScreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("videoFullScreen", 0);
        }

        PlayerPrefs.Save();

    }

}
