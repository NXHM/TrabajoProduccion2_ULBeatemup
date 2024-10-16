using UnityEngine;

public class Shuriken : MonoBehaviour
{
    private float speed;
    private Vector2 direction;

    public void Initialize(Vector2 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;

        // Asegurar que la dirección sea estrictamente horizontal
        this.direction.y = 0;
        this.direction = this.direction.normalized;

        // Orientar el shuriken en la dirección de lanzamiento
        float angle = this.direction.x > 0 ? 0f : 180f;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Update()
    {
        // Mover el shuriken en la dirección establecida
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Hit player!");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Destroy(gameObject, 5f);
    }
}