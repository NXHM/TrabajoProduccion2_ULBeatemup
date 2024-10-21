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
    //public bool IsJumping = false;
    //private bool m_CanAttack = true;

    //[SerializeField]
    //private FloorMovement m_FloorMovement;

    //[SerializeField]
    //private Transform m_RaycastPoint;
    [SerializeField]
    private float m_RaycastDistance = 0.1f;

    //private Vector3 m_InitialPos;

    //private Rigidbody2D m_SpriteRb;
    private Vector2 m_Velocity = Vector2.zero;
    private AudioSource m_AudioSource;

    private Animator m_SpriteAnimator;

    private enum Actions
    {
        Walking, Jumping, Attacking
    }
    private Actions action;
    private Rigidbody2D rb;
    [SerializeField] private Rigidbody2D rbSalto;
    private Transform playerSprite;

    private void Awake()
    {
        //m_SpriteRb = transform.Find("PlayerSprite").GetComponent<Rigidbody2D>();
        playerSprite = transform.Find("PlayerSprite");
        m_SpriteAnimator = playerSprite.GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        //m_InitialPos = transform.position;
    }

    public void Move(float movX, float movY)
    {
        if (action != Actions.Attacking)
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
            }
            else if (Mathf.Abs(movY) > 0f)
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
        else
        {
            m_Velocity = Vector2.zero;
        }
    }

    private void Update()
    {
        //transform.position += (Vector3)(m_Velocity * Time.deltaTime);
        rb.velocity = m_Velocity;
        playerSprite.localPosition = rbSalto.position;
        KeepPlayerInBounds();
    }

    private void FixedUpdate()
    {
        if (action == Actions.Jumping && rbSalto.velocity.y < 0f)
        {
            m_SpriteAnimator.SetFloat("VelY", rbSalto.velocity.y);

            //if (IsGrounded())
            if(rbSalto.position.y<=0)
            {
                Debug.Log("DeactivateJump");
                DeactivateJump();
            }
        }

    }

    public void Jump()
    {
        if (action == Actions.Walking)
        {
            ActivateJump();
        }
    }

    public void Attack1()
    {
        if (action == Actions.Walking)
        {
            ActivateAttack1();
        }
    }

    public void ActivateJump()
    {
        Debug.Log("ActivateJump");
        //IsJumping = true;
        action = Actions.Jumping;
        m_SpriteAnimator.SetFloat("VelY", 0f);
        rbSalto.bodyType = RigidbodyType2D.Dynamic;
        rbSalto.AddForce(Vector2.up * m_JumpSpeed);
        m_SpriteAnimator.SetBool("Jump", true);
        m_AudioSource.Play();
    }

    public void DeactivateJump()
    {
        rbSalto.bodyType = RigidbodyType2D.Static;
        rbSalto.transform.position = Vector3.zero;
        //IsJumping = false;
        action = Actions.Walking;
        m_SpriteAnimator.SetBool("Jump", false);
        m_SpriteAnimator.SetFloat("VelY", 0f);
    }

    public void ActivateAttack1()
    {
        m_AudioSource.Play();
        m_SpriteAnimator.SetTrigger("Attack1");
        //m_CanAttack = false;
        action = Actions.Attacking;
    }

    public void DeactivateAttack1()
    {
        //m_CanAttack = true;
        action = Actions.Walking;
    }

    private void KeepPlayerInBounds()
    {
        Vector3 pos = transform.position;

        // Asegurarse de que el jugador no se salga del área de la cámara
        Vector3 minBounds = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 maxBounds = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        // Limitar la posición del jugador dentro de los límites de la cámara
        pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
        pos.y = Mathf.Clamp(pos.y, minBounds.y, maxBounds.y);

        transform.position = pos;
    }

    /*public bool IsGrounded()
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
    }*/

    /*public void OnDrawGizmos()
    {
        Gizmos.DrawRay(
            m_RaycastPoint.position,
            Vector3.down * m_RaycastDistance);
    }*/
}
