using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] public GameObject enemyPrefab;
    public float spawnInterval = 3f;
    public float initialSpawnDelay = 10f;
    private bool isSpawnerActive = true;

    private int enemiesSpawned = 0;
    private int enemiesDead = 0;
    public int maxEnemies = 5;

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
                    enemiesSpawned++;
                    Debug.Log($"[EnemySpawner] Enemigo generado en: {spawnPosition}");
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
            Vector2 candidatePosition = GetRandomPointInPolygon();
            if (IsPositionValid(candidatePosition))
                return candidatePosition;
        }

        // Si no se encuentra una posición válida, retornar Vector2.zero
        return Vector2.zero;
    }

    Vector2 GetRandomPointInPolygon(float margin = 15f)
    {
        int pathIndex = Random.Range(0, bounds.pathCount);
        Vector2[] pathPoints = bounds.GetPath(pathIndex);

        // Determinamos los límites mínimos y máximos del área del polígono
        float minX = Mathf.Min(pathPoints[0].x, pathPoints[1].x, pathPoints[2].x) + margin;
        float maxX = Mathf.Max(pathPoints[0].x, pathPoints[1].x, pathPoints[2].x) - margin;
        float minY = 0.4f;
        float maxY = Mathf.Max(pathPoints[0].y, pathPoints[1].y, pathPoints[2].y) - margin;

        // Generamos un punto aleatorio dentro de estos nuevos límites
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        return new Vector2(randomX, randomY);
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
        enemiesDead++;
        Debug.Log($"[EnemySpawner] Enemigo eliminado. Total muertos: {enemiesDead}");
    }
}
