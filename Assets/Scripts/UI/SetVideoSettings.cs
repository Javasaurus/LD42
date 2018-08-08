using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SetVideoSettings : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    List<Resolution> allowedResolutions;

    List<string> resolutionNames;
    int currentResolutionIndex;

    void Awake()
    {
        QualitySettings.SetQualityLevel(3);
        resolutionDropdown.ClearOptions();
        LoadResolutions();
        resolutionDropdown.AddOptions(resolutionNames);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }


    public void SetVideoQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.Log("Set the quality to " + QualitySettings.GetQualityLevel());
    }

    public void SetFullscreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
        if (!fullScreen)
        {
            Resolution resolution = Screen.currentResolution;
            Screen.SetResolution(resolution.width, resolution.height, fullScreen);
        }
    }


    public void SetResolution(int resolutionIndex)
    {
        if (allowedResolutions.Count > 1)
        {
            Resolution resolution = allowedResolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            resolutionDropdown.RefreshShownValue();
        }
    }

    bool IsSameResolution(Resolution a, Resolution b)
    {
        return a.width == b.width && a.height == b.height && a.refreshRate == b.refreshRate;
    }

    void LoadResolutions()
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


}
