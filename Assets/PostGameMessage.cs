using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostGameMessage : MonoBehaviour
{

    public static string END_MESSAGE;
    public TMPro.TextMeshProUGUI deathmsgField;

    private void OnEnable()
    {
        deathmsgField.text = END_MESSAGE;
    }

}
