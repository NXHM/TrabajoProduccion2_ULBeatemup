using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Image healthBarFill;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public Gradient healthColorGradient; // Arrastra un Gradient en Unity para configurar los colores

private void UpdateHealthBar()
{
    if (healthBarFill != null)
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;
        healthBarFill.color = healthColorGradient.Evaluate(healthBarFill.fillAmount);
    }
}


    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        UpdateHealthBar();
    }

    private void Die()
    {
        if (gameObject.CompareTag("Player"))
        {
            // Reaparece el jugador al inicio del nivel
            transform.position = Vector3.zero; // Ajustar posici�n de reaparecimiento seg�n sea necesario
            currentHealth = maxHealth;
        }
        else
        {
            // Para los enemigos, destruye el objeto al morir
            Destroy(gameObject);
        }
        UpdateHealthBar();
    }
}
