using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject [] blob;

    private int blobNo;

    [SerializeField]
    private GameObject brush, blob1, dryingMachine, ringParts, ringFinal, nailPolishBottle, dottedLine, sandingMachine, accessoryStep, finalHand, finalConfetti;

    public MeshRenderer ringFinalOutput;

    [SerializeField]
    private MeshRenderer ringMesh;

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
    private GameObject cam1, cam2_dryer, cam3, cam4_hand, cam4_hand1, cam4_hand2;

    public static bool IS_READY_FOR_INPUT;

    public bool isSandingUpperPart;

    [SerializeField]
    private CinemachineBrain cinemachineBrain;

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

        cam1.SetActive(false);
        cam2_dryer.SetActive(true);

        ringHolderAnimator.SetTrigger("bottleOut");
        brushAnimator.SetTrigger("moveBrushOut");
        bottleAnimator.SetTrigger("moveBottleOut");

        yield return new WaitForSeconds(.12f);

      


        yield return new WaitForSeconds(1);


        dryingMachineAnimator.gameObject.SetActive(true);

        yield return new WaitForSeconds(.1f);

        ringHolderAnimator.SetTrigger("dryIn");

        yield return new WaitForSeconds(2);

        ringFinal.SetActive(true);
        ringParts.SetActive(false);


        yield return new WaitForSeconds(3);

        ringMesh.material = matRough;
        dryingMachineAnimator.SetTrigger("out");

        yield return new WaitForSeconds(1);

        ringHolderAnimator.SetTrigger("dryOut");

        yield return new WaitForSeconds(1);

        nailPolishBottle.SetActive(false);

        cam2_dryer.SetActive(false);
        cam3.SetActive(true);

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
        ringHolderAnimator.SetTrigger("accessoryPos");

        sandingMachine.SetActive(false);

        Invoke("DelayAccessoryStep", 1);
    }

    void ReadyForSandingBottomPart()
    {
        IS_READY_FOR_INPUT = true;
        ringHolderAnimator.enabled = false;
    }

    void DelayAccessoryStep()
    {
        accessoryStep.SetActive(true);
    }

    public void OnAccessoryDone()
    {
       

        StartCoroutine(StartHandStep());
    }

    IEnumerator StartHandStep()
    {
        yield return new WaitForSeconds(1);


        cinemachineBrain.m_DefaultBlend.m_Time = 0;

        finalHand.SetActive(true);
        cam4_hand1.SetActive(true);

        yield return new WaitForSeconds(3);

        cam4_hand1.SetActive(false);
        cam4_hand2.SetActive(true);

        finalHand.GetComponent<Animator>().SetTrigger("fingereMove");

        yield return new WaitForSeconds(1);

        finalConfetti.SetActive(true);
    }
}
