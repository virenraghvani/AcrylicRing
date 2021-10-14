using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCyclinder : MonoBehaviour
{
    public GameObject finalRing;
    public GameObject partRing;

    public float rotationX, rotationY, rotationZ;

    public Vector3[] rotationVal;

    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
            transform.Rotate(0, 1f, 0, Space.Self);

        if (Input.GetKeyDown(KeyCode.A))
        {
            finalRing.SetActive(true);
            partRing.SetActive(false);
        }

        rotationX = transform.localEulerAngles.x;
        rotationY = transform.localEulerAngles.y;
        rotationZ = transform.localEulerAngles.z;

    }


    public void Rotate(int index)
    {
        LeanTween.rotateLocal(gameObject, rotationVal[index], 1);
    }
    // 355.2581, 235.2214, 90.06071
    //297.742, 55.34146, 269.87
}