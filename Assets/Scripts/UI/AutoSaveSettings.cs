using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSaveSettings : MonoBehaviour
{

    //saves to player prefs on disable
    private void OnDisable()
    {
        if (PreferencesManager.INSTANCE)
        {
            PreferencesManager.INSTANCE.Save();
            Debug.Log("Saved settings");
        }
    }

    //saves to player prefs on application quit (TODO does this also handle a crash? )
    private void OnApplicationQuit()
    {
        if (PreferencesManager.INSTANCE)
        {
            Debug.Log("Saved settings");
        }
    }


}
