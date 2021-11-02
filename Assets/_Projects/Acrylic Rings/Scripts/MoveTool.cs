using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTool : MonoBehaviour
{
    [SerializeField] private float _sensitivity;
    private Vector3 _startPos;
    private Vector3 _lastPos;
    private float   _camDist;

    public bool moveToDefaultPos;

    public Vector3 startPos;

    private void Awake()
    {

        InputManager.inst.OnClickCallback += StartMove;
        InputManager.inst.OnDragCallback  += Move;

        _camDist = Vector3.Distance(Camera.main.transform.position, transform.position);

        startPos = transform.localPosition;
    }

    private void OnEnable()
    {
        moveToDefaultPos = true;
    }
    private void StartMove(Vector2 startPos)
    {
        _lastPos = Camera.main.ScreenToWorldPoint(new Vector3(startPos.x, startPos.y, _camDist));
    }

    private void Move(Vector2 currentScreenPos)
    {
        Vector3 currentPos = Camera.main.ScreenToWorldPoint(new Vector3(currentScreenPos.x, currentScreenPos.y, _camDist));
        Vector3 delta = currentPos - _lastPos;
        delta = new Vector3(delta.x, 0f, -delta.y);
        _lastPos = currentPos;

        transform.position += delta * _sensitivity;
    }

    private void LateUpdate()
    {
        if (moveToDefaultPos) {
            //transform.localPosition = startPos;
            moveToDefaultPos = false;

            LeanTween.moveLocal(gameObject, startPos, .5f);
        }

    }
}
