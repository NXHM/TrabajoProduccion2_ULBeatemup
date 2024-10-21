using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle, Chasing, AttackMelee, AttackRange
}

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float m_RaycastDistance = 8f;
    [SerializeField]
    private float m_AttackRangeDistance = 8f;
    [SerializeField]
    private float m_ChaseDistance = 4f;
    [SerializeField]
    private float m_AttackMeleeDistance = 1f;
    [SerializeField]
    private float m_Speed = 4f;
    [SerializeField]
    private Transform m_RaycastGenerator;
    private EnemyState m_State = EnemyState.Idle;
    private Animator m_SpriteAnimator;
    private bool m_IsTalking = false;
    public Transform m_Player = null;

    private void Awake()
    {
        m_SpriteAnimator = transform.Find("Sprite").GetComponent<Animator>();
    }

    private void Update()
    {
        float distance = GetPlayerDistance();
        if (distance >= 0f) // Solo si se ha detectado un jugador
        {
            AttackOrChase(distance);
        }
        else
        {
            m_State = EnemyState.Idle; // No se detectó jugador, el enemigo se queda en Idle
        }

        // Cambiar el estado del enemigo según corresponda
        switch (m_State)
        {
            case EnemyState.Idle:
                OnIdle();
                break;
            case EnemyState.Chasing:
                OnChase();
                break;
            case EnemyState.AttackMelee:
                OnAttackMelee();
                break;
            case EnemyState.AttackRange:
                OnAttackRange(distance); 
                break;
        }

    }

    private void OnIdle()
    {
        // Comportamiento cuando el enemigo está en estado Idle (por ejemplo, animación o patrullaje)
    }

    private void OnChase()
    {
        if (m_Player == null) return;

        Vector3 dir = (m_Player.position - transform.position).normalized;
        transform.position += m_Speed * Time.deltaTime * dir;
    }

    private void OnAttackMelee()
    {
        m_SpriteAnimator.SetTrigger("MeleeAttack");
    }

    private void OnAttackRange(float distance)
    {
        m_SpriteAnimator.SetTrigger("RangeAttack");

        // Disparar proyectil al atacar a distancia, pasando la distancia como argumento
        GetComponent<EnemyAttack>()?.FireProjectile(distance);
    }


    private void AttackOrChase(float distance)
    {
        if (distance < m_AttackMeleeDistance)
        {
            m_State = EnemyState.AttackMelee;
        }
        else if (distance <= m_ChaseDistance)
        {
            m_State = EnemyState.Chasing;
        }
        else if (distance <= m_AttackRangeDistance)
        {
            m_State = EnemyState.AttackRange;
        }
    }

    public float GetPlayerDistance()
    {
        // Realiza un raycast hacia la izquierda
        var hit = Physics2D.Raycast(
            m_RaycastGenerator.position, Vector2.left,
            m_RaycastDistance, LayerMask.GetMask("Hitbox")
        );

        if (hit.collider != null)
        {
            m_Player = hit.collider.transform;
            return Vector3.Distance(m_Player.position, transform.position);
        }

        // Si no se encontró a un jugador hacia la izquierda, realiza un raycast hacia la derecha
        hit = Physics2D.Raycast(
            m_RaycastGenerator.position, Vector2.right,
            m_RaycastDistance, LayerMask.GetMask("Hitbox")
        );

        if (hit.collider != null)
        {
            m_Player = hit.collider.transform;
            return Vector3.Distance(m_Player.position, transform.position);
        }

        // Si no se encuentra ningún jugador, retorna -1
        m_Player = null;
        return -1f; // Indica que no se encontró al jugador
    }

    public void TriggerMeleeAttack()
    {
        m_SpriteAnimator.SetTrigger("MeleeAttack");
    }

    public void TriggerRangeAttack()
    {
        m_SpriteAnimator.SetTrigger("RangeAttack");
    }

    public void SetState(EnemyState state)
    {
        m_State = state;
    }

    public void Talk()
    {
        if (!m_IsTalking)
        {
            m_SpriteAnimator.SetTrigger("Talk");
            m_IsTalking = true;
        }
        else
        {
            m_SpriteAnimator.SetTrigger("StopTalk");
            m_IsTalking = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(m_RaycastGenerator.position, Vector2.left * m_RaycastDistance);
        Gizmos.DrawRay(m_RaycastGenerator.position, Vector2.right * m_RaycastDistance);
    }
}
