using System;
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

    // Instancia estática de Random para evitar patrones repetitivos
    private static readonly System.Random random = new System.Random();

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

    Vector2 GetRandomPointInPolygon(float marginX = 15f, float marginY=1.2f)
    {
        int pathIndex = random.Next(bounds.pathCount); // Seleccionar una ruta aleatoria
        Vector2[] pathPoints = bounds.GetPath(pathIndex); // Obtener los puntos de la ruta seleccionada

        // Determinar los límites de X aplicando el margen
        float minX = Mathf.Min(Array.ConvertAll(pathPoints, p => p.x)) + marginX;
        float maxX = Mathf.Max(Array.ConvertAll(pathPoints, p => p.x)) - marginX;

        // Ajustar los límites de Y con margen
        float minY = Mathf.Min(Array.ConvertAll(pathPoints, p => p.y)) + marginY;
        float maxY = -1.25f;

        // Generar un punto aleatorio dentro del rango ajustado
        float randomX = Lerp(minX, maxX, (float)random.NextDouble());
        float randomY = Lerp(minY, maxY, (float)random.NextDouble());

        Vector2 randomPoint = new Vector2(randomX, randomY);

        // Verificar si el punto está dentro del polígono
        if (bounds.OverlapPoint(randomPoint))
        {
            return randomPoint;
        }

        // Si no es válido, retornar Vector2.zero
        return Vector2.zero;
    }


    bool IsPositionValid(Vector2 position)
    {
        // Comprobar la distancia con otras posiciones generadas
        foreach (Vector2 spawnedPosition in spawnedPositions)
        {
            if (Vector2.Distance(spawnedPosition, position) < minDistanceBetweenEnemies)
                return false;
        }

        // Comprobar si la posición está dentro del polígono
        return bounds.OverlapPoint(position);
    }

    public void EnemyDied()
    {
        enemiesDead++;
        Debug.Log($"[EnemySpawner] Enemigo eliminado. Total muertos: {enemiesDead}");
    }

    // Función de interpolación lineal
    private float Lerp(float min, float max, float t)
    {
        return min + (max - min) * t;
    }
}
