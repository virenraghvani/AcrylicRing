using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCyclinder : MonoBehaviour
{
    public Vector3[] rotationVal;

    public void Rotate(int index)
    {
        LeanTween.rotateLocal(gameObject, rotationVal[index], 1);
    }
    // 355.2581, 235.2214, 90.06071
    //297.742, 55.34146, 269.87
}