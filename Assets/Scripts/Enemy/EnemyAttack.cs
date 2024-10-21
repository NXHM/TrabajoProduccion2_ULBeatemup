using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private EnemyMovement enemyMovement;

    [SerializeField]
    private float shootDistance = 8f; // Distancia para disparar
    [SerializeField]
    private float chaseDistance = 4f; // Distancia para perseguir
    [SerializeField]
    private float meleeDistance = 1f; // Distancia para ataque cuerpo a cuerpo
    [SerializeField]
    private float projectileSpeed = 0.1f; // Velocidad del proyectil
    [SerializeField]
    private float attackCooldown = 2f; // Tiempo de cooldown entre ataques de rango
    private float lastAttackTime = -Mathf.Infinity; // Registro del último ataque
    private ProjectilePoolManager projectilePoolManager; // Referencia al Pool Manager

    // Daños de los ataques
    [SerializeField]
    private float meleeDamage = 1f; // Daño del ataque melee
    [SerializeField]
    private float rangeDamageMax = 1.5f; // Daño máximo del ataque de rango (cuando está cerca)
    [SerializeField]
    private float rangeDamageMin = 0.5f; // Daño mínimo del ataque de rango (cuando está lejos)

    private void Awake()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        projectilePoolManager = FindObjectOfType<ProjectilePoolManager>();
    }

    private void Update()
    {
        if (enemyMovement == null) return;

        float distance = enemyMovement.GetPlayerDistance();
        if (distance < 0) return; // No hay jugador detectado

        HandleAttack(distance);
    }

    private void HandleAttack(float distance)
    {
        if (distance < meleeDistance)
        {
            PerformMeleeAttack();
        }
        else if (distance <= chaseDistance)
        {
            ChasePlayer();
        }
        else if (distance <= shootDistance && Time.time >= lastAttackTime + attackCooldown)
        {
            PerformRangeAttack(distance);
        }
    }

    private void PerformMeleeAttack()
    {
        Debug.Log("Realizando ataque cuerpo a cuerpo");
        enemyMovement.TriggerMeleeAttack();
        ApplyMeleeDamage();
    }

    private void ChasePlayer()
    {
        Debug.Log("Persiguiendo al jugador...");
        enemyMovement.SetState(EnemyState.Chasing);
    }

    private void PerformRangeAttack(float distance)
    {
        Debug.Log("Disparando proyectil");
        enemyMovement.TriggerRangeAttack();
        FireProjectile(distance);
        lastAttackTime = Time.time; // Actualiza el tiempo del último ataque
    }

    private void ApplyMeleeDamage()
    {
        if (enemyMovement.m_Player != null)
        {
            // Aplica el daño de melee al jugador
            PlayerHealth playerHealth = enemyMovement.m_Player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(meleeDamage);
                Debug.Log($"El jugador recibió {meleeDamage} de daño por ataque cuerpo a cuerpo.");
            }
        }
    }

    public void FireProjectile(float distance)
    {
        // Asegúrate de que el Pool Manager no sea nulo
        if (projectilePoolManager != null && enemyMovement.m_Player != null)
        {
            GameObject projectile = projectilePoolManager.GetProjectile();
            if (projectile != null)
            {
                // Establece la posición inicial del proyectil
                projectile.transform.position = transform.position + new Vector3(0.5f, 1.5f, 0); // Ajustar si es necesario

                // Establece el objetivo del proyectil
                Shuriken projectileScript = projectile.GetComponent<Shuriken>();
                if (projectileScript != null)
                {
                    projectileScript.SetTarget(enemyMovement.m_Player); // Asigna el jugador como objetivo
                    projectileScript.SetDamage(distance, rangeDamageMax, rangeDamageMin, shootDistance); // Asigna el daño
                }

                // Establece la rotación inicial del proyectil
                projectile.transform.rotation = Quaternion.identity;

                Debug.Log("Proyectil disparado hacia el jugador.");
            }
            else
            {
                Debug.LogWarning("No se pudo obtener un proyectil del pool.");
            }
        }
        else
        {
            Debug.LogWarning("Projectile Pool Manager no encontrado o jugador no asignado.");
        }
    }
}
