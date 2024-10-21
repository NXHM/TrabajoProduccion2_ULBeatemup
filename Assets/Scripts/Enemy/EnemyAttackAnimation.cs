using UnityEngine;

public class EnemyAttackAnimation : StateMachineBehaviour
{
    private bool hasAttacked = false;

    // Este método es llamado al inicio de la animación
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasAttacked = false; // Resetea el flag para asegurar que solo atacamos una vez por animación
    }

    // Este método se llama cuando la animación finaliza
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("ENTRAAAAAAAAA");

        // Accede al AttackEventProxy en el mismo objeto que el Animator
        var attackEventProxy = animator.gameObject.GetComponent<AttackEventProxy>();
        if (attackEventProxy != null && !hasAttacked)
        {
            var enemyMovement = animator.gameObject.GetComponentInParent<EnemyMovement>();
            Debug.Log($"Enemy movement {enemyMovement}");
            if (enemyMovement != null)
            {
                // Dependiendo del estado actual del enemigo, ejecuta el ataque correspondiente
                if (enemyMovement.GetState() == EnemyState.AttackMelee)
                {
                    attackEventProxy.PerformMeleeAttack(); // Llama al método del AttackEventProxy para realizar el ataque cuerpo a cuerpo
                }
                else if (enemyMovement.GetState() == EnemyState.AttackRange)
                {
                    attackEventProxy.PerformRangeAttack(); // Llama al método del AttackEventProxy para realizar el ataque a distancia
                }

                hasAttacked = true; // Marca el ataque como ejecutado
            }
        }

        // Resetear isPerformingMeleeAttack y isPerformingRangeAttack para que pueda atacar nuevamente
        var enemyAttack = animator.gameObject.GetComponentInParent<EnemyAttack>();
        if (enemyAttack != null)
        {
            enemyAttack.isPerformingMeleeAttack = false;
            enemyAttack.isPerformingRangeAttack = false;
            Debug.Log("Reseteando isPerformingMeleeAttack y isPerformingRangeAttack al finalizar la animación.");
        }
    }
}
