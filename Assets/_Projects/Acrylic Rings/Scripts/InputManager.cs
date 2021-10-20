using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager inst;

    private Vector2 _lastMousePos;
    private Vector2 _startMousePos;

    public static Action<Vector2> MouseDragStarted = delegate { };
    public static Action<Vector2> MouseDragged = delegate { };
    public static Action<Vector2> MouseDragEnded = delegate { };

    public delegate void OnDrag(Vector2 currentPos);
    public delegate void OnClick(Vector2 startPos);
    public delegate void OnClickEnd(Vector2 endPos);

    public OnDrag     OnDragCallback;
    public OnClick    OnClickCallback;
    public OnClickEnd OnClickEndCallback;

    private void Awake()
    {
        #region Singelton
        if (inst == null)
        {
            inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("More then one InputManager was created, destroying duplicates");
            Destroy(this);
        }
        #endregion
    }

    void Update()
    {
        if (!GameManager.IS_READY_FOR_INPUT)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            _lastMousePos  = Input.mousePosition;
            _startMousePos = _lastMousePos;

            OnClickCallback.Invoke(_startMousePos);

            MouseDragStarted.Invoke(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            OnDragCallback.Invoke(Input.mousePosition);
            MouseDragged.Invoke((_lastMousePos - new Vector2(Input.mousePosition.x, Input.mousePosition.y)) / Screen.height);

            _lastMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnClickEndCallback.Invoke(Input.mousePosition);
            _lastMousePos = Input.mousePosition;

            MouseDragEnded.Invoke(Input.mousePosition);
        }
    }
}
