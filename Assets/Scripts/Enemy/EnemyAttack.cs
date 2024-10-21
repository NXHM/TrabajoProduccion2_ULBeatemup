using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private float meleeDistance = 1f; // Distancia para ataque cuerpo a cuerpo
    [SerializeField]
    private float attackCooldown = 2f; // Tiempo de cooldown entre ataques de rango
    private float lastAttackTime = -Mathf.Infinity; // Registro del �ltimo ataque
    private ProjectilePoolManager projectilePoolManager; // Referencia al Pool Manager

    [SerializeField]
    private float meleeDamage = 1f; // Da�o del ataque cuerpo a cuerpo
    [SerializeField]
    private float rangeDamageMax = 1.5f; // Da�o m�ximo del ataque de rango
    [SerializeField]
    private float rangeDamageMin = 0.5f; // Da�o m�nimo del ataque de rango

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

    // M�todo para desencadenar el ataque cuerpo a cuerpo
    public void TriggerMeleeAttack()
    {
        if (!isPerformingMeleeAttack)
        {
            isPerformingMeleeAttack = true;  // Marca que est� atacando
            m_SpriteAnimator.Play("MeleeAttack");  // Fuerza la reproducci�n de la animaci�n de ataque cuerpo a cuerpo
            Debug.Log("Iniciando ataque cuerpo a cuerpo");

            // Si algo falla, asegura el reseteo despu�s de 1.5 segundos (ajusta seg�n la duraci�n de la animaci�n)
            Invoke("ResetMeleeAttack", 1.5f);
        }
    }

    // M�todo para desencadenar el ataque a distancia
    public void TriggerRangeAttack()
    {
        if (!isPerformingRangeAttack && Time.time >= lastAttackTime + attackCooldown)
        {
            isPerformingRangeAttack = true;  // Marca que est� atacando
            lastAttackTime = Time.time;  // Registra el tiempo del �ltimo ataque
            m_SpriteAnimator.Play("RangeAttack");  // Fuerza la reproducci�n de la animaci�n de ataque a distancia
            Debug.Log("Iniciando ataque a distancia");

            // Si algo falla, asegura el reseteo despu�s de 1.5 segundos (ajusta seg�n la duraci�n de la animaci�n)
            Invoke("ResetRangeAttack", 1.5f);
        }
    }

    // Este m�todo ser� llamado al final de la animaci�n de ataque cuerpo a cuerpo (con Animation Event)
    public void PerformMeleeAttack()
    {
        if (enemyMovement.m_Player != null)
        {
            PlayerHealth playerHealth = enemyMovement.m_Player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(meleeDamage);  // Aplica el da�o cuerpo a cuerpo
                Debug.Log("Da�o cuerpo a cuerpo aplicado");
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
                // Configuraci�n del proyectil
                projectile.transform.position = transform.position + new Vector3(0.5f, 1.5f, 0); // Ajustar la posici�n seg�n sea necesario

                // Asignar el objetivo y la velocidad del proyectil
                Shuriken projectileScript = projectile.GetComponent<Shuriken>();
                if (projectileScript != null)
                {
                    projectileScript.SetTarget(enemyMovement.m_Player);  // Establecer el jugador como objetivo
                    float distance = enemyMovement.GetPlayerDistance();
                    projectileScript.SetDamage(distance, rangeDamageMax, rangeDamageMin, meleeDistance);  // Configurar el da�o en funci�n de la distancia
                }

                // Activar el proyectil
                projectile.SetActive(true);
                Debug.Log("Proyectil lanzado");
            }
            else
            {
                Debug.LogWarning("No se encontr� proyectil en el pool");
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
