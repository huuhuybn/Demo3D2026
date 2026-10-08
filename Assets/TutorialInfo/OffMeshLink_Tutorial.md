# 🌉 HƯỚNG DẪN CHI TIẾT: THIẾT LẬP OFF-MESH LINK

> **Mục tiêu**: Nắm vững cách sử dụng **Off-Mesh Link** để giúp AI (NavMeshAgent) di chuyển qua các địa hình đứt gãy, không liền mạch (ví dụ: nhảy qua hào, vượt cầu gãy, trèo thang, hoặc dịch chuyển tức thời).

---

## 🎯 Off-Mesh Link là gì?
Mặc định, `NavMeshAgent` chỉ có thể di chuyển trên các mặt phẳng liền mạch (khu vực màu xanh dương khi Bake). 
Nếu trên đường đi có một hố sâu (cây cầu bị gãy đôi), AI sẽ đứng khựng lại vì hết đường. **Off-Mesh Link** đóng vai trò là một "đường nối ma thuật", ghép 2 mép vực lại với nhau và cho AI biết rằng: *"Mày có thể bay/nhảy/dịch chuyển qua khoảng trống này!"*

---

## 🛠️ Hướng dẫn từng bước: Tạo cây cầu gãy

### Bước 1: Chuẩn bị môi trường (Mặt đất)
Để làm ví dụ, chúng ta cần 2 bờ đất bị cắt đứt.
1. Khởi tạo bờ A: Chuột phải vào cửa sổ **Hierarchy** -> `3D Object` -> `Cube` (hoặc `Plane`). Đặt tên là `Bo_A`. 
   - Thay đổi **Scale** (Ví dụ: `X=10, Y=1, Z=10`) để nó đủ rộng.
2. Khởi tạo bờ B: Tương tự, tạo một `Cube` khác tên là `Bo_B`. 
   - Đặt `Bo_B` cách `Bo_A` một khoảng trống (Ví dụ: dịch trục X ra xa khoảng 5 mét).
3. Đánh dấu vật thể đứng yên: 
   - Chọn cả `Bo_A` và `Bo_B`. Nhìn sang bảng **Inspector**, đánh dấu tích vào ô vuông **Static** (hoặc mở menu thả xuống và chọn `Navigation Static`).

### Bước 2: Nướng (Bake) NavMesh
AI không thể đi nếu chưa nướng (Bake) đường.
1. Mở cửa sổ Navigation: Lên thanh menu đỉnh màn hình chọn `Window` -> `AI` -> `Navigation`.
2. Chuyển sang tab **Bake** trong cửa sổ Navigation vừa mở.
3. Nhấn nút **Bake** ở dưới cùng.
   - *Kết quả:* Bạn sẽ thấy bề mặt của `Bo_A` và `Bo_B` được phủ một lớp màu xanh dương, nhưng ở giữa chúng hoàn toàn trống rỗng.

### Bước 3: Đặt điểm nhảy (Jump Nodes)
Off-Mesh Link cần 2 tọa độ (Start và End) để biết phải nối từ đâu sang đâu.
1. Chuột phải vào **Hierarchy** -> `Create Empty`. Đặt tên là `Diem_Xuat_Phat`.
   - Sử dụng công cụ Move (Phím `W`), kéo `Diem_Xuat_Phat` đặt ngay **sát mép vực** của `Bo_A`. Hãy đảm bảo điểm này nằm chạm lên bề mặt màu xanh dương của NavMesh.
2. Tương tự, tạo một GameObject rỗng nữa tên là `Diem_Dap_Xuong`.
   - Kéo `Diem_Dap_Xuong` đặt sát mép vực của `Bo_B`.

### Bước 4: Tạo Component NavMesh Link
Bây giờ ta sẽ nối 2 điểm trên lại.
1. Chuột phải vào **Hierarchy** -> `Create Empty`. Đặt tên là `Lien_Ket_Cau_Gay` (Để quản lý code cho gọn gàng).
2. Nhấn vào `Lien_Ket_Cau_Gay`, sang bảng **Inspector**, bấm nút **Add Component**.
3. Tìm và chọn Component **`NavMesh Link`** (hoặc `NavMeshLink`).
*(Lưu ý: Từ Unity 2022 trở đi với gói AI Navigation mới, Component cũ `Off Mesh Link` đã được thay thế bằng Component mới xịn hơn là `NavMesh Link`)*.
4. Thiết lập liên kết:
   - Trong Component vừa thêm, kéo thả `Diem_Xuat_Phat` từ Hierarchy vào ô **Start Transform** (hoặc Start Point).
   - Kéo thả `Diem_Dap_Xuong` từ Hierarchy vào ô **End Transform** (hoặc End Point).
   - Chỉnh **Width** bằng độ rộng của mặt cầu để nhiều AI có thể nhảy qua cùng lúc (thay vì xếp hàng 1).
5. Nhìn vào cửa sổ **Scene**: Bạn sẽ thấy có các vòng tròn và **đường chỉ nối thành hình vòng cung** nối giữa 2 bờ. Chúc mừng, liên kết đã thành công!

---

## ⚙️ Các tuỳ chọn nâng cao của Off-Mesh Link

Trong Component **Off Mesh Link** có vài cài đặt cực kỳ quan trọng:

| Thuộc Tính | Ý Nghĩa / Cách Dùng |
| :--- | :--- |
| **Bi-Directional** | *(Dịch: Hai chiều)*. Nếu tích chọn, AI có thể nhảy từ Bờ A sang Bờ B, **VÀ** ngược lại từ bờ B sang bờ A. Nếu bạn chỉ muốn AI nhảy một chiều (VD: trượt vách núi xuống, không trèo lên được), hãy **bỏ tích** ô này. |
| **Cost Override** | *(Dịch: Ghi đè chi phí).* Giá trị mặc định là `-1`. Nếu bạn đổi thành giá trị cao (VD: 50), AI sẽ ưu tiên tìm các con đường khác (đi đường vòng qua cầu xịn) thay vì đi qua đường Off-Mesh Link này vì "chi phí" nhảy qua hố quá đắt. |
| **Activated** | Bật / Tắt cây cầu. Bạn có thể dùng Code để tắt nút này (`offMeshLink.activated = false`), lúc đó AI sẽ không thể đi qua cầu gãy được nữa. Rất hữu ích cho các cơ chế giải đố (Kéo cần gạt để mở lối nhảy). |
| **Auto Update Positions** | Nếu 2 điểm Start và End của bạn là vật thể di động (Ví dụ con thuyền di chuyển), tích vào đây để đường link luôn tự động cập nhật nối theo thuyền. |

---

## 💻 Xử lý Off-Mesh Link bằng Script (Bonus)
Mặc định, khi AI đi qua Off-Mesh Link, nó sẽ "trượt" trên một đường thẳng hình vòng cung một cách khá đơ cứng. Nếu bạn muốn thêm Animation nhảy (Jump) lộn nhào, hãy kiểm tra trạng thái này trong code của con AI:

```csharp
using UnityEngine;
using UnityEngine.AI;

public class AgentJumpCheck : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Kiểm tra xem AI có đang sử dụng Off-Mesh Link (đang nhảy qua hố) không?
        if (agent.isOnOffMeshLink)
        {
            // TODO: Bật Animation nhảy lộn vòng tại đây
            // animator.SetTrigger("Jump");
        }
    }
}
```

> 🎯 **Bài tập nhỏ:** Bạn hãy thử tắt mục **Bi-Directional** và quan sát xem khi ra lệnh cho AI đi ngược lại từ bờ B sang bờ A, AI có tìm được đường không nhé!
