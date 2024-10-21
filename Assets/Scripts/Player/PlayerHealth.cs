using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 10f; // Vida m�xima del jugador
    private float currentHealth;    // Vida actual del jugador

    private void Awake()
    {
        currentHealth = maxHealth; // Inicia con la vida m�xima
    }

    // M�todo para reducir la vida del jugador
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"El jugador ha recibido {damage} de da�o. Vida restante: {currentHealth}");

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    // M�todo para manejar la muerte del jugador
    private void Die()
    {
        Debug.Log("El jugador ha muerto.");
        // Aqu� puedes a�adir la l�gica de muerte, como desactivar el control del jugador, reproducir animaciones, etc.
        // Deshabilitar el control del jugador, por ejemplo:
        GetComponent<PlayerInput>().enabled = false;
    }

    // M�todo para curar al jugador
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
