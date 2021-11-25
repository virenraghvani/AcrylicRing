using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public bool onDryingMachine_bool, onAccessory_bool, onGlitterPaint_bool;

    private int blobNo;

    [SerializeField]
    private GameObject brush, nailPolishBottle, dottedLine, sandingMachine, accessoryStep, finalHand, finalConfetti, levelStartPanel, gameOverPanel, powderSelectionPanel;

    [HideInInspector]
    public MeshRenderer ringFinalOutput;

    [SerializeField]
    private Material matShine, matRough, matFinalHand, matTinSides, matTinBorder, matTinSurface;

    [SerializeField]
    private Texture normalMap;

    public MoveTool moveTool;

    public RotateCyclinder rotateCyclinder;

    public Animator brushAnimator, ringHolderAnimator, powderTinAnimator, dryingMachineAnimator;

    [SerializeField]
    private GameObject sandParticles;

    private bool isAutoCompleting;

    [SerializeField]
    private GameObject cam1, cam2_dryer, cam3, cam4_hand, cam4_hand1, cam4_hand2;

    public static bool IS_READY_FOR_INPUT;

    public bool isSandingUpperPart;

    [SerializeField]
    private CinemachineBrain cinemachineBrain;

    [SerializeField]
    private GameObject pf_ring;

    [SerializeField]
    private Transform RingParent;

    public GameObject currentRing;

    public Transform toolRayCast, marker;

    public int currentLevel;

    //[SerializeField]
    //private Image img_customerRequest;

    //[SerializeField]
    //private Sprite[] customerRequests;

    [SerializeField]
    private GameObject [] endMessages;

    public Text levelNo_text;

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
        //  SpawnRing();
        //powderSelectionPanel.SetActive(true);
        
        levelNo_text.text = "Level " + PlayerPrefs.GetInt("currLevel_pp");
        currentLevel = PlayerPrefs.GetInt("level", 0);

        //img_customerRequest.sprite = customerRequests[currentLevel % 5];
    }

    void SpawnRing()
    {
    

        blobNo = 0;
        isAutoCompleting = false;
        currentRing = Instantiate(pf_ring, RingParent);
        currentRing.name = "Ring";

        ringFinalOutput = currentRing.GetComponent<RingData>().ringMesh;

        powderTinAnimator.gameObject.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //SceneManager.LoadScene(0);
            ScreenCapture.CaptureScreenshot("icon.png");
        }
        
        if (Input.GetKeyDown(KeyCode.Tab))
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

        //RaycastHit hit;
        //int layerMask = 1 << 8;
        //layerMask = ~layerMask;

        //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        //{
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        //    Debug.Log("Did Hit");

        //    transform.position = hit.point;
        //}
        //else
        //{
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        //    Debug.Log("Did not Hit");
        //}
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
        currentRing.GetComponent<RingData>().blob[blobNo - 1].GetComponent<ManipulateNailBlob>().enabled = false;
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
        currentRing.GetComponent<RingData>().blob[blobNo].SetActive(true);

        dottedLine.SetActive(true);
        IS_READY_FOR_INPUT = true;
    }

    IEnumerator StartDrying() {

        endMessages[Random.Range(0, 6)].SetActive(true);

        cam1.SetActive(false);
        cam2_dryer.SetActive(true);

        ringHolderAnimator.SetTrigger("bottleOut");
        brushAnimator.SetTrigger("moveBrushOut");
        powderTinAnimator.SetTrigger("moveBottleOut");

        yield return new WaitForSeconds(.12f);

        yield return new WaitForSeconds(1);

        brush.SetActive(false);

        if(onDryingMachine_bool)
            dryingMachineAnimator.gameObject.SetActive(true);

        powderTinAnimator.gameObject.SetActive(false);

        yield return new WaitForSeconds(.1f);
        
        ringHolderAnimator.SetTrigger("dryIn");

        if (onDryingMachine_bool)
            yield return new WaitForSeconds(2);
        else
            yield return new WaitForSeconds(0.1f);

        currentRing.GetComponent<RingData>().ringFinal.SetActive(true);
        currentRing.GetComponent<RingData>().ringParts.SetActive(false);

        if (onDryingMachine_bool)
            yield return new WaitForSeconds(3);
        else
            yield return new WaitForSeconds(1);


        currentRing.GetComponent<RingData>().ringMesh.material = matRough;

        if (onDryingMachine_bool)
        {
            dryingMachineAnimator.SetTrigger("out");

            yield return new WaitForSeconds(1);
        }
        
        ringHolderAnimator.SetTrigger("dryOut");
        
        yield return new WaitForSeconds(1);

        if (onDryingMachine_bool)
        {
            dryingMachineAnimator.gameObject.SetActive(false);
        }

        nailPolishBottle.SetActive(false);

        cam2_dryer.SetActive(false);
        cam3.SetActive(true);

        sandingMachine.SetActive(true);
        IS_READY_FOR_INPUT = true;

     //   ringHolderAnimator.enabled = false;
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
        endMessages[Random.Range(0, 6)].SetActive(true);

        ringHolderAnimator.enabled = true;
        ringHolderAnimator.SetTrigger("accessoryPos");

        sandingMachine.SetActive(false);

        if(onAccessory_bool)
            Invoke("DelayAccessoryStep", 1);
        else
            StartCoroutine(StartHandStep());
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
        cam3.SetActive(false);

        yield return new WaitForSeconds(3);

        cam4_hand1.SetActive(false);
        cam4_hand2.SetActive(true);

        finalHand.GetComponent<Animator>().SetTrigger("fingereMove");

        yield return new WaitForSeconds(1);

        finalConfetti.SetActive(true);

        gameOverPanel.SetActive(true);

    }

    public void NextLevel()
    {
        if(PlayerPrefs.GetInt("loopCurrLevel_pp") >= 8)
        {
            PlayerPrefs.SetInt("loopCurrLevel_pp", 0);   
        }

        PlayerPrefs.SetInt("loopCurrLevel_pp", PlayerPrefs.GetInt("loopCurrLevel_pp") + 1);
        PlayerPrefs.SetInt("currLevel_pp", PlayerPrefs.GetInt("currLevel_pp") + 1);


        finalHand.SetActive(false);

        cam4_hand2.SetActive(false);
        cam1.SetActive(true);

        //powderSelectionPanel.SetActive(true);

        gameOverPanel.SetActive(false);
        accessoryStep.SetActive(false);
        //accessoryStep.GetComponent<AccessoryStep>().OnAccessoryPanel();

        ringHolderAnimator.transform.GetChild(0).localRotation = Quaternion.identity;
        ringHolderAnimator.enabled = true;
        ringHolderAnimator.SetTrigger("reset");
        //   brushAnimator.SetTrigger("moveBrushIn");

        nailPolishBottle.SetActive(true);

        cinemachineBrain.m_DefaultBlend.m_Time = 1;
        Destroy(currentRing);

        currentLevel++;
        PlayerPrefs.SetInt("level", currentLevel);
        //img_customerRequest.sprite = customerRequests[currentLevel % 5];

        Invoke("OnLevelStartPanel", 0.5f);
    }

    void OnLevelStartPanel()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("loopCurrLevel_pp"));
        //levelStartPanel.SetActive(true);
    }

    public void OnPowderSelection(int index)
    {
        Material mat = new Material(Resources.Load("Prefabs/PowderColor/" + index, typeof(Material)) as Material);
        ChangeAllMaterials(mat);
        SpawnRing();
    }

    public void OnAccessorySelection(int index)
    {

    }


    void ChangeAllMaterials(Material mat)
    {
        matRough.CopyPropertiesFromMaterial(mat);
        matShine.CopyPropertiesFromMaterial(mat);
        matFinalHand.CopyPropertiesFromMaterial(mat);

        matRough.SetFloat("_Glossiness", 0);
        matFinalHand.SetFloat("_Glossiness", 0.76f);


        matTinBorder.CopyPropertiesFromMaterial(mat);
        matTinBorder.SetFloat("_Glossiness", 0.76f);
        
        matTinSides.CopyPropertiesFromMaterial(mat);
        matTinSides.SetFloat("_Glossiness", 0.76f);

        matTinSurface.CopyPropertiesFromMaterial(mat);
        matTinSurface.SetFloat("_Glossiness", 0.55f);
        matTinSurface.SetTexture("_BumpMap", normalMap);

        if (onGlitterPaint_bool)
        {
            matRough.SetFloat("_BumpScale", 0.55f);
            matShine.SetFloat("_BumpScale", 0.55f);
            matTinBorder.SetFloat("_BumpScale", 0.55f);
            matFinalHand.SetFloat("_BumpScale", 0.55f);
            matTinSides.SetFloat("_BumpScale", 0.55f);
            matTinSurface.SetFloat("_BumpScale", 0.55f);
        }
        else
        {
            matRough.SetFloat("_BumpScale", 0);
            matShine.SetFloat("_BumpScale", 0);
            matTinBorder.SetFloat("_BumpScale", 0);
            matFinalHand.SetFloat("_BumpScale", 0);
            matTinSides.SetFloat("_BumpScale", 0);
            matTinSurface.SetFloat("_BumpScale", 0);
        }
    }


}
