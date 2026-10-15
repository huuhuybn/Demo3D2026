using UnityEngine;

public class FirstViewPlayerController : MonoBehaviour
{
    public Transform playerCamera;
    public float mouseSensitivity = 2f;
    float xRotation = 0f;
    public float moveSpeed = 5f;
    CharacterController characterController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void Update()
    {
        // xoay chuot
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // xoay trai phai
        transform.Rotate(Vector3.up * mouseX);
        
        // di chuyen
        float x  = Input.GetAxis("Horizontal"); // A D 
        float z = Input.GetAxis("Vertical"); // W S 
        Vector3 moveDirection = transform.right * x + transform.forward * z;
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        
    }
}
