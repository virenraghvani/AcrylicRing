using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class Tools
{
    [MenuItem("Tools/Clear PlayerPrefs")]
    public static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
