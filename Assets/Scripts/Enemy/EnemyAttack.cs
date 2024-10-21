using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private float meleeDistance = 1f; // Distancia para ataque cuerpo a cuerpo
    [SerializeField]
    private float attackCooldown = 2f; // Tiempo de cooldown entre ataques de rango
    private float lastAttackTime = -Mathf.Infinity; // Registro del último ataque
    private ProjectilePoolManager projectilePoolManager; // Referencia al Pool Manager

    [SerializeField]
    private float meleeDamage = 1f; // Daño del ataque cuerpo a cuerpo
    [SerializeField]
    private float rangeDamageMax = 1.5f; // Daño máximo del ataque de rango
    [SerializeField]
    private float rangeDamageMin = 0.5f; // Daño mínimo del ataque de rango

    private Animator m_SpriteAnimator; // Referencia al Animator
    public bool isPerformingMeleeAttack = false;
    public bool isPerformingRangeAttack = false;

    private EnemyMovement enemyMovement; // Referencia a EnemyMovement

    private void Awake()
    {
        m_SpriteAnimator = transform.Find("Sprite").GetComponent<Animator>(); // Referencia al Animator
        enemyMovement = GetComponent<EnemyMovement>(); // Asigna EnemyMovement
        projectilePoolManager = FindObjectOfType<ProjectilePoolManager>(); // Encuentra el Pool Manager para proyectiles
    }

    // Método para desencadenar el ataque cuerpo a cuerpo
    public void TriggerMeleeAttack()
    {
        if (!isPerformingMeleeAttack)
        {
            isPerformingMeleeAttack = true;  // Marca que está atacando
            m_SpriteAnimator.Play("MeleeAttack");  // Fuerza la reproducción de la animación de ataque cuerpo a cuerpo
            Debug.Log("Iniciando ataque cuerpo a cuerpo");

            // Si algo falla, asegura el reseteo después de 1.5 segundos (ajusta según la duración de la animación)
            Invoke("ResetMeleeAttack", 1.5f);
        }
    }

    // Método para desencadenar el ataque a distancia
    public void TriggerRangeAttack()
    {
        if (!isPerformingRangeAttack && Time.time >= lastAttackTime + attackCooldown)
        {
            isPerformingRangeAttack = true;  // Marca que está atacando
            lastAttackTime = Time.time;  // Registra el tiempo del último ataque
            m_SpriteAnimator.Play("RangeAttack");  // Fuerza la reproducción de la animación de ataque a distancia
            Debug.Log("Iniciando ataque a distancia");

            // Si algo falla, asegura el reseteo después de 1.5 segundos (ajusta según la duración de la animación)
            Invoke("ResetRangeAttack", 1.5f);
        }
    }

    // Este método será llamado al final de la animación de ataque cuerpo a cuerpo (con Animation Event)
    public void PerformMeleeAttack()
    {
        if (enemyMovement.m_Player != null)
        {
            PlayerHealth playerHealth = enemyMovement.m_Player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(meleeDamage);  // Aplica el daño cuerpo a cuerpo
                Debug.Log("Daño cuerpo a cuerpo aplicado");
            }
        }
        isPerformingMeleeAttack = false;  // Resetea el estado para permitir futuros ataques
        Debug.Log("isPerformingMeleeAttack reseteado en PerformMeleeAttack");
    }

    public void PerformRangeAttack()
    {
        if (enemyMovement.m_Player != null)
        {
            GameObject projectile = projectilePoolManager.GetProjectile();
            if (projectile != null)
            {
                // Configuración del proyectil
                projectile.transform.position = transform.position + new Vector3(0.5f, 1.5f, 0); // Ajustar la posición según sea necesario

                // Asignar el objetivo y la velocidad del proyectil
                Shuriken projectileScript = projectile.GetComponent<Shuriken>();
                if (projectileScript != null)
                {
                    projectileScript.SetTarget(enemyMovement.m_Player);  // Establecer el jugador como objetivo
                    float distance = enemyMovement.GetPlayerDistance();
                    projectileScript.SetDamage(distance, rangeDamageMax, rangeDamageMin, meleeDistance);  // Configurar el daño en función de la distancia
                }

                // Activar el proyectil
                projectile.SetActive(true);
                Debug.Log("Proyectil lanzado");
            }
            else
            {
                Debug.LogWarning("No se encontró proyectil en el pool");
            }
        }
    }


    // Funciones de reseteo de failsafe por si OnStateExit no lo hace correctamente
    private void ResetMeleeAttack()
    {
        isPerformingMeleeAttack = false;
        Debug.Log("isPerformingMeleeAttack reseteado mediante Invoke");
    }

    private void ResetRangeAttack()
    {
        isPerformingRangeAttack = false;
        Debug.Log("isPerformingRangeAttack reseteado mediante Invoke");
    }
}
