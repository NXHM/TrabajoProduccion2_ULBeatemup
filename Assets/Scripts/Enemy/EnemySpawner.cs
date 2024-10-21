using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Variables privadas
    private int enemiesSpawned = 0;
    private int enemiesDead = 0;

    // Propiedades públicas para acceder a las variables
    public int EnemiesSpawned { get { return enemiesSpawned; } }
    public int EnemiesDead { get { return enemiesDead; } }

    [SerializeField] public GameObject enemyPrefab;  // Usamos un solo prefab de enemigo
    public float spawnInterval = 3f;
    public float initialSpawnDelay = 10f;

    public int maxEnemies = 5; // Número máximo de enemigos que pueden estar activos simultáneamente

    private bool isSpawnerActive = true;

    [SerializeField] private PolygonCollider2D bounds;
    public float minDistanceBetweenEnemies = 1.5f;

    private List<Vector2> spawnedPositions = new List<Vector2>();

    void Start()
    {
        Debug.Log($"[EnemySpawner] Iniciado con delay de {initialSpawnDelay} segundos.");
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(initialSpawnDelay);

        while (isSpawnerActive)
        {
            if (enemiesSpawned - enemiesDead < maxEnemies)
            {
                Vector2 spawnPosition = GetValidSpawnPosition();
                if (spawnPosition != Vector2.zero)
                {
                    Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                    spawnedPositions.Add(spawnPosition);
                    enemiesSpawned++; // Incrementamos el contador de enemigos generados
                }
                else
                {
                    Debug.LogWarning("[EnemySpawner] No se encontró una posición válida.");
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    Vector2 GetValidSpawnPosition()
    {
        int maxAttempts = 10;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 candidatePosition = GetRandomPointInCameraView();
            if (IsPositionValid(candidatePosition))
                return candidatePosition;
        }

        return Vector2.zero;
    }

    Vector2 GetRandomPointInCameraView()
    {
        Camera cam = Camera.main;
        Vector2 cameraMin = cam.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 cameraMax = cam.ViewportToWorldPoint(new Vector2(1, 1));

        // Limitar la coordenada Y para que los enemigos no aparezcan por debajo de -2.88
        float randomX = UnityEngine.Random.Range(cameraMin.x, cameraMax.x);
        float randomY = UnityEngine.Random.Range(-7.21f, -2.88f); // Limitar Y entre -7.21 y -2.88


        Vector2 randomPoint = new Vector2(randomX, randomY);

        if (bounds.OverlapPoint(randomPoint))
        {
            return randomPoint;
        }

        return Vector2.zero;
    }

    bool IsPositionValid(Vector2 position)
    {
        foreach (Vector2 spawnedPosition in spawnedPositions)
        {
            if (Vector2.Distance(spawnedPosition, position) < minDistanceBetweenEnemies)
                return false;
        }

        return bounds.OverlapPoint(position);
    }

    public void EnemyDied()
    {
        enemiesDead++; // Incrementamos el contador de enemigos muertos
        Debug.Log($"[EnemySpawner] Enemigo eliminado. Total muertos: {enemiesDead}");
    }
}
