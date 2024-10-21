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
        mainCamera = Camera.main; // Obtener la c�mara principal (la que renderiza)
    }

    private void Start()
    {
        AdjustBoundsToCamera();
    }

    private void Update()
    {
        // Si la resoluci�n cambia o el tama�o de la c�mara cambia, recalcular los l�mites
        float camHeight = 2f * virtualCamera.m_Lens.OrthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        if (camHeight != boxCollider.size.y || camWidth != boxCollider.size.x)
        {
            AdjustBoundsToCamera();
        }
    }

    private void AdjustBoundsToCamera()
    {
        // Obtener el tama�o visible de la c�mara virtual en unidades del mundo
        float camHeight = 2f * virtualCamera.m_Lens.OrthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        // Ajustar el BoxCollider2D al tama�o de la c�mara virtual
        boxCollider.size = new Vector2(camWidth, camHeight);
        boxCollider.offset = Vector2.zero; // Centramos el collider en la posici�n de la c�mara

        Debug.Log($"Bounds ajustado al tama�o de la c�mara virtual: {boxCollider.size}");
    }
}
