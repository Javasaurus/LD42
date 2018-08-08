using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetAudioLevels : MonoBehaviour
{

    public AudioMixer mainMixer;					//Used to hold a reference to the AudioMixer mainMixer
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        PreferencesManager.INSTANCE.Load();
        //LOAD MUSIC SETTINGS FROM PLAYER PREFS
        SetMusicLevel(PreferencesManager.INSTANCE.musicVol);
        SetSfxLevel(PreferencesManager.INSTANCE.sfxVol);
        //here we should also set the sliders to their correct value
        musicSlider.value = PreferencesManager.INSTANCE.musicVol;
        sfxSlider.value = PreferencesManager.INSTANCE.sfxVol;
    }



    //Call this function and pass in the float parameter musicLvl to set the volume of the AudioMixerGroup Music in mainMixer
    public void SetMusicLevel(float musicLvl)
    {
        mainMixer.SetFloat("musicVol", musicLvl);
        if (PreferencesManager.INSTANCE)
        {
            PreferencesManager.INSTANCE.musicVol = musicLvl;
        }
    }

    //Call this function and pass in the float parameter sfxLevel to set the volume of the AudioMixerGroup SoundFx in mainMixer
    public void SetSfxLevel(float sfxLevel)
    {
        mainMixer.SetFloat("sfxVol", sfxLevel);
        if (PreferencesManager.INSTANCE)
        {
            PreferencesManager.INSTANCE.sfxVol = sfxLevel;
        }
    }
}
