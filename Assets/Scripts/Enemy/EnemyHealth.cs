using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 5f; // Vida máxima del enemigo
    private float currentHealth;  // Vida actual del enemigo

    private void Awake()
    {
        currentHealth = maxHealth; // Inicia con la vida máxima
    }

    // Método para que el enemigo reciba daño
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Enemigo recibió {damage} de daño. Vida restante: {currentHealth}");

        // Si la vida del enemigo llega a 0 o menos, el enemigo muere
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    // Método para manejar la muerte del enemigo
    private void Die()
    {
        Debug.Log("Enemigo ha muerto.");
        // Aquí podrías añadir efectos visuales, sonidos, etc.
        Destroy(gameObject); // Elimina al enemigo de la escena

        // Notificar al EnemySpawner que el enemigo ha muerto (opcional)
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.EnemyDied(); // Notifica al spawner que un enemigo ha muerto
        }
    }
}
