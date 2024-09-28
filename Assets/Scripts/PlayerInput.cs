using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerMovement m_PlayerMovement;
    private BeatemupControls m_BeatemupControls;
    private InputAction m_MoveInputAction;

    private void Awake() 
    {
        m_BeatemupControls = new BeatemupControls();
        m_PlayerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        m_BeatemupControls.Player.Jump.performed += DoJump;
        m_BeatemupControls.Player.Jump.Enable();

        m_BeatemupControls.Player.Attack1.performed += DoAttack1;
        m_BeatemupControls.Player.Attack1.Enable();

        m_MoveInputAction = m_BeatemupControls.Player.Move;
        m_MoveInputAction.Enable();
    }


    private void OnDisable()
    {
        m_BeatemupControls.Player.Jump.performed -= DoJump;
        m_BeatemupControls.Player.Jump.Disable();

        m_BeatemupControls.Player.Attack1.performed -= DoAttack1;
        m_BeatemupControls.Player.Attack1.Disable();

        m_MoveInputAction.Disable();
    }


    private void Update() 
    {
        var movVector = m_MoveInputAction.ReadValue<Vector2>();

        m_PlayerMovement.Move(movVector.x, movVector.y);
    }

    private void DoJump(InputAction.CallbackContext context)
    {
        m_PlayerMovement.Jump();
    }
    private void DoAttack1(InputAction.CallbackContext context)
    {
        m_PlayerMovement.Attack1();
    }
}
