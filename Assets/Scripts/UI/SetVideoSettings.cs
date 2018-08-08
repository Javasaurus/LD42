using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SetVideoSettings : MonoBehaviour
{
    public Dropdown resolutionDropdown;             // the dropdown box for resolutions
    public Dropdown qualityDropdown;                // the dropdown box for quality settings
    public Toggle fullScreenToggle;                 // toggle for full screen

    List<Resolution> allowedResolutions;            // the list of allowed resolutions (in relation to the refresh rate etc (59 or 60 hz)
    List<string> resolutionNames;                   // list of resolution names to populate the drop down
    int currentResolutionIndex;                     // the active resolution

    void Start()
    {
        Init();
    }

    public void Init()
    {
        PreferencesManager.INSTANCE.Load();

        //LOAD QUALITY FROM PLAYER PREFS
        if (PreferencesManager.INSTANCE.videoQualityIndex > 0 && PreferencesManager.INSTANCE.videoQualityIndex < QualitySettings.names.Length)
        {
            SetVideoQuality(PreferencesManager.INSTANCE.videoQualityIndex);
            qualityDropdown.value = PreferencesManager.INSTANCE.videoQualityIndex;
            qualityDropdown.RefreshShownValue();
        }

        InitResolutions();

        //LOAD RESOLUTION FROM PLAYER PREFS
        if (PreferencesManager.INSTANCE.videoResolutionIndex > 0 && PreferencesManager.INSTANCE.videoResolutionIndex < allowedResolutions.Count)
        {
            SetResolution(PreferencesManager.INSTANCE.videoResolutionIndex);
            currentResolutionIndex = PreferencesManager.INSTANCE.videoResolutionIndex;
        }
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        //LOAD FULLSCREEN FROM PLAYER PREFS
        SetFullscreen(PreferencesManager.INSTANCE.videoFullScreen);

    }

    /// <summary>
    /// Initializes the resolutions for the drop down
    /// </summary>
    void InitResolutions()
    {
        resolutionDropdown.ClearOptions();
        FindFitResolutions();
        resolutionDropdown.AddOptions(resolutionNames);
    }

    /// <summary>
    /// Discovers available resolutions that are actually 
    /// working on this display
    /// </summary>
    void FindFitResolutions()
    {
        //we can assume the current refreshrate is the one to use !
        int currentRefreshRate = Screen.currentResolution.refreshRate;

        //DIRTY WAY TO CLEAN IT UP , DID NOT WORK WITH LINQ FOR SOME REASON
        Resolution[] resolutions = Screen.resolutions;

        //filter out refreshrates
        List<Resolution> applicableResolutions = new List<Resolution>();
        foreach (Resolution resolution in resolutions)
        {
            if (resolution.refreshRate == currentRefreshRate)
            {
                applicableResolutions.Add(resolution);
            }
        }

        //filter out identicals
        allowedResolutions = new List<Resolution>();
        foreach (Resolution resolution in applicableResolutions)
        {
            if (allowedResolutions.Count == 0)
            {
                allowedResolutions.Add(resolution);
            }
            else
            {
                //check if it is already there or not
                foreach (Resolution allowedResolution in allowedResolutions)
                {
                    if (!IsSameResolution(resolution, allowedResolution))
                    {
                        allowedResolutions.Add(resolution);
                        break;
                    }
                }
            }
        }

        // put these into the available list
        //populate the resolutions
        resolutionNames = new List<string>();
        currentResolutionIndex -= 0;
        for (int i = 0; i < allowedResolutions.Count; i++)
        {
            resolutionNames.Add(allowedResolutions[i].width + " x " + allowedResolutions[i].height);
            if (IsSameResolution(allowedResolutions[i], Screen.currentResolution))
            {
                currentResolutionIndex = i;
            }
        }
        if (resolutionNames.Count == 0)
        {
            resolutionNames.Add("Default");
        }
    }

    /// <summary>
    /// Sets the video quality
    /// </summary>
    /// <param name="qualityIndex"> The index of the quality option (see in Unity inspector in Edit > Project Settings > Quality )</param>
    public void SetVideoQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.Log("Set the quality to " + QualitySettings.GetQualityLevel());
        PreferencesManager.INSTANCE.videoQualityIndex = qualityIndex;
    }

    /// <summary>
    /// Sets the resolution to the chosen format
    /// </summary>
    /// <param name="resolutionIndex">The index of the currently selected resolution</param>
    public void SetResolution(int resolutionIndex)
    {
        if (allowedResolutions == null)
        {
            InitResolutions();
        }
        if (allowedResolutions.Count > 1)
        {
            Resolution resolution = allowedResolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            resolutionDropdown.RefreshShownValue();
            PreferencesManager.INSTANCE.videoResolutionIndex = resolutionIndex;
        }
    }

    /// <summary>
    /// Sets the screen to fullscreen if enabled
    /// </summary>
    /// <param name="fullScreen">is fullScreen</param>
    public void SetFullscreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
        if (!fullScreen)
        {
            Resolution resolution = Screen.currentResolution;
            Screen.SetResolution(resolution.width, resolution.height, fullScreen);
        }
        fullScreenToggle.isOn = fullScreen;
        PreferencesManager.INSTANCE.videoFullScreen = fullScreen;
    }



    /// <summary>
    /// Verifies if resolutions are identical (there is no static operator for Resolutions)
    /// </summary>
    /// <param name="a"> the first resolution </param>
    /// <param name="b"> the second resolution </param>
    /// <returns>true if the resolutions are equal</returns>
    bool IsSameResolution(Resolution a, Resolution b)
    {
        return a.width == b.width && a.height == b.height && a.refreshRate == b.refreshRate;
    }




}
