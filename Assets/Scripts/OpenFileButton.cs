using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OpenFileButton : MonoBehaviour
{
    public string path;

    public void OpenDialog()
    {
        path = EditorUtility.OpenFilePanel("Open file", "", "*");
    }
}