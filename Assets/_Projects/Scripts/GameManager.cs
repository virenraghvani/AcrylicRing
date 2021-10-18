﻿using System.Collections;
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

    public Animator brushAnimator, ringHolderAnimator, bottleAnimator, sandingMachineAnimator;

    public RingBlendshapes ringBlendshapes;

    [SerializeField]
    private GameObject sandParticles;

    private bool isAutoCompleting;

    private void Awake()
    {
        ManipulateNailBlob.ProgressChanged += OnProgressChanged;
    }

    private void Destroy()
    {
        ManipulateNailBlob.ProgressChanged -= OnProgressChanged;
    }

    private void Start()
    {
        blobNo = 0;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (blobNo > 1)
            {
                StartCoroutine(MoveRingOut());
                return;
            }
            moveTool.moveToDefaultPos = true;
            rotateCyclinder.Rotate(blobNo);
            blobNo++;

            Invoke("BrushMoveDelay", 0.5f);
        }
    }

    public void OnProgressChanged(float percentage)
    {
        if (percentage > .5f && !isAutoCompleting)
        {
            isAutoCompleting = true;

            if (blobNo > 1)
            {
                StartCoroutine(MoveRingOut());
                return;
            }
            moveTool.moveToDefaultPos = true;
            rotateCyclinder.Rotate(blobNo);
            blobNo++;

            Invoke("BrushMoveDelay", 0.5f);
        }
    }

    void BrushMoveDelay()
    {
        blob[blobNo - 1].GetComponent<ManipulateNailBlob>().enabled = false;
        isAutoCompleting = false;
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

    IEnumerator MoveRingOut()
    {
        ringHolderAnimator.SetTrigger("removeRing");
        brushAnimator.SetTrigger("moveBrushOut");
        bottleAnimator.SetTrigger("moveBottleOut");

        yield return new WaitForSeconds(2);

        sandingMachineAnimator.gameObject.SetActive(true);

        ringHolderAnimator.enabled = false;
        ringBlendshapes.startBlendShape = true;

        sandParticles.SetActive(true);
    }
}
