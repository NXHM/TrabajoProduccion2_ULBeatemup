using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(BoxCollider2D))]
public class CinemachineCameraBoundsScaler : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private CinemachineVirtualCamera virtualCamera;
    private Camera mainCamera;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>(); // Encuentra la Cinemachine Virtual Camera
        mainCamera = Camera.main; // Obtener la cámara principal (la que renderiza)
    }

    private void Start()
    {
        AdjustBoundsToCamera();
    }

    private void Update()
    {
        // Si la resolución cambia o el tamaño de la cámara cambia, recalcular los límites
        float camHeight = 2f * virtualCamera.m_Lens.OrthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        if (camHeight != boxCollider.size.y || camWidth != boxCollider.size.x)
        {
            AdjustBoundsToCamera();
        }
    }

    private void AdjustBoundsToCamera()
    {
        // Obtener el tamaño visible de la cámara virtual en unidades del mundo
        float camHeight = 2f * virtualCamera.m_Lens.OrthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        // Ajustar el BoxCollider2D al tamaño de la cámara virtual
        boxCollider.size = new Vector2(camWidth, camHeight);
        boxCollider.offset = Vector2.zero; // Centramos el collider en la posición de la cámara

        Debug.Log($"Bounds ajustado al tamaño de la cámara virtual: {boxCollider.size}");
    }
}
