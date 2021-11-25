using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tabtale.TTPlugins;

public class ttpSetup_script : MonoBehaviour
{
    void Awake()
    {
        TTPCore.Setup();
        DontDestroyOnLoad(gameObject);
    }
}
