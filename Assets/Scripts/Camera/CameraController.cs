using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private EnemySpawner enemySpawner;
    private CinemachineVirtualCamera virtualCamera; // Referencia a la c�mara virtual de Cinemachine
    private Transform playerTransform; // Referencia al jugador

    private bool canMove = true;
    private Vector3 fixedCameraPosition; // Posici�n fija para cuando la c�mara se detiene

    private void Awake()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>(); // Obtener la referencia a la c�mara virtual
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Obtener la referencia al jugador
    }

    void Start()
    {
        virtualCamera.Follow = playerTransform; // Hacer que la c�mara siga al jugador inicialmente
        fixedCameraPosition = new Vector3(transform.position.x, -1.103f, transform.position.z); // Establecer la posici�n Y fija
    }

    void Update()
    {
        if (enemySpawner != null && enemySpawner.EnemiesSpawned - enemySpawner.EnemiesDead > 0)
        {
            // Si hay enemigos activos, fijamos la c�mara en su posici�n actual, pero con Y fija en -1.103
            if (canMove)
            {
                virtualCamera.Follow = null; // Detenemos el seguimiento del jugador
                transform.position = new Vector3(transform.position.x, -1.103f, transform.position.z); // Fijar la posici�n en Y
                canMove = false;
            }
        }
        else
        {
            // Si no hay enemigos, la c�mara sigue al jugador
            if (!canMove)
            {
                virtualCamera.Follow = playerTransform; // Reanudar el seguimiento del jugador
                canMove = true;
            }
        }
    }
}
