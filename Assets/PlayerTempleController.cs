using UnityEngine;

public class PlayerTempleController : MonoBehaviour
{
    public int currentLane = 1; // 0 : left , 1 : center , 2 : right 
    public float laneDistance = 2f;
    public float sideSpeed = 10f; // toc do chuyen lan 
    
    public float jumpForce = 10f;
    public float gravity = 20f;
    public float groundY;
    float verticalVelocity = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        groundY = transform.position.y; // vi tri ban dau khi chay game 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) if (currentLane >0) currentLane--;
        if (Input.GetKeyDown(KeyCode.D)) if (currentLane <2) currentLane++;
        float targetX = (currentLane - 1) * laneDistance;
        float newX = Mathf.Lerp(transform.position.x, targetX, Time.deltaTime * sideSpeed);
        
        bool isGrounded = transform.position.y <= groundY + 0.01f; // kiểm tra trạng thái chạm đất nếu đang ở khoảng vị trí Y ban đầu
        if (isGrounded)
        {
            verticalVelocity = 0f;
            if (Input.GetKeyDown(KeyCode.Space)) verticalVelocity = jumpForce; // neu dang cham đất thì cho phép nhảy lên
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime; // nếu ko chạm đất thì bị kéo xuống 
        }
        float newY = transform.position.y + verticalVelocity * Time.deltaTime;
        if (newY < groundY) newY = groundY; // kiểm tra chốt chặn ko cho rơi xuyên qua mặt đất 
        transform.position = new Vector3(newX, newY, transform.position.z);
        
    }
}
