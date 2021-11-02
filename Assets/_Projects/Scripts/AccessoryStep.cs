using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessoryStep : MonoBehaviour
{
    public bool isTargetFound, isPlaced;

    [SerializeField]
    private Transform startPos, targetPos;

    [SerializeField]
    private GameObject accessory;

    [SerializeField]
    private GameObject sparkle, particleParent;

    public GameManager gameManager;

    void Awake()
    {
        GameManager.IS_READY_FOR_INPUT = true;
        InputManager.inst.OnDragCallback += Move;
    }

    private void OnEnable()
    {
        GameManager.IS_READY_FOR_INPUT = true;

        isTargetFound = false;
        isPlaced = false;
        accessory.transform.position = startPos.position;
   
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
            accessory.transform.position = Vector3.Lerp(accessory.transform.position, targetPos.transform.position, 10 * Time.deltaTime);

            if (Vector3.Distance(accessory.transform.position, targetPos.transform.position) < .01f)
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
