using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BossCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    
    void Start()
    {
        virtualCamera.Follow = null;
        transform.position = new Vector3(transform.position.x, -1.103f, transform.position.z);
    }
}
