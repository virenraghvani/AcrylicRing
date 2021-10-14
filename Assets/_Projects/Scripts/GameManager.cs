using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject [] blob;

    private int blobNo;

    [SerializeField]
    private GameObject brush, blob1;


    private void Start()
    {
        blobNo = 1;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            blob[blobNo].SetActive(true);
            blobNo++;
        }
    }

    public void OnTinEntered()
    {
        // Activate Blob
        brush.SetActive(true);
    }

    public void OnBrushPlaced()
    {
        // Activate Blob
        blob1.SetActive(true);
    }
}
