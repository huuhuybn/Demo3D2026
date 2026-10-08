using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [Header("NavMesh Obstacles")]
    public GameObject obstacle;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Tạo 1 Raycast chiếu thì vị trí chuột để xác định vị trí rơi của vật cản
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 dropPosition = hit.point + new Vector3(0, 5f, 0); // Cong them do cao la 5
                Instantiate(obstacle, dropPosition, Quaternion.identity);
            }
        }
    }
}
