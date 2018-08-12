﻿using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Button))]
public class FlexibleUIButton : FlexibleUI
{

    Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        base.Initialize();
    }

    protected override void OnSkinUI()
    {
        base.OnSkinUI();
    }

}
