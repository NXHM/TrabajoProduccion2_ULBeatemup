using System.Collections.Generic;
using UnityEngine;

public class ProjectilePoolManager : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab; // Prefab del proyectil
    [SerializeField]
    private int initialPoolSize = 10; // Tama�o inicial del pool
    [SerializeField]
    private int additionalProjectiles = 5; // Cantidad de proyectiles adicionales a crear cuando el pool est� vac�o
    [SerializeField]
    private int maxPoolSize = 100; // Tama�o m�ximo del pool (opcional para prevenir uso excesivo de memoria)

    private Queue<GameObject> projectilePool; // Cola para gestionar los proyectiles inactivos

    private void Awake()
    {
        // Inicializar la cola de proyectiles
        projectilePool = new Queue<GameObject>();

        // Llenar el pool con proyectiles inactivos
        FillPool(initialPoolSize);
    }

    private void FillPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.SetActive(false); // Desactiva el proyectil
            projectilePool.Enqueue(projectile); // A�ade el proyectil al pool
        }
    }

    public GameObject GetProjectile()
    {
        // Verifica si hay proyectiles disponibles
        if (projectilePool.Count > 0)
        {
            GameObject projectile = projectilePool.Dequeue(); // Obtiene un proyectil del pool
            projectile.SetActive(true); // Activa el proyectil
            return projectile;
        }
        else
        {
            // Si no hay proyectiles disponibles, crea nuevos hasta el tama�o m�ximo
            Debug.Log("Pool vac�o, creando m�s proyectiles...");
            int currentPoolSize = initialPoolSize + projectilePool.Count; // Corrige este c�lculo
            int newProjectiles = Mathf.Min(additionalProjectiles, maxPoolSize - currentPoolSize);
            if (newProjectiles > 0)
            {
                FillPool(newProjectiles); // Llena el pool con nuevos proyectiles
            }
            return GetProjectile(); // Llama nuevamente para obtener un proyectil del pool
        }
    }

    public void ReturnProjectile(GameObject projectile)
    {
        projectile.SetActive(false); // Desactiva el proyectil
        projectilePool.Enqueue(projectile); // Lo devuelve al pool
    }
}
