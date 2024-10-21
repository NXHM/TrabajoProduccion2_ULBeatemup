using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private EnemyMovement enemyMovement;

    [SerializeField]
    private float shootDistance = 15f; // Distancia para disparar
    [SerializeField]
    private float chaseDistance = 4f; // Distancia para perseguir
    [SerializeField]
    private float meleeDistance = 0.5f; // Distancia para ataque cuerpo a cuerpo
    [SerializeField]
    private float projectileSpeed = 0.1f; // Velocidad del proyectil
        private int meleeDamage = 10; // Daño cuerpo a cuerpo
    [SerializeField]
    private int rangedDamage = 5; // Daño de proyectil
    private ProjectilePoolManager projectilePoolManager; // Referencia al Pool Manager

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
        else if (distance <= shootDistance)
        {
            PerformRangeAttack(); // Dispara sin cooldown
        }
    }

    private void PerformMeleeAttack()
    {
        Debug.Log("Realizando ataque cuerpo a cuerpo");
        enemyMovement.TriggerMeleeAttack();
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, meleeDistance); // Detectar colisiones en rango
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                // Infligir daño al jugador
                hit.GetComponent<PlayerHealth>().TakeDamage(meleeDamage);
            }
        }
    }

    private void ChasePlayer()
    {
        Debug.Log("Persiguiendo al jugador...");
        enemyMovement.SetState(EnemyState.Chasing);
    }
    
    private void PerformRangeAttack()
    {
        Debug.Log("Disparando proyectil");
        FireProjectile(); // Dispara directamente sin cooldown
    }

    public void FireProjectile() // Cambiado a public
    {
        // Aseg�rate de que el Pool Manager no sea nulo
        if (projectilePoolManager != null && enemyMovement.m_Player != null)
        {
            GameObject projectile = projectilePoolManager.GetProjectile();
            if (projectile != null)
            {
                // Establece la posici�n inicial del proyectil
                projectile.transform.position = transform.position + new Vector3(0.5f, 1f, 0); // Dispara desde la derecha del enemigo

                // Establece el objetivo del proyectil
                Shuriken projectileScript = projectile.GetComponent<Shuriken>();
                if (projectileScript != null)
                {
                    projectileScript.SetTarget(enemyMovement.m_Player); // Asigna el jugador como objetivo
                    projectileScript.SetDamage(rangedDamage);
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
