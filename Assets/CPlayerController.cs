using System;
using UnityEngine;

public class CPlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float rotationSpeed = 12f;
    public CharacterController characterController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(h, 0f, v).normalized;
        characterController.Move(movement * moveSpeed * Time.deltaTime);
    }

   
}
