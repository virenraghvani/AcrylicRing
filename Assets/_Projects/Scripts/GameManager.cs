﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject [] blob;

    private int blobNo;

    [SerializeField]
    private GameObject brush, blob1, dryingMachine, ringParts, ringFinal, nailPolishBottle, dottedLine, sandingMachine;

    public MeshRenderer ringFinalOutput;

    [SerializeField]
    private SkinnedMeshRenderer ringMesh;

    [SerializeField]
    private Material matShine, matRough;

    public MoveTool moveTool;

    public RotateCyclinder rotateCyclinder;

    public Animator brushAnimator, ringHolderAnimator, bottleAnimator, dryingMachineAnimator;

    public RingBlendshapes ringBlendshapes;

    [SerializeField]
    private GameObject sandParticles;

    private bool isAutoCompleting;

    [SerializeField]
    private GameObject cam1, cam2;

    public static bool IS_READY_FOR_INPUT;

    public bool isSandingUpperPart;

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
                StartCoroutine(StartDrying());
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
            dottedLine.SetActive(false);
            IS_READY_FOR_INPUT = false;

            if (blobNo > 1)
            {
                StartCoroutine(StartDrying());
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

        dottedLine.SetActive(true);
        IS_READY_FOR_INPUT = true;
    }

    IEnumerator StartDrying() {

        ringHolderAnimator.SetTrigger("bottleOut");
        brushAnimator.SetTrigger("moveBrushOut");
        bottleAnimator.SetTrigger("moveBottleOut");

        yield return new WaitForSeconds(.12f);

      


        yield return new WaitForSeconds(1);

        nailPolishBottle.SetActive(false);

        dryingMachineAnimator.gameObject.SetActive(true);

        yield return new WaitForSeconds(.1f);

        ringHolderAnimator.SetTrigger("dryIn");

        yield return new WaitForSeconds(2);

        ringFinal.SetActive(true);
        ringParts.SetActive(false);


        yield return new WaitForSeconds(3);

        ringMesh.material = matRough;
        ringHolderAnimator.SetTrigger("dryOut");

        yield return new WaitForSeconds(1);

        dryingMachineAnimator.SetTrigger("out");

        yield return new WaitForSeconds(1);


        cam1.SetActive(false);
        cam2.SetActive(true);

        sandingMachine.SetActive(true);
        IS_READY_FOR_INPUT = true;

        ringHolderAnimator.enabled = false;
        ringBlendshapes.startBlendShape = true;

        sandParticles.SetActive(true);
    }


    IEnumerator MoveRingOut()
    {
        ringHolderAnimator.SetTrigger("removeRing");
        brushAnimator.SetTrigger("moveBrushOut");
        bottleAnimator.SetTrigger("moveBottleOut");

        yield return new WaitForSeconds(2);

        sandingMachine.SetActive(true);

        ringHolderAnimator.enabled = false;
        ringBlendshapes.startBlendShape = true;

        sandParticles.SetActive(true);
    }

    public void SandingUpperDone()
    {
        Debug.Log("rotate ring");

        ringHolderAnimator.enabled = true;
        ringHolderAnimator.SetTrigger("ringRotate");

        Invoke("ReadyForSandingBottomPart", 1);
    }

    public void SandingBottomDone()
    {
        ringHolderAnimator.enabled = true;
    }

    void ReadyForSandingBottomPart()
    {
        IS_READY_FOR_INPUT = true;
        ringHolderAnimator.enabled = false;
    }
}
