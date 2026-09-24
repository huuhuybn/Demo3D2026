using UnityEngine;

public class CubeContrller : MonoBehaviour
{
    public GameObject Enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 pos = transform.position;
        Vector3 forward = transform.forward; Debug.Log(forward); // hướng phía trước của mô hình : blue axis : z 
        Vector3 up = transform.up; Debug.Log(up);
        Vector3 right = transform.right; Debug.Log(right);
        Vector3 left = -transform.right; Debug.Log(left);
        Vector3 down = -transform.up; Debug.Log(down);
        
        // Phép cộng Vector 
        Vector3 A = new Vector3(2,1,0);
        Vector3 B = new Vector3(3,2,0);
        Vector3 C = A + B;
        // Ý nghĩa : nhân vật di chuyển từ A sau đó lại tới B 
        Vector3 movement = Vector3.forward + Vector3.right;
        // Phép trừ Vector 
        Vector3 playerPos = transform.position;
        Vector3 enemyPos = Enemy.transform.position;
        Vector3 direction = enemyPos - playerPos; // hướng từ Player -> Enemy 
        // target position - player position 
        
        // Dot Product .  
        // Vector3.Dot : Kiểm tra xem 2 vector đang cùng hướng, ngược hướng 
        // kiểm tra xem Enemy có ở phía trước của nhân vật hay không 
        // Cross Product : Tạo ra 1 vector mới : vuông góc với A và B 

        //Vector3 result = Vector3.Cross(transform.position, Enemy.transform.position);
        Vector3 result = Vector3.Cross(Vector3.right, Vector3.up);
        // forward 
        // xác định bên trái, bên phải 
        // quay hướng camera ... 
        // Quaternion : class // tránh lỗi : Gimbal Lock 
        transform.eulerAngles = new  Vector3(0, 90, 0); // xoay theo trục y 90 
        transform.rotation = Quaternion.Euler(0, 90, 0);
        // LookRotation : xoay nhân vật theo hướng 
        // Game Defender : tháp súng xoay theo nhân vật 
        // Distance : Khoảng cách giữa 2 điểm 
        float distance = Vector3.Distance(transform.position, Enemy.transform.position);
        
        // Lerp : Linear Interpolation 
        // di chuyen tuyen tinh 
        transform.rotation = Quaternion.Lerp(transform.rotation, Enemy.transform.rotation,
            Time.deltaTime * speed);
    }

    public float speed = 6f;
    // Update is called once per frame
    void Update()
    {
        /*Vector3 movement = Vector3.forward + Vector3.right;
        transform.position += movement * Time.deltaTime;*/
        //Vector3 direction = (Enemy.transform.position - transform.position).normalized ; // Chỉ chứa hướng 
        //transform.position += direction * speed *  Time.deltaTime;

        
        /*Vector3 directionToPlayer = (transform.position - Enemy.transform.position).normalized;
        float dot = Vector3.Dot(Enemy.transform.position, directionToPlayer);
        Debug.Log(dot);*/
        
        /*Vector3 direction = Enemy.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);*/
        
        // Vector3 Angle : Góc giữa 2 vector 
        /*Vector3 directionToPlayer = transform.position - Enemy.transform.position;
        float angle = Vector3.Angle(transform.forward,directionToPlayer);
        Debug.Log("Angle : " + angle);*/
    
        /*transform.position =
            Vector3.Lerp(transform.position, Enemy.transform.position, speed * Time.deltaTime);*/
   
    }
}
