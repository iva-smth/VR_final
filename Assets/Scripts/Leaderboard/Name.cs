using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.XR.OpenXR.Features.Interactions;

public class Name : MonoBehaviour
{
    private string playerName = "";

    public void SetName(string name)
    {
        if (name != null)
            playerName = name;
        else playerName = "AnonymPlayer";
    }
}
