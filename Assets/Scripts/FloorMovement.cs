using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMovement : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement m_PlayerMovement;

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.collider.CompareTag("Player"))
        {
            m_PlayerMovement.DeactivateJump();
        }
    }
}
