using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 10f; // Vida máxima del jugador
    private float currentHealth;    // Vida actual del jugador

    private void Awake()
    {
        currentHealth = maxHealth; // Inicia con la vida máxima
    }

    // Método para reducir la vida del jugador
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"El jugador ha recibido {damage} de daño. Vida restante: {currentHealth}");

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    // Método para manejar la muerte del jugador
    private void Die()
    {
        Debug.Log("El jugador ha muerto.");
        // Aquí puedes añadir la lógica de muerte, como desactivar el control del jugador, reproducir animaciones, etc.
        // Deshabilitar el control del jugador, por ejemplo:
        GetComponent<PlayerInput>().enabled = false;
    }

    // Método para curar al jugador
    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log($"El jugador se ha curado. Vida actual: {currentHealth}");
    }
}
