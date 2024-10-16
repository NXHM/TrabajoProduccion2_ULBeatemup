using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject enemyPrefab;

    public float spawnInterval = 3f; // Intervalo de tiempo entre spawns
    private bool isSpawnerActive = true; // Indica si el spawner est� activo
    private int enemiesSpawned = 0; 
    private int enemiesDead = 0; 
    public int maxEnemies = 5; 

    [SerializeField]
    private PolygonCollider2D bounds; // �rea donde pueden aparecer los enemigos

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    // Corrutina para generar enemigos a intervalos de tiempo
    IEnumerator SpawnEnemies()
    {
        while (isSpawnerActive)
        {
            // Si no hemos llegado al m�ximo de enemigos activos
            if (enemiesSpawned - enemiesDead < maxEnemies)
            {
                SpawnEnemy();
                enemiesSpawned++;
            }

            // Esperar el intervalo antes de generar otro enemigo
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Genera el enemigo
    void SpawnEnemy()
    {
        // Generar una posici�n aleatoria dentro de los l�mites
        Vector2 spawnPosition = GetRandomPointInBounds();

        // Instanciar el enemigo en la posici�n generada
        // prefab, posici�n, sin rotar
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    // Obtener una posici�n aleatoria dentro de bounds
    Vector2 GetRandomPointInBounds()
    {
        Vector2 point = Vector2.zero; 
        while (!IsPointInsideBounds(point))
        {
            // Obtener el punto dentro del �rea delimitada por el collider, haciendo un random between
            point = new Vector2(
                Random.Range(bounds.bounds.min.x, bounds.bounds.max.x),
                Random.Range(bounds.bounds.min.y, bounds.bounds.max.y/3)
            );
        } 

        return point;
    }

    // El area esta dentro del mapa??
    bool IsPointInsideBounds(Vector2 point)
    {
        return bounds.OverlapPoint(point);
    }

    // Sube contador de muertes
    public void EnemyDied()
    {
        enemiesDead++;
    }
}
