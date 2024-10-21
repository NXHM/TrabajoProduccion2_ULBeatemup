using UnityEngine;

public class AttackEventProxy : MonoBehaviour
{
    private EnemyAttack enemyAttack;

    private void Awake()
    {
        // Busca el componente EnemyAttack en el objeto padre
        enemyAttack = GetComponentInParent<EnemyAttack>();
    }

    // Método que será llamado desde el evento de animación para el ataque cuerpo a cuerpo
    public void PerformMeleeAttack()
    {
        if (enemyAttack != null)
        {
            enemyAttack.PerformMeleeAttack();  // Llama al método PerformMeleeAttack del script EnemyAttack en el padre
        }
        else
        {
            Debug.LogError("EnemyAttack no se encontró en el objeto padre.");
        }
    }

    // Método que será llamado desde el evento de animación para el ataque a distancia
    public void PerformRangeAttack()
    {
        if (enemyAttack != null)
        {
            enemyAttack.PerformRangeAttack();  // Llama al método PerformRangeAttack del script EnemyAttack en el padre
        }
        else
        {
            Debug.LogError("EnemyAttack no se encontró en el objeto padre.");
        }
    }
}
