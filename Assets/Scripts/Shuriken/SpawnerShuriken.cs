using UnityEngine;

public class SpawnerShuriken : MonoBehaviour
{
    public GameObject shurikenPrefab;
    public float attackCooldown = 2f;
    public float shurikenSpeed = 10f;
    private float cooldownTimer;
    private bool facingRight = true;

    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
        else
        {
            Attack();
        }
    }

    private void Attack()
    {
        LaunchShuriken();
        cooldownTimer = attackCooldown;
    }

    private void LaunchShuriken()
    {
        if (shurikenPrefab == null)
        {
            Debug.LogError("Shuriken prefab not assigned!");
            return;
        }

        Vector2 direction = facingRight ? Vector2.right : Vector2.left;

        // Usar el centro exacto del objeto como punto de lanzamiento
        Vector3 spawnPosition = transform.position;

        GameObject shuriken = Instantiate(shurikenPrefab, spawnPosition, Quaternion.identity);
        if (shuriken == null)
        {
            Debug.LogError("Failed to instantiate Shuriken!");
            return;
        }

        Shuriken shurikenComponent = shuriken.GetComponent<Shuriken>();
        if (shurikenComponent != null)
        {
            shurikenComponent.Initialize(direction, shurikenSpeed);
        }
        else
        {
            Debug.LogError("Shuriken component not found on instantiated object!");
        }
    }

    public void FlipDirection()
    {
        facingRight = !facingRight;
    }
}