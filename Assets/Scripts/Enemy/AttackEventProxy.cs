using UnityEngine;

public class AttackEventProxy : MonoBehaviour
{
    private EnemyAttack enemyAttack;

    private void Awake()
    {
        // Busca el componente EnemyAttack en el objeto padre
        enemyAttack = GetComponentInParent<EnemyAttack>();
    }

    // M�todo que ser� llamado desde el evento de animaci�n para el ataque cuerpo a cuerpo
    public void PerformMeleeAttack()
    {
        if (enemyAttack != null)
        {
            enemyAttack.PerformMeleeAttack();  // Llama al m�todo PerformMeleeAttack del script EnemyAttack en el padre
        }
        else
        {
            Debug.LogError("EnemyAttack no se encontr� en el objeto padre.");
        }
    }

    // M�todo que ser� llamado desde el evento de animaci�n para el ataque a distancia
    public void PerformRangeAttack()
    {
        if (enemyAttack != null)
        {
            enemyAttack.PerformRangeAttack();  // Llama al m�todo PerformRangeAttack del script EnemyAttack en el padre
        }
        else
        {
            Debug.LogError("EnemyAttack no se encontr� en el objeto padre.");
        }
    }
}
