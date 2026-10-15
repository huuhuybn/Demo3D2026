using Unity.Cinemachine;
using UnityEngine;

public class CameraShooterController : MonoBehaviour
{

    public CinemachineCamera aimCam;
    public CinemachineCamera zoneCam;
    void Update()
    {
        aimCam.Priority.Value = Input.GetMouseButton(1) ? 20 : 5;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zone")) zoneCam.Priority.Value = 25;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Zone")) zoneCam.Priority.Value = 5;
    }
}
