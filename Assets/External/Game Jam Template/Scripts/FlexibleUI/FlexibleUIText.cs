using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class FlexibleUIText : FlexibleUI

{
    private TMPro.TextMeshProUGUI text;


    void Awake()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
        base.Initialize();
    }

    protected override void OnSkinUI()
    {
        base.OnSkinUI();
 

    }



}
