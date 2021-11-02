using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushAnim : MonoBehaviour
{
    public GameManager gameManager;



    public void BrushPlaced()
    {
        gameManager.OnBrushPlaced();
    }

    public void OnBrushDipped()
    {
        gameManager.powderTinAnimator.SetTrigger("onBrushDipped");
    }
}
