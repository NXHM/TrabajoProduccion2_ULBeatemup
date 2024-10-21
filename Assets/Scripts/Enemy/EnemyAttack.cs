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
    private float lastAttackTime = -Mathf.Infinity; // Registro del �ltimo ataque
    private ProjectilePoolManager projectilePoolManager; // Referencia al Pool Manager

    // Da�os de los ataques
    [SerializeField]
    private float meleeDamage = 0.10f; // Da�o del ataque melee
    [SerializeField]
    private float rangeDamageMax = 0.10f; // Da�o m�ximo del ataque de rango (cuando est� cerca)
    [SerializeField]
    private float rangeDamageMin = 0.05f; // Da�o m�nimo del ataque de rango (cuando est� lejos)

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
        lastAttackTime = Time.time; // Actualiza el tiempo del �ltimo ataque
    }

    private void ApplyMeleeDamage()
    {
        if (enemyMovement.m_Player != null)
        {
            GameObject player = GameObject.FindWithTag("PlayerHealthBar");
            // Aplica el da�o de melee al jugador
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(meleeDamage);
                Debug.Log($"El jugador recibi� {meleeDamage} de da�o por ataque cuerpo a cuerpo.");
            }
        }
    }

    public void FireProjectile(float distance)
    {
        // Aseg�rate de que el Pool Manager no sea nulo
        if (projectilePoolManager != null && enemyMovement.m_Player != null)
        {
            GameObject projectile = projectilePoolManager.GetProjectile();
            if (projectile != null)
            {
                // Establece la posici�n inicial del proyectil
                projectile.transform.position = transform.position + new Vector3(0.5f, 1.5f, 0); // Ajustar si es necesario

                // Establece el objetivo del proyectil
                Shuriken projectileScript = projectile.GetComponent<Shuriken>();
                if (projectileScript != null)
                {
                    projectileScript.SetTarget(enemyMovement.m_Player); // Asigna el jugador como objetivo
                    projectileScript.SetDamage(distance, rangeDamageMax, rangeDamageMin, shootDistance); // Asigna el da�o
                }

                // Establece la rotaci�n inicial del proyectil
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
