using UnityEngine;

public class Shuriken : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f; // Velocidad del proyectil
    private Transform target; // Referencia al objetivo (jugador u otro objeto)
    private int damage; // Variable para almacenar el daño del proyectil

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

    public void SetDamage(int newDamage)
    {
        damage = newDamage; // Asigna el daño que el proyectil causará
    }

    private void MoveTowardsTarget()
    {
        // Calcula la dirección hacia el objetivo
        Vector3 direction = (target.position - transform.position).normalized;

        // Mueve el proyectil usando Rigidbody2D
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;

        // Opcional: verifica si el proyectil ha alcanzado al jugador
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            // Aplicar daño al objetivo si es un jugador
            PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // Aplicar el daño al jugador
            }

            // Devuelve el proyectil al pool
            ProjectilePoolManager poolManager = FindObjectOfType<ProjectilePoolManager>();
            poolManager.ReturnProjectile(gameObject);
        }
    }
}
