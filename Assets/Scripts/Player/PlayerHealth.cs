using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 1f; // Vida m�xima del jugador
    [SerializeField]
    private float currentHealth;    // Vida actual del jugador
    [SerializeField]
    private Transform playerStart;    // Vida actual del jugador

     [SerializeField]
    private Transform healthBarFill;
    private Vector3 originalScale;

    public Vector3 initpos; 
    


    private void Awake()
    {
        currentHealth = maxHealth; // Inicia con la vida m�xima
        originalScale = healthBarFill.localScale;
        initpos = playerStart.position;

        UpdateHealthBar();
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

        UpdateHealthBar();
    }

    // M�todo para manejar la muerte del jugador
    private void Die()
    {
        playerStart.position = initpos;
        Heal(1f);
    }

    // M�todo para curar al jugador
    public void Heal(float amount)
    {
        Renderer renderer = healthBarFill.GetComponent<Renderer>();
        currentHealth = maxHealth;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
            renderer.material.color = Color.green;
        }
        Debug.Log($"El jugador se ha curado. Vida actual: {currentHealth}");
    }

    private void UpdateHealthBar()
    {  

        const float epsilon = 0.0001f; // Define una pequeña tolerancia para el punto flotante

    if (currentHealth < epsilon)
    {
        currentHealth = 0f; // Ajusta la salud a 0 si es muy cercana a 0
    }

    healthBarFill.localScale = new Vector3(originalScale.x * currentHealth , originalScale.y, originalScale.z);

    Renderer renderer = healthBarFill.GetComponent<Renderer>();
    if (currentHealth > 0.75f)
    {
        
        renderer.material.color = Color.green;
    }
    else if (currentHealth> 0.5f)
    {
        renderer.material.color = Color.yellow;
    }
    else if (currentHealth > 0.25f)
    {
        renderer.material.color = new Color(1f, 0.65f, 0f);
    }
    else
    {
        renderer.material.color = Color.red;
    }
    }

}