using UnityEngine;

public class AnimPlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float rotationSpeed = 8f;

    [Header("Jump")]
    public float jumpHeight = 2f;
    public float gravity = -20f;

    private Animator animator;
    private CharacterController characterController;
    private Vector3 verticalVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Move();
        HandleJump();
        HandleAttack();
        ApplyGravity();
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection = new Vector3(h, 0f, v);
        inputDirection = Vector3.ClampMagnitude(inputDirection, 1f);

        bool isMoving = inputDirection.sqrMagnitude > 0.01f;
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        // Chỉ chạy nhanh khi giữ Left Shift
        float speed = isRunning ? runSpeed : walkSpeed;

        if (isMoving)
        {
            // Xoay nhân vật theo hướng di chuyển
            Quaternion targetRotation =
                Quaternion.LookRotation(inputDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            // Di chuyển theo hướng input trong world space
            characterController.Move(
                inputDirection * speed * Time.deltaTime
            );
        }

        // Cập nhật animation
        if (animator != null)
        {
            float animationSpeed = 0f;

            if (isMoving)
            {
                animationSpeed = isRunning ? 1f : 0.3f;
            }

            animator.SetFloat(
                "Speed",
                animationSpeed,
                0.1f,
                Time.deltaTime
            );
        }
    }

    private void HandleJump()
    {
        if (characterController.isGrounded &&
            verticalVelocity.y < 0f)
        {
            verticalVelocity.y = -2f;
        }

        if (characterController.isGrounded &&
            Input.GetKeyDown(KeyCode.Space))
        {
            verticalVelocity.y = Mathf.Sqrt(
                jumpHeight * -2f * gravity
            );

            if (animator != null)
            {
                animator.SetTrigger("Jump");
            }
        }
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) &&
            animator != null)
        {
            animator.SetTrigger("Slash");
        }
    }

    private void ApplyGravity()
    {
        verticalVelocity.y += gravity * Time.deltaTime;

        characterController.Move(
            verticalVelocity * Time.deltaTime
        );
    }
}
