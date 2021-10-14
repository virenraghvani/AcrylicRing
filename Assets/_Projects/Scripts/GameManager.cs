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


    public MoveTool moveTool;

    public RotateCyclinder rotateCyclinder;

    public Animator brushAnimator;

    private void Start()
    {
        blobNo = 0;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            moveTool.moveToDefaultPos = true;
            rotateCyclinder.Rotate(blobNo);
            blobNo++;

            Invoke("BrushMoveDelay", 0.5f);
        }
    }

    void BrushMoveDelay()
    {
        brushAnimator.Play("Brush", -1, 0f);
    }

    public void OnTinEntered()
    {
        // Activate Blob
        brush.SetActive(true);
    }

    public void OnBrushPlaced()
    {
        // Activate Blob
        blob[blobNo].SetActive(true);
    }
}
