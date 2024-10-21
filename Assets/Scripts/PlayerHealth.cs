using UnityEngine;
using GameUI; // Espacio de nombres donde está definida la clase HealthBar

public class PlayerHealth : MonoBehaviour
{
    public HealthBar healthBar; // Asignar el componente HealthBar desde el Inspector
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        // Reaparecer al jugador en una posición específica (por ejemplo, la inicial)
        transform.position = Vector3.zero; // Cambia a la posición de inicio del nivel según sea necesario
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);
    }
}
