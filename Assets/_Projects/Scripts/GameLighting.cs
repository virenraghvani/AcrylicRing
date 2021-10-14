using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLighting : MonoBehaviour
{
    void Start()
    {
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        RenderSettings.ambientSkyColor = new Color32(255, 246, 213, 255);
        RenderSettings.ambientEquatorColor = new Color32(142, 164, 180, 255);
        RenderSettings.ambientGroundColor = new Color32(202, 191, 169, 255);
    }
}
