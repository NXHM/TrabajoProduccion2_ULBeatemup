using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    private bool m_CanAttack = true;

    [SerializeField]
    private FloorMovement m_FloorMovement;

    [SerializeField]
    private Transform m_RaycastPoint;
    [SerializeField]
    private float m_RaycastDistance = 0.1f;

    private Vector3 m_InitialPos;

    private Rigidbody2D m_SpriteRb;
    private Vector2 m_Velocity = Vector2.zero;
    private AudioSource m_AudioSource;

    private Animator m_SpriteAnimator;

    private void Awake()
    {
        m_SpriteRb = transform.Find("Sprite").GetComponent<Rigidbody2D>();
        m_SpriteAnimator = transform.Find("Sprite").GetComponent<Animator>();
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

        if (Mathf.Abs(movX) > 0f)
        {
            m_SpriteAnimator.SetFloat("MovX", Mathf.Abs(movX));
            m_SpriteAnimator.SetBool("Stop", false);
        } else if (Mathf.Abs(movY) > 0f)
        {
            m_SpriteAnimator.SetFloat("MovX", Mathf.Abs(movY));
            m_SpriteAnimator.SetBool("Stop", false);
        }
        else
        {
            m_SpriteAnimator.SetBool("Stop", true);
            m_SpriteAnimator.SetFloat("MovX", 0f);
        }

    }

    private void Update()
    {
        transform.position += (Vector3)(m_Velocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (IsJumping && m_SpriteRb.velocity.y < 0f)
        {
            m_SpriteAnimator.SetFloat("VelY", m_SpriteRb.velocity.y);

            if (IsGrounded())
            {
                Debug.Log("DeactivateJump");
                DeactivateJump();
            }
        }

    }

    public void Jump()
    {
        if (!IsJumping)
        {
            ActivateJump();
        }
    }

    public void Attack1()
    {
        if (m_CanAttack && !IsJumping)
        {
            ActivateAttack1();
        }
    }

    public void ActivateJump()
    {
        Debug.Log("ActivateJump");
        IsJumping = true;
        m_SpriteAnimator.SetFloat("VelY", 0f);
        m_SpriteRb.AddForce(Vector2.up * m_JumpSpeed);
        m_SpriteAnimator.SetBool("Jump", true);
        m_AudioSource.Play();
    }

    public void DeactivateJump()
    {
        IsJumping = false;
        m_SpriteAnimator.SetBool("Jump", false);
        m_SpriteAnimator.SetFloat("VelY", 0f);
    }

    public void ActivateAttack1()
    {
        m_AudioSource.Play();
        m_SpriteAnimator.SetTrigger("Attack1");
        m_CanAttack = false;
    }

    public void DeactivateAttack1()
    {
        m_CanAttack = true;
    }

    public bool IsGrounded()
    {
        var hit = Physics2D.Raycast(
            m_RaycastPoint.position,
            Vector2.down,
            m_RaycastDistance,
            LayerMask.GetMask("Floor"));

        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawRay(
            m_RaycastPoint.position,
            Vector3.down * m_RaycastDistance);
    }
}
