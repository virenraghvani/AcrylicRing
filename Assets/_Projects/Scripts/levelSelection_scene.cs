using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelSelection_scene : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("firstTime_pp") == 0)
        {
            PlayerPrefs.SetInt("firstTime_pp", 1);
            PlayerPrefs.SetInt("loopCurrLevel_pp", 1);
            PlayerPrefs.SetInt("currLevel_pp", 1);
        }

        SceneManager.LoadScene(PlayerPrefs.GetInt("loopCurrLevel_pp"));
    }
}
