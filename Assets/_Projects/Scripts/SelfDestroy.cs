using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public void SelfDest()
    {
        Destroy(gameObject);
    }

    public void DeActivate()
    {
        gameObject.SetActive(false);
    }
}
