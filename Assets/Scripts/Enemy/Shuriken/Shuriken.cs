using UnityEngine;

public class Shuriken : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f; // Velocidad del proyectil
    [SerializeField]
    private float maxLifetime = 3f; // Tiempo m�ximo de vida del proyectil
    private Transform target; // Referencia al objetivo (jugador u otro objeto)
    private float lifetime; // Tiempo de vida restante del proyectil
    private float damage; // Da�o que infligir� el proyectil

    private void OnEnable()
    {
        lifetime = maxLifetime; // Resetea el tiempo de vida cuando se activa el proyectil
    }

    private void Update()
    {
        // Mueve el proyectil hacia el objetivo si se ha asignado uno
        if (target != null)
        {
            MoveTowardsTarget();
        }

        // Reduce el tiempo de vida y devuelve el proyectil si excede el tiempo m�ximo
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            ReturnToPool();
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget; // Asigna el nuevo objetivo al proyectil
    }

    public void SetDamage(float distance, float maxDamage, float minDamage, float maxDistance)
    {
        // Calcula el da�o basado en la distancia entre el enemigo y el jugador
        damage = Mathf.Lerp(maxDamage, minDamage, distance / maxDistance);
    }

    private void MoveTowardsTarget()
    {
        // Calcula la direcci�n hacia el objetivo
        Vector3 direction = (target.position - transform.position).normalized;

        // Mueve el proyectil usando Rigidbody2D
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Shuriken ha colisionado con el jugador");

            GameObject player = GameObject.FindWithTag("PlayerHealthBar");
            // Aplica el da�o de melee al jugador
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Debug.Log($"Aplicando {damage} de da�o al jugador");
                playerHealth.TakeDamage(0.15f); // Aplica el da�o al jugador
                Debug.Log($"El jugador recibi� {damage} de da�o por el shuriken.");
            }
            else
            {
                Debug.LogWarning("No se encontr� el componente PlayerHealth en el jugador.");
            }

            // Devuelve el proyectil al pool (o lo desactiva) al impactar con el jugador
            ReturnToPool();
        }
    }


    private void ReturnToPool()
    {
        // Devuelve el proyectil al pool
        ProjectilePoolManager poolManager = FindObjectOfType<ProjectilePoolManager>();
        poolManager.ReturnProjectile(gameObject);
    }
}
