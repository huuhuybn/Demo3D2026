# 🖱️ HƯỚNG DẪN: DI CHUYỂN BẰNG CLICK CHUỘT (KIỂU MOBA/RTS)

> **Mục tiêu**: Thay vì dùng bàn phím (WASD), chúng ta sẽ biến Player thành một `NavMeshAgent` và điều khiển bằng cách click chuột phải xuống đất, giống hệt cơ chế của Liên Minh Huyền Thoại (LOL) hay Diablo.

---

## 🛠️ Yêu cầu chuẩn bị
1. Mặt đất trong Scene của bạn đã được đánh dấu `Navigation Static` và đã được **Bake NavMesh** (hiện vùng xanh dương).
2. Character (Player) của bạn đã được gắn Component **`NavMesh Agent`**.
3. Camera trong game phải soi xuống đất (Góc nhìn chéo từ trên xuống như các game MOBA là đẹp nhất).

---

## 👨‍💻 Bước 1: Viết Script điều khiển bằng chuột

Bạn hãy tạo một Script C# mới (ví dụ đặt tên là `ClickToMove.cs`), mở lên và copy/paste đoạn code chuẩn mực dưới đây:

```csharp
using UnityEngine;
using UnityEngine.AI; // Cần thiết để gọi lệnh NavMesh

public class ClickToMove : MonoBehaviour
{
    private NavMeshAgent agent;

    [Header("Effects")]
    public GameObject clickEffectPrefab; // (Tuỳ chọn) Đưa Prefab hiệu ứng click vào đây

    void Start()
    {
        // Tự động lấy Component NavMeshAgent đã gắn trên Player
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // 1 (Left Click) | 0 (Right Click). Thường MOBA dùng chuột phải (số 1).
        if (Input.GetMouseButtonDown(1))
        {
            MoveToCursor();
        }
    }

    private void MoveToCursor()
    {
        // Tạo một tia Raycast bắn từ màn hình Camera đi xuyên qua vị trí con trỏ chuột
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Nếu tia Raycast chạm vào một vật thể bất kỳ có Collider (ví dụ: mặt đất)
        if (Physics.Raycast(ray, out hit))
        {
            // Ra lệnh cho NavMeshAgent chạy tới điểm vừa bị tia Raycast bắn trúng
            agent.SetDestination(hit.point);

            // (Tuỳ chọn) Sinh ra hiệu ứng đánh dấu vị trí click chuột
            if (clickEffectPrefab != null)
            {
                Instantiate(clickEffectPrefab, hit.point, Quaternion.identity);
            }
        }
    }
}
```

---

## 🎮 Bước 2: Thiết lập trên Editor

1. Chọn GameObject **Player** của bạn.
2. Đảm bảo Player đã có Component **`NavMesh Agent`**.
3. Kéo thả Script `ClickToMove.cs` vừa tạo vào Player.
4. Chạy Game (Play). Thử bấm chuột phải ra các điểm trên mặt đất, nhân vật của bạn sẽ tự động tìm đường lách qua chướng ngại vật để tới đúng vị trí đó!

---

## 💡 Giải thích cơ chế (Làm sao nó hoạt động?)

Cơ chế này kết hợp 2 kỹ thuật cốt lõi của không gian 3D:
* **Camera.main.ScreenPointToRay(Input.mousePosition):** Màn hình máy tính của chúng ta là mặt phẳng 2D. Hàm này dùng để "bắn" một tia laze ảo (Raycast) từ ống kính Camera góc nhìn người chơi, đâm thẳng tắp qua màn hình tại vị trí con trỏ chuột, lao vào không gian 3D của Game.
* **Physics.Raycast(ray, out hit):** Trả về `true` nếu tia laze chạm trúng một bề mặt có Collider (như khối mặt đất). Chữ `hit` sẽ lưu trữ toàn bộ thông tin của cú va chạm đó.
* **hit.point:** Đây là tọa độ (Vector3) chính xác trên không gian 3D nơi tia laze đập vào mặt đất. Chúng ta chỉ việc ném tọa độ này cho `agent.SetDestination()` là xong!

---

## ⚙️ Nâng cấp tiếp theo (Tuỳ chọn)
Để game cảm giác thật hơn giống LOL, bạn có thể:
1. Tạo một GameObject nhỏ xíu hình vòng tròn, gắn Particle System toả sáng. Lưu thành Prefab. Kéo nó vào ô `clickEffectPrefab` của Script. Cứ mỗi lần click là mặt đất sẽ chớp sáng.
2. Thêm LayerMask: Nếu bạn lỡ click trúng đầu một con quái vật, `hit.point` sẽ nằm ở trên đỉnh đầu nó khiến Player bị khựng lại. Bạn có thể sử dụng `LayerMask` trong hàm Raycast để tia laze chỉ được phép va chạm với Layer "Mặt đất" (`Ground`) mà xuyên qua mọi thứ khác.
