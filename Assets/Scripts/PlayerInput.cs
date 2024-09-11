using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerMovement m_PlayerMovement;

    private void Awake() 
    {
        m_PlayerMovement = GetComponent<PlayerMovement>();
    }

    private void Update() 
    {
        float movX = Input.GetAxis("Horizontal");
        float movY = Input.GetAxis("Vertical");

        m_PlayerMovement.Move(movX, movY);
    }
}
