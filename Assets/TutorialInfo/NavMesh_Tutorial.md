# 🗺️ HƯỚNG DẪN SỬ DỤNG NAVMESH NÂNG CAO TRONG UNITY

> **Mục tiêu**: Bổ sung trạng thái di chuyển (Patrol/Chase) cho AI bằng `NavMeshAgent`, thiết lập cầu nhảy (Off-Mesh Link) và làm chủ các thuộc tính cơ bản.

---

## 📋 MỤC LỤC

1. [Phần 1: Bổ sung trạng thái Patrol và Chase cho AI](#phan-1)
2. [Phần 2: Thiết lập Off-Mesh Link (Cầu gãy)](#phan-2)
3. [Phần 3: Các thuộc tính quan trọng của NavMeshAgent](#phan-3)

---

<a id="phan-1"></a>
## 1. Bổ Sung Trạng Thái Patrol (Tuần Tra) Và Chase (Truy Đuổi)

Để AI có thể tự động đi tuần và rượt đuổi người chơi khi đến gần, chúng ta sẽ áp dụng mô hình Máy Trạng Thái (State Machine) bằng C#.

### Bước 1: Khai báo thư viện và trạng thái
Mở file Script điều khiển AI (ví dụ: `EnemyController.cs`), thêm thư viện AI và khai báo:

```csharp
using UnityEngine;
using UnityEngine.AI; // BẮT BUỘC: Thư viện để dùng NavMesh

public class EnemyController : MonoBehaviour
{
    // 1. Enum định nghĩa 2 trạng thái
    public enum EnemyState { Patrol, Chase }
    public EnemyState currentState = EnemyState.Patrol;

    private NavMeshAgent agent;

    [Header("Patrol Settings")]
    public Transform[] points;
    private int pointIndex = 0;

    [Header("Chase Settings")]
    public Transform playerTarget;
    public float chaseDistance = 10f; // Khoảng cách để phát hiện mục tiêu
```

### Bước 2: Thiết lập trong Start()
Lấy Component `NavMeshAgent` và gán mục tiêu tuần tra đầu tiên:

```csharp
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Tự động tìm tất cả điểm CheckPoints trong Scene
        GameObject CheckPoints = GameObject.FindGameObjectWithTag("CheckPoints");
        if(CheckPoints != null)
        {
            points = new Transform[CheckPoints.transform.childCount];
            for (int i = 0; i < CheckPoints.transform.childCount; i++)
            {
                points[i] = CheckPoints.transform.GetChild(i).transform;
            }
        }
        
        // Đi tới Waypoint đầu tiên
        if (points.Length > 0)
            agent.SetDestination(points[0].position);
    }
```

### Bước 3: Hàm Update xử lý State
Hàm `Update` chỉ dùng để gọi đúng hành vi tùy theo trạng thái:

```csharp
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                PatrolBehavior();
                break;
            case EnemyState.Chase:
                ChaseBehavior();
                break;
        }
    }
```

### Bước 4: Viết riêng chức năng cho từng State

```csharp
    private void PatrolBehavior()
    {
        // Kích hoạt Chase nếu Player lọt vào tầm nhìn
        if (playerTarget != null && Vector3.Distance(transform.position, playerTarget.position) < chaseDistance)
        {
            currentState = EnemyState.Chase;
            return;
        }

        // Nếu đi tuần tới nơi, lặp sang Waypoint kế tiếp
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            pointIndex++;
            if (pointIndex >= points.Length) pointIndex = 0; // Quay lại điểm số 0
            
            agent.SetDestination(points[pointIndex].position);
        }
    }

    private void ChaseBehavior()
    {
        if (playerTarget != null)
        {
            // Liên tục bám theo vị trí mới của Player
            agent.SetDestination(playerTarget.position);

            // Bỏ cuộc nếu Player chạy đi quá xa (x1.5 lần tầm nhìn)
            if (Vector3.Distance(transform.position, playerTarget.position) > chaseDistance * 1.5f)
            {
                currentState = EnemyState.Patrol;
                // Quay lại điểm Waypoint trước đó
                if(points.Length > 0) agent.SetDestination(points[pointIndex].position);
            }
        }
    }
}
```

> 💡 **Mẹo:** Đừng quên gắn script và Component `NavMeshAgent` vào quái, đồng thời kéo thả `Player` vào biến `playerTarget` nhé!

---

<a id="phan-2"></a>
## 2. Thiết Lập Off-Mesh Link (Xử lý cầu gãy, nhảy vực)

Off-Mesh Link giúp Agent băng qua khu vực địa hình không liền mạch (vực sâu, vách đá, mái nhà).

**Cách thao tác trên Unity Editor:**
1. **Chuẩn bị địa hình:**
   * Tạo 2 khối `Cube` (hoặc `Plane`) làm 2 bờ cầu gãy cách nhau một đoạn.
   * Đánh dấu chúng là `Navigation Static` và tiến hành Bake thành NavMesh (có màu xanh dương).
2. **Đánh dấu điểm nối:**
   * Tạo 1 GameObject rỗng (Empty) tên là `Diem_Start` đặt sát mép vực bên bờ 1.
   * Tạo 1 GameObject rỗng tên là `Diem_End` đặt sát mép vực bên bờ 2.
3. **Kết nối đường đi:**
   * Tạo thêm một GameObject rỗng tên là `Cau_Gay`.
   * Add Component: **`NavMesh Link`**. *(Lưu ý: trong các bản Unity mới sử dụng gói AI Navigation, component này tên là NavMesh Link thay vì Off Mesh Link)*.
   * Ở component vừa hiện ra, kéo `Diem_Start` vào ô **Start Transform**, và kéo `Diem_End` vào ô **End Transform**.
4. **Cấu hình tùy chọn:**
   * Tích chọn **Bi-directional (Hai chiều)** nếu muốn Agent có thể nhảy qua lại giữa cả 2 bờ.
   * *Kết quả:* Bạn sẽ thấy 2 mũi tên nối vòng cung giữa 2 điểm. Agent giờ đã biết tự nhảy qua khoảng trống!

---

<a id="phan-3"></a>
## 3. Các Thuộc Tính Quan Trọng Của NavMeshAgent

Khi tùy chỉnh `NavMeshAgent` trong Inspector, bạn cần lưu ý các thông số quan trọng sau:

### Tùy chỉnh điều hướng (Steering)
*   **`Speed`:** Tốc độ di chuyển của Agent. Dễ dàng thay đổi bằng code khi đuổi `Player` thì tăng lên, khi về tuần tra thì giảm xuống.
*   **`Angular Speed`:** Tốc độ quay đầu khi Agent cần đổi hướng (Độ/giây). Khoảng **120-200** là phù hợp để AI cua cong tự nhiên. Nếu để `360` trở lên, AI sẽ giật quay ngoắt một góc tức thì.
*   **`Stopping Distance`:** Khoảng cách để Agent hãm phanh lại trước khi giẫm trúng tâm mục tiêu. Nên để từ **0.5 - 1.5** để AI đứng trước mặt Player thay vì đè đùn đẩy Player.
*   **`Auto Braking`:** Tự động giảm tốc/phanh từ từ khi đến đích, rất tốt cho hành vi đi dạo tuần tra. Nếu làm game rượt đuổi tốc độ cao, có thể tắt đi.

### Tránh né vật cản (Obstacle Avoidance)
*   **`Radius / Height`:** Vùng không gian lưới xanh lá hiển thị quanh AI. Hãy chỉnh sao cho vừa khít với kích thước nhân vật (như cách chỉnh Capsule Collider).
*   **`Priority (Độ ưu tiên)`:** Thang đo từ `0 đến 99`.
    *   Sử dụng để tránh tình trạng "kẹt xe" khi có quá nhiều Enemy ở 1 chỗ.
    *   **Số càng nhỏ (vd: 0):** Độ ưu tiên cực cao. AI này sẽ cứ đi thẳng, các AI khác phải tự động dạt ra nhường đường (Thích hợp đặt cho Boss hoặc Xe Tank).
    *   **Số càng cao (vd: 99):** Ưu tiên thấp. Bắt buộc phải chủ động rẽ lách lượn tránh đám đông. 
