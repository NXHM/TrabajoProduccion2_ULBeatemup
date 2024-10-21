using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private EnemySpawner enemySpawner;
    private CinemachineVirtualCamera virtualCamera; // Referencia a la cámara virtual de Cinemachine
    private Transform playerTransform; // Referencia al jugador

    private bool canMove = true;
    private Vector3 fixedCameraPosition; // Posición fija para cuando la cámara se detiene

    private void Awake()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>(); // Obtener la referencia a la cámara virtual
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Obtener la referencia al jugador
    }

    void Start()
    {
        virtualCamera.Follow = playerTransform; // Hacer que la cámara siga al jugador inicialmente
        fixedCameraPosition = new Vector3(transform.position.x, -1.103f, transform.position.z); // Establecer la posición Y fija
    }

    void Update()
    {
        if (enemySpawner != null && enemySpawner.EnemiesSpawned - enemySpawner.EnemiesDead > 0)
        {
            // Si hay enemigos activos, fijamos la cámara en su posición actual, pero con Y fija en -1.103
            if (canMove)
            {
                virtualCamera.Follow = null; // Detenemos el seguimiento del jugador
                transform.position = new Vector3(transform.position.x, -1.103f, transform.position.z); // Fijar la posición en Y
                canMove = false;
            }
        }
        else
        {
            // Si no hay enemigos, la cámara sigue al jugador
            if (!canMove)
            {
                virtualCamera.Follow = playerTransform; // Reanudar el seguimiento del jugador
                canMove = true;
            }
        }
    }
}
