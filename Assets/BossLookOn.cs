using Unity.Cinemachine;
using UnityEngine;

public class BossLookOn : MonoBehaviour
{
    public CinemachineCamera lookOnCamera;
    private bool isLookOn = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isLookOn = !isLookOn;
            lookOnCamera.Priority = isLookOn ? 30 : 0;
        }
    }
}
