using UnityEngine;

public enum EnemyState
{
    Idle,         // El enemigo está esperando
    Chasing,      // El enemigo está persiguiendo al jugador
    AttackMelee,  // El enemigo está atacando cuerpo a cuerpo
    AttackRange   // El enemigo está atacando a distancia
}

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float m_RaycastDistance = 16f;
    [SerializeField]
    private float m_AttackRangeDistance = 16f;
    [SerializeField]
    private float m_ChaseDistance = 8f;
    [SerializeField]
    private float m_AttackMeleeDistance = 4f;
    [SerializeField]
    private float m_Speed = 4f;
    [SerializeField]
    private Transform m_RaycastGenerator;

    private EnemyState m_State = EnemyState.Idle; // Estado actual del enemigo
    public Transform m_Player = null;

    private EnemyAttack enemyAttack; // Referencia al componente EnemyAttack

    private void Awake()
    {
        enemyAttack = GetComponent<EnemyAttack>(); // Asigna EnemyAttack para manejar los ataques
    }

    private void Update()
    {
        float distance = GetPlayerDistance();
        if (distance >= 0f) // Solo si se ha detectado un jugador
        {
            // Decidimos si perseguir o atacar
            AttackOrChase(distance);
        }
        else
        {
            SetState(EnemyState.Idle); // Si no hay jugador, el enemigo se queda en Idle
        }

        // Gestiona los diferentes estados del enemigo
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
                OnAttackRange();
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
        transform.position += m_Speed * Time.deltaTime * dir; // Persigue al jugador
    }

    private void OnAttackMelee()
    {
        enemyAttack.TriggerMeleeAttack();  // Desencadena el ataque cuerpo a cuerpo en EnemyAttack
    }

    private void OnAttackRange()
    {
        Debug.Log("OnAttackRange ejecutado");
        enemyAttack.TriggerRangeAttack();  // Desencadena el ataque a distancia en EnemyAttack
    }

    private void AttackOrChase(float distance)
    {
        if (distance < m_AttackMeleeDistance)
        {
            Debug.Log("ATACA cuerpo a cuerpo");
            SetState(EnemyState.AttackMelee);  // Cambia el estado para cuerpo a cuerpo
        }
        else if (distance <= m_ChaseDistance)
        {
            SetState(EnemyState.Chasing);  // Persigue al jugador
        }
        else if (distance <= m_AttackRangeDistance)
        {
            Debug.Log("ATACA a distancia");
            SetState(EnemyState.AttackRange);  // Cambia el estado para ataque a distancia
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

    public void SetState(EnemyState state)
    {
        m_State = state;
    }

    public EnemyState GetState()
    {
        return m_State;
    }

}
