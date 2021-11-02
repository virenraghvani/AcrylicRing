using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessoryStep : MonoBehaviour
{
    public bool isTargetFound, isPlaced;

    [SerializeField]
    private Transform startPos, targetPos;

    [SerializeField]
    private GameObject [] accessory;

    [SerializeField]
    private GameObject sparkle, particleParent;

    public GameManager gameManager;

    [SerializeField]
    private GameObject accessorySelectionPanel;

    private int selectedAccessoryIndex;

    void Awake()
    {
        GameManager.IS_READY_FOR_INPUT = true;
        InputManager.inst.OnDragCallback += Move;
    }

    private void OnEnable()
    {
        accessorySelectionPanel.SetActive(true);

        isTargetFound = false;
        isPlaced = false;
   
    }

    public void OnAccessorySelection(int index)
    {
        selectedAccessoryIndex = index;
        accessory[index].SetActive(true);
        accessory[index].transform.position = startPos.position;

        GameManager.IS_READY_FOR_INPUT = true;

    }

    void Move(Vector2 pos)
    {
        if (isTargetFound)
            return;

        isTargetFound = true;

        Debug.Log("move");
        //LeanTween.move(accessory, targetPos.transform.position, .3f);

    }

    private void LateUpdate()
    {
        if (isTargetFound && !isPlaced)
        {
            // Learp
            accessory[selectedAccessoryIndex].transform.position = Vector3.Lerp(accessory[selectedAccessoryIndex].transform.position, targetPos.transform.position, 10 * Time.deltaTime);

            if (Vector3.Distance(accessory[selectedAccessoryIndex].transform.position, targetPos.transform.position) < .01f)
            {
                isPlaced = true;
                //Invoke("DelayAccessoryPlaced", 2);

                Instantiate(sparkle, particleParent.transform);

                //vibration.TriggerHeavyImpact();
                //AudioController.instance.PlayAudio(audioClip);

                gameManager.OnAccessoryDone();
            }
        }
    }
}
