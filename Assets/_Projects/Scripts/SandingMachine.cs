using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandingMachine : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    void Start()
    {
        InputManager.inst.OnClickCallback += StartMove;
        InputManager.inst.OnDragCallback += Move;
        InputManager.inst.OnClickEndCallback += EndMove;

    }

    void StartMove(Vector2 startPos)
    {
        animator.SetBool("sandingIn", true);
    }

    private void Move(Vector2 currentPos)
    {
       
    }

    private void EndMove(Vector2 currentPos)
    {
        animator.SetBool("sandingIn", false);
    }

    public void SandingDone()
    {
        animator.SetBool("sandingIn", false);
    }
}
