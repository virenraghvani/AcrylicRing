using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotterRing : MonoBehaviour
{
    public float rotSpeed;
    public GameManager gameManager;

    void Update()
    {
        if(gameManager.isSandingUpperPart)
            transform.Rotate(0, 0, 6.0f * rotSpeed * Time.deltaTime);
        else
            transform.Rotate(0, 0, 6.0f * -rotSpeed * Time.deltaTime);

    }
}
