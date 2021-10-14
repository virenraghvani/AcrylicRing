using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinAnim : MonoBehaviour
{
    public GameManager gameManager;


    public void OnTinEntered()
    {
        gameManager.OnTinEntered();
    }
}
