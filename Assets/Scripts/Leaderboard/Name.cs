using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.XR.OpenXR.Features.Interactions;

public class Name : MonoBehaviour
{
    public Name instance;

    private string playerName = "";


    private void Awake()
    {
        instance = this;
    }
    public void SetName(TMP_Text name)
    {
        if (name != null)
            playerName = name.text;
        else playerName = "AnonymPlayer";

        Debug.Log(playerName);
    }

    
}
