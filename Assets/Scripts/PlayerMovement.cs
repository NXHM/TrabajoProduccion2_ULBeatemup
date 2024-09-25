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
    private float m_JumpSpeed = 350f;
    public bool IsJumping = false;

    [SerializeField]
    private FloorMovement m_FloorMovement;

    private Vector3 m_InitialPos;

    private Rigidbody2D m_SpriteRb;
    private Vector2 m_Velocity = Vector2.zero;
    private AudioSource m_AudioSource;

    private void Awake() 
    {
        m_SpriteRb = transform.Find("Sprite").GetComponent<Rigidbody2D>();
        m_AudioSource = GetComponent<AudioSource>();
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

        m_Velocity = new Vector2(
            movX * m_SpeedX, 
            movY * m_SpeedY
        );
    }

    private void Update() 
    {
        transform.position += (Vector3)(m_Velocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (!IsJumping)
        {
            IsJumping = true;
            m_SpriteRb.AddForce(Vector2.up * m_JumpSpeed);
            m_AudioSource.Play();
        }
    }
}
