using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCyclinder : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKey(KeyCode.Z))
            transform.Rotate(0, 0, .1f, Space.World);
    }
}