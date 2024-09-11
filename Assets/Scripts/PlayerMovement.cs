using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float m_SpeedX = 1f;
    [SerializeField]
    private float m_SpeedY = 1f;

    private Rigidbody2D m_Rb;
    private bool m_IsFacingRight = true;

    private void Awake() 
    {
        m_Rb = GetComponent<Rigidbody2D>();
    }

    public void Move(float movX, float movY)
    {
        if (movX < 0 && m_IsFacingRight)
        {
            transform.rotation *= Quaternion.Euler(0f, 180f, 0f);
            m_IsFacingRight = !m_IsFacingRight;
        }
        if (movX > 0 && !m_IsFacingRight)
        {
            transform.rotation *= Quaternion.Euler(0f, -180f, 0f); 
            m_IsFacingRight = !m_IsFacingRight;      
        }

        m_Rb.velocity = new Vector2(
            movX * m_SpeedX, 
            movY * m_SpeedY
        );
    }
}
