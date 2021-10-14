using System;
using System.Collections.Generic;
using UnityEngine;

public class BrushTipView : MonoBehaviour
{
    public List<Transform> tipTransforms;

    private Vector3     _currentRotationVector;
    private const float _rotationAmount       = 7f;
    private const float _dragToRotationFactor = 30f;

    private bool _isDragged = false;

    private void Awake()
    {
        InputManager.MouseDragStarted += OnDragStarted;
        InputManager.MouseDragEnded   += OnDragEnded;
        InputManager.MouseDragged     += OnPowderDragged;
    }

    private void OnDestroy()
    {
        InputManager.MouseDragStarted -= OnDragStarted;
        InputManager.MouseDragEnded   -= OnDragEnded;
        InputManager.MouseDragged     -= OnPowderDragged;
    }

    private void OnPowderDragged( Vector2 dragVector )
    {
        Bend( dragVector );
    }

    private void Update()
    {
        HandleBending();

        if ( !_isDragged )
            HandleDaping();
    }

    public void Bend( Vector2 dragVector )
    {
        _currentRotationVector = new Vector3( 0f, Mathf.Clamp( _currentRotationVector.y + dragVector.y * -_dragToRotationFactor, -1f, 1f ), Mathf.Clamp( _currentRotationVector.z + dragVector.x * -_dragToRotationFactor, -1f, 1f ) );
    }

    private void HandleBending()
    {
        foreach ( var tipTransform in tipTransforms ) {
            tipTransform.localRotation = Quaternion.Euler( _currentRotationVector * _rotationAmount );
        }
    }

    private void HandleDaping()
    {
        _currentRotationVector = _currentRotationVector * ( 1 - Time.deltaTime );
    }

    private void OnDragStarted( Vector2 dragPosition )
    {
        _isDragged = true;
    }

    private void OnDragEnded( Vector2 dragPosition )
    {
        _isDragged = false;
    }
}
