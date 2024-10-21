using UnityEngine;

public class Shuriken : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f; // Velocidad del proyectil
    private Transform target; // Referencia al objetivo (jugador u otro objeto)

    private void Update()
    {
        // Mueve el proyectil hacia el objetivo si se ha asignado uno
        if (target != null)
        {
            MoveTowardsTarget();
        }
        else
        {
            // Si no hay objetivo, puedes devolver el proyectil al pool (opcional)
            ProjectilePoolManager poolManager = FindObjectOfType<ProjectilePoolManager>();
            poolManager.ReturnProjectile(gameObject);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget; // Asigna el nuevo objetivo al proyectil
    }

    private void MoveTowardsTarget()
    {
        // Calcula la direcci�n hacia el objetivo
        Vector3 direction = (target.position - transform.position).normalized;

        // Mueve el proyectil usando Rigidbody2D
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;

        // Opcional: verifica si el proyectil ha alcanzado al jugador
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            // Devuelve el proyectil al pool si alcanza al jugador
            ProjectilePoolManager poolManager = FindObjectOfType<ProjectilePoolManager>();
            poolManager.ReturnProjectile(gameObject);
        }
    }
}
