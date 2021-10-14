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

    public Animator brushAnimator, ringHolderAnimator, bottleAnimator, sandingMachineAnimator;

    public RingBlendshapes ringBlendshapes;

    [SerializeField]
    private GameObject sandParticles;

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
