using UnityEngine;
using GameUI;

public class EnemyHealth : MonoBehaviour
{
    public HealthBar healthBar; // Asignar el componente HealthBar desde el Inspector
    public int maxHealth = 50; // Puedes ajustar el valor según cada enemigo
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
            Die();
        }
    }

    void Die()
    {
        // Desactiva el enemigo o activa la animación de muerte, etc.
        gameObject.SetActive(false); // Ejemplo simple de desactivación
    }
}
