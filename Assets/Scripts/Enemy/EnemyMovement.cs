using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle, Chasing, Attacking
}

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float m_RaycastDistance = 3f;
    [SerializeField]
    private float m_AttackDistance = 0.5f;
    [SerializeField]
    private float m_Speed = 4f;
    [SerializeField]
    private Transform m_RaycastGenerator;
    [SerializeField]
    private GameObject shurikenPrefab;

    private float minAttackCooldown = 2f; // Tiempo mínimo entre lanzamientos
    private float maxAttackCooldown = 5f; // Tiempo máximo entre lanzamientos
    private float cooldownTimer;

    private EnemyState m_State = EnemyState.Idle;
    private Transform m_Player = null;

    private void Awake()
    {
        SetRandomCooldown(); // Establecer un cooldown inicial aleatorio
    }

    private void Update()
    {
        float distance = GetPlayerDistance();
        if (distance > 0f)
        {
            AttackorChase(distance);
        }
        else
        {
            m_State = EnemyState.Idle;
        }

        switch (m_State)
        {
            case EnemyState.Idle:
                OnIdle();
                break;
            case EnemyState.Chasing:
                OnChase();
                break;
            case EnemyState.Attacking:
                // Aquí no se hace nada, ya que solo lanzaremos el shuriken
                break;
        }

        // Manejar el temporizador para lanzar el Shuriken
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // Lanzar el Shuriken cuando el temporizador llegue a cero
        if (cooldownTimer <= 0)
        {
            LaunchShuriken(); // Lanzar el Shuriken
            SetRandomCooldown(); // Establecer un nuevo cooldown aleatorio
        }
    }

    private void LaunchShuriken()
    {
        if (shurikenPrefab == null)
        {
            Debug.LogError("Shuriken prefab is not assigned!");
            return;
        }

        if (m_Player == null) return; // Asegúrate de que hay un jugador al que apuntar

        Vector3 spawnPosition = transform.position; // Salir del cuerpo del enemigo
        GameObject shuriken = Instantiate(shurikenPrefab, spawnPosition, Quaternion.identity);

        if (shuriken != null)
        {
            Vector2 direction = (m_Player.position - transform.position).normalized; // Apuntar hacia el jugador
            shuriken.GetComponent<Shuriken>().Initialize(spawnPosition, direction);
            Debug.Log($"Launching Shuriken from {spawnPosition} towards {m_Player.position}");
        }
    }

    private void SetRandomCooldown()
    {
        cooldownTimer = UnityEngine.Random.Range(minAttackCooldown, maxAttackCooldown);
    }

    private void OnIdle() { }

    private void OnChase()
    {
        if (m_Player == null) return;

        Vector3 dir = (m_Player.position - transform.position).normalized;
        transform.position += m_Speed * Time.deltaTime * dir;
    }

    private void AttackorChase(float distance)
    {
        if (distance < m_AttackDistance)
        {
            m_State = EnemyState.Attacking;
        }
        else
        {
            m_State = EnemyState.Chasing;
        }
    }

    private float GetPlayerDistance()
    {
        // Lanzas el Raycast
        var hit = Physics2D.Raycast(
            m_RaycastGenerator.position,
            Vector2.left,
            m_RaycastDistance,
            LayerMask.GetMask("Hitbox")
        );

        if (hit.collider != null)
        {
            m_Player = hit.collider.transform;
            return Vector3.Distance(m_Player.position, transform.position);
        }

        hit = Physics2D.Raycast(
            m_RaycastGenerator.position,
            Vector2.right,
            m_RaycastDistance,
            LayerMask.GetMask("Hitbox")
        );

        if (hit.collider != null)
        {
            m_Player = hit.collider.transform;
            return Vector3.Distance(m_Player.position, transform.position);
        }

        m_Player = null;
        return -1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(m_RaycastGenerator.position, Vector2.left * m_RaycastDistance);
        Gizmos.DrawRay(m_RaycastGenerator.position, Vector2.right * m_RaycastDistance);
    }
}
