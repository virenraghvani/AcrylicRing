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
    private GameObject particleEffect2;

    void Awake()
    {
        GameManager.IS_READY_FOR_INPUT = true;
        InputManager.inst.OnDragCallback += Move;
    }

    void Move(Vector2 pos)
    {
        if (isTargetFound)
            return;

        isTargetFound = true;

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
                particleEffect2.SetActive(true);

                //vibration.TriggerHeavyImpact();
                //AudioController.instance.PlayAudio(audioClip);
            }
        }
    }
}
