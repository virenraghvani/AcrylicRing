using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCyclinder : MonoBehaviour
{
    public GameObject finalRing;
    public GameObject partRing;

    void Update()
    {
        if(Input.GetKey(KeyCode.Z))
            transform.Rotate(0, 0, .5f, Space.World);

        if (Input.GetKeyDown(KeyCode.A))
        {
            finalRing.SetActive(true);
            partRing.SetActive(false);
        }
    }
}