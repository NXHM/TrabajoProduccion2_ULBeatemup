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

    [SerializeField]
    private float m_Gravity = 0.5f;
    [SerializeField]
    private float m_JumpSpeed = 350f;
    public bool IsJumping = false;

    [SerializeField]
    private FloorMovement m_FloorMovement;

    private Vector3 m_InitialPos;

    private void Awake() 
    {
        m_Rb = GetComponent<Rigidbody2D>();
    }

    private void Start() 
    {
        m_InitialPos = transform.position;
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
            !IsJumping ? movY * m_SpeedY : m_Rb.velocity.y
        );
    }

    private void Update() 
    {
        if (IsJumping)
        {
            m_Rb.velocity += Vector2.down * (m_Gravity * Time.deltaTime);
            Vector3 deltaMov = transform.position - m_InitialPos;
            m_FloorMovement.Move(deltaMov.x, 0f);
        }else
        {
            Vector3 deltaMov = transform.position - m_InitialPos;
            Debug.Log(deltaMov.y);
            if (deltaMov.magnitude == 0f) deltaMov.y = 0f;
            m_FloorMovement.Move(deltaMov.x, deltaMov.y);
        } 
        m_InitialPos = transform.position;
    }

    public void Jump()
    {
        if (!IsJumping)
        {
            IsJumping = true;
            m_Rb.AddForce(Vector2.up * m_JumpSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        IsJumping = false;
        m_Rb.velocity = Vector2.zero;
    }
}
