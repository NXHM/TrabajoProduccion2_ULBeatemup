using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 1f; // Vida m�xima del jugador
    [SerializeField]
    private float currentHealth;    // Vida actual del jugador

     [SerializeField]
    private Transform healthBarFill;
    [SerializeField]
    private GameObject enemy;
    private Vector3 originalScale;

    private Vector3 initpos; 
    


    private void Awake()
    {
        currentHealth = maxHealth; // Inicia con la vida m�xima
        originalScale = healthBarFill.localScale;

        UpdateHealthBar();
    }

    void Update()
    {
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
        Destroy(enemy);
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

    private void UpdateHealthBar()
    {  

    

    healthBarFill.localScale = new Vector3(originalScale.x * currentHealth , originalScale.y, originalScale.z);

    Renderer renderer = healthBarFill.GetComponent<Renderer>();
    if (currentHealth > 0.75f)
    {
        
        renderer.material.color = Color.green;
    }
    else if (currentHealth> 0.5f)
    {
        Debug.Log("Amarillo");
        renderer.material.color = Color.yellow;
    }
    else if (currentHealth > 0.25f)
    {
        renderer.material.color = new Color(1f, 0.65f, 0f); // Anaranjado
    }
    else
    {
        renderer.material.color = Color.red;
    }
    }

}