using Unity.Cinemachine;
using UnityEngine;

public class CameraShakeTester : MonoBehaviour
{
    
    public float shakeForce = 2f;
    CinemachineImpulseSource impulseSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            impulseSource.GenerateImpulseWithForce(shakeForce);
        }
    }
}
