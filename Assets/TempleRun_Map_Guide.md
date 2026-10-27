# 🏃‍♂️ HƯỚNG DẪN TỪNG BƯỚC CHO NGƯỜI MỚI: LÀM GAME CHẠY TEMPLE RUN BẰNG HÌNH KHỐI 3D TRONG UNITY
*(Lộ trình thực hành: Tạo Player di chuyển trước ──► Tạo Đường đi tự động nối dài sau)*

> **Phương pháp học dễ hiểu nhất cho người mới**:
> Thay vì dựng cả đống đường phức tạp ngay từ đầu mà chưa có gì để chạy thử, chúng ta sẽ đi theo thứ tự trực quan nhất:
> 1. **BƯỚC 1 & 2**: Tạo **Nhân vật (Player)** + **Camera sau lưng** + **Code di chuyển** (Tự chạy thẳng, bấm `A/D` lách 3 làn Trái/Phải, bấm `Space` Nhảy, bấm `S` Trượt) và chạy thử ngay lập tức!
> 2. **BƯỚC 3 & 4**: Khi nhân vật đã chạy mượt rồi, chúng ta tiến hành **Dựng Đoạn Đường Mẫu (`Prefab`)** bằng hình khối 3D và viết **Code tự động nối đường vô tận (`RoadSpawner`)** trước mặt nhân vật!
> 3. **BƯỚC 5**: Nâng cấp thêm **Khúc cua rẽ góc 90 độ** và **Bẫy / Đồng xu**.

---

## 📋 MỤC LỤC THEO TỪNG BƯỚC THỰC HÀNH

| Bước | Nội dung thực hành | Thời gian |
|------|--------------------|-----------|
| **BƯỚC 1** | Dựng Nhân vật (`Player`), Mặt sàn nháp & Camera góc nhìn sau lưng | 5 phút |
| **BƯỚC 2** | Viết Code cho Player Di chuyển (Chạy, Đổi 3 làn, Nhảy, Trượt) & Camera bám đuôi | 10 phút |
| **BƯỚC 3** | Quy tắc Tọa độ & Dựng Đoạn Đường Chuẩn (`20 mét`) bằng hình khối `Cube`, `Cylinder` | 10 phút |
| **BƯỚC 4** | Đóng gói Đường thành `Prefab` & Viết Code Tự động nối đường vô tận (`RoadSpawner`) | 10 phút |
| **BƯỚC 5** | Làm chức năng Rẽ Góc 90 Độ ở Khúc Cua (`TurnZone`) & Tạo Địa hình Hố vực / Cầu hẹp | 10 phút |
| **BƯỚC 6** | Bảng tra cứu lỗi thường gặp của người mới & Cách sửa nhanh | Tra cứu |

---

# ═══════════════════════════════════════════
# BƯỚC 1: DỰNG NHÂN VẬT (PLAYER) & CAMERA TRÊN SCENE
# ═══════════════════════════════════════════

Trước tiên, hãy tạo một mặt sàn nháp và một nhân vật bằng hình viên thuốc (`Capsule`) để chúng ta có thứ nhìn thấy ngay trên màn hình.

## 1.1 Tạo Mặt sàn nháp (`TempGround`) để đứng thử
1. Mở Unity, nhìn sang cửa sổ **Hierarchy** (bên trái).
2. Click chuột phải vào khoảng trống $\rightarrow$ chọn **`3D Object -> Cube`** $\rightarrow$ Đổi tên thành **`TempGround`**.
3. Nhìn sang cửa sổ **Inspector** (bên phải), chỉnh thông số **Transform**:
   - **`Position`**: **`X = 0`**, **`Y = -0.5`**, **`Z = 40`**
   - **`Rotation`**: **`X = 0`**, **`Y = 0`**, **`Z = 0`**
   - **`Scale`**: **`X = 10`**, **`Y = 1`**, **`Z = 100`** *(Tạo tạm con đường rộng 10m, dài 100m để tập chạy)*.

---

## 1.2 Tạo Nhân vật (`Player`) chuẩn công nghiệp
> ⚠️ **Quy tắc cho người mới**: Không gắn trực tiếp code vào khối `Capsule` hình ảnh. Hãy tạo một **Object rỗng (`Create Empty`)** làm "Phần Hồn" (chứa hộp va chạm và code), rồi đặt khối `Capsule` làm "Phần Xác" (hình ảnh) ở bên trong!

```
📂 Player          <── Object Rỗng (Create Empty): Chứa CharacterController + Script di chuyển
 └── 💊 BodyVisual <── Khối 3D Capsule: Chỉ dùng để hiển thị hình dáng nhân vật
```

### Thao tác từng bước trên Hierarchy:
1. **Tạo Object Cha (`Player`)**:
   - Chuột phải vào khoảng trống trên Hierarchy $\rightarrow$ chọn **`Create Empty`** $\rightarrow$ Đổi tên thành **`Player`**.
   - Chỉnh **Transform** của `Player`:
     - **`Position`**: **`(0, 0, 0)`**
     - **`Rotation`**: **`(0, 0, 0)`**
     - **`Scale`**: **`(1, 1, 1)`**
   - Đổi **Tag** (ở ngay dưới tên `Player` bên Inspector) thành: **`Player`**.

2. **Gắn bộ điều khiển vật lý (`Character Controller`) cho `Player`**:
   - Vẫn đang chọn `Player`, nhìn sang **Inspector** bấm nút **`Add Component`**.
   - Gõ tìm và chọn **`Character Controller`**.
   - Sửa thông số của `Character Controller` như sau:
     - **`Center`**: **`X = 0`**, **`Y = 1`**, **`Z = 0`** *(Nâng tâm hộp va chạm lên `1m` để đáy chân nhân vật chạm đúng mặt sàn `Y = 0`!)*.
     - **`Radius`**: **`0.4`**
     - **`Height`**: **`2.0`**

3. **Tạo Hình dáng nhân vật (`BodyVisual`) bên trong `Player`**:
   - Trên Hierarchy, click chuột phải trực tiếp vào chữ **`Player`** $\rightarrow$ chọn **`3D Object -> Capsule`** $\rightarrow$ Đổi tên thành **`BodyVisual`**.
   - Chỉnh **Transform** của `BodyVisual`:
     - **`Position`**: **`(0, 1, 0)`** *(Để đáy viên thuốc nằm đúng tại chân `Player`)*
     - **`Rotation`**: **`(0, 0, 0)`**
     - **`Scale`**: **`(0.8, 1, 0.8)`**
   - **Xóa Collider thừa trên `BodyVisual`**: Nhìn sang Inspector của `BodyVisual`, tìm mục `Capsule Collider` $\rightarrow$ bấm vào dấu `⋮` (3 chấm góc phải) $\rightarrow$ chọn **`Remove Component`** *(Vì `Player` ở ngoài đã có `Character Controller` rồi)*.
   - *(Tuỳ chọn)* Tạo 1 Material màu xanh dương (`Chuột phải vào Project -> Create -> Material`) rồi kéo thả vào `BodyVisual` cho nổi bật.

---

## 1.3 Đặt góc nhìn cho `Main Camera` (Sau lưng nhân vật)
Không cần cài Cinemachine phức tạp! Bạn chỉ cần chọn **`Main Camera`** có sẵn trên Hierarchy và chỉnh **Transform**:
- **`Position`**: **`X = 0`**, **`Y = 4.5`**, **`Z = -6`** *(Cao hơn mặt đất `4.5m`, lùi về sau lưng nhân vật `6m`)*.
- **`Rotation`**: **`X = 20`**, **`Y = 0`**, **`Z = 0`** *(Hơi cúi ống kính xuống `20 độ` để nhìn rõ mặt đường)*.

---

# ═══════════════════════════════════════════
# BƯỚC 2: VIẾT CODE DI CHUYỂN PLAYER & CAMERA
# ═══════════════════════════════════════════

Bây giờ chúng ta sẽ viết 2 file code C# siêu gọn:
1. **`RunnerPlayer.cs`**: Cho nhân vật tự động chạy tới trước, bấm `A/D` lách qua lại 3 làn đường, bấm `Space` Nhảy, bấm `S` Cúi trượt.
2. **`SimpleRunnerCamera.cs`**: Cho Camera tự động lướt êm theo sau lưng nhân vật.

---

## 2.1 Giải thích nguyên lý "Đổi 3 Làn Trái / Giữa / Phải" cho người mới
Mặt đường của chúng ta rộng `10 mét` (từ `-5` sang `+5`), được chia làm **3 làn chạy** cách nhau **`3 mét`**:
- **Làn Trái (`currentLane = 0`)**: Tọa độ `X = -3`
- **Làn Giữa (`currentLane = 1`)**: Tọa độ `X = 0` *(Mặc định lúc bắt đầu)*
- **Làn Phải (`currentLane = 2`)**: Tọa độ `X = +3`

```
      Làn Trái (Số 0)          Làn Giữa (Số 1)          Làn Phải (Số 2)
          X = -3                   X = 0                    X = +3
            │                        │                        │
            │       ◄── Bấm A ──    🏃‍♂️    ── Bấm D ──►       │
```

Từ số thứ tự làn (`0, 1, 2`), công thức tính tọa độ `X` chỉ gồm 1 phép tính:
$$\text{targetX} = (\text{currentLane} - 1) \times 3$$

---

## 2.2 Tạo Script điều khiển Nhân vật (`RunnerPlayer.cs`)
1. Trong cửa sổ **Project** (`Assets`), click chuột phải $\rightarrow$ **`Create -> Scripting -> MonoBehaviour Script`** (hoặc `C# Script`).
2. Đặt tên chính xác là: **`RunnerPlayer`**.
3. Nháy đúp mở file lên, xóa hết code cũ và dán đoạn code có chú thích chi tiết sau vào:

```csharp
using System.Collections;
using UnityEngine;

/// <summary>
/// BƯỚC 2: Script điều khiển Nhân vật chạy kiểu Temple Run / Subway Surfers.
/// - Tự động chạy thẳng về phía trước (+Z).
/// - Bấm A / D (hoặc Mũi tên Trái / Phải) để lướt giữa 3 làn (-3, 0, +3).
/// - Bấm Space / W để Nhảy lên.
/// - Bấm S để Cúi trượt xuống.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class RunnerPlayer : MonoBehaviour
{
    [Header("1. Tốc độ Chạy thẳng")]
    public float forwardSpeed = 10f;        // Tốc độ chạy về phía trước (mét/giây)

    [Header("2. Chuyển 3 Làn đường (Trái - Giữa - Phải)")]
    public float laneDistance = 3.0f;       // Khoảng cách giữa 2 làn (3 mét)
    public float laneChangeSpeed = 12f;     // Tốc độ lướt ngang khi đổi làn
    private int currentLane = 1;            // 0 = Trái, 1 = Giữa (mặc định), 2 = Phải

    [Header("3. Nhảy (Jump) & Trọng lực")]
    public float jumpForce = 10f;           // Lực bật nhảy lên cao
    public float gravity = -25f;            // Trọng lực kéo nhân vật rơi xuống đất
    private float verticalVelocity = 0f;    // Vận tốc bay lên / rơi xuống hiện tại

    [Header("4. Trượt / Cúi người (Slide)")]
    public Transform bodyVisual;            // Kéo khối BodyVisual vào đây để thu nhỏ khi trượt
    private bool isSliding = false;

    // Tham chiếu tới bộ điều khiển vật lý
    private CharacterController controller;

    void Start()
    {
        // Lấy thành phần CharacterController đang gắn trên Player
        controller = GetComponent<CharacterController>();

        // Tự động tìm BodyVisual bên trong nếu bạn quên kéo vào Inspector
        if (bodyVisual == null && transform.childCount > 0)
        {
            bodyVisual = transform.GetChild(0);
        }
    }

    void Update()
    {
        // 1. Kiểm tra người chơi bấm phím Trái (A) / Phải (D) để đổi số thứ tự làn (0, 1, 2)
        ReadLaneInput();

        // 2. Kiểm tra người chơi bấm phím Nhảy (Space/W) hoặc Trượt (S)
        ReadJumpAndSlideInput();

        // 3. Thực hiện di chuyển nhân vật theo cả 3 hướng (X, Y, Z)
        MoveCharacter();
    }

    /// <summary>
    /// Bước 1: Đọc phím A/D để thay đổi biến currentLane (0: Trái, 1: Giữa, 2: Phải)
    /// </summary>
    private void ReadLaneInput()
    {
        // Bấm A hoặc Mũi tên Trái -> Giảm số làn đi 1 (nhưng không nhỏ hơn 0)
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentLane > 0)
            {
                currentLane--;
            }
        }

        // Bấm D hoặc Mũi tên Phải -> Tăng số làn lên 1 (nhưng không lớn hơn 2)
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentLane < 2)
            {
                currentLane++;
            }
        }
    }

    /// <summary>
    /// Bước 2: Đọc phím Nhảy (Space / W) và Trượt (S)
    /// </summary>
    private void ReadJumpAndSlideInput()
    {
        // Kiểm tra chân nhân vật có đang chạm mặt sàn hay không?
        if (controller.isGrounded)
        {
            // Giữ một lực ép nhẹ xuống sàn để nhân vật không bị bồng bềnh
            if (verticalVelocity < 0f)
            {
                verticalVelocity = -2f;
            }

            // Khi đang chạm đất mà bấm Space, W hoặc Mũi tên Lên -> Nhảy lên!
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            // Đang bay trên không mà bấm S (Xuống) -> Lao nhanh xuống đất!
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                verticalVelocity = -jumpForce * 1.5f;
            }
        }

        // Bấm S hoặc Mũi tên Xuống -> Cúi thấp người (Trượt) trong 0.8 giây
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !isSliding)
        {
            StartCoroutine(SlideCoroutine());
        }

        // Trọng lực luôn kéo nhân vật rơi xuống theo thời gian
        verticalVelocity += gravity * Time.deltaTime;
    }

    /// <summary>
    /// Bước 3: Tổng hợp vận tốc 3 trục (X, Y, Z) và cho nhân vật bước đi
    /// </summary>
    private void MoveCharacter()
    {
        // Tính tọa độ X đích đến của làn hiện tại: Làn 0 -> -3, Làn 1 -> 0, Làn 2 -> +3
        float targetX = (currentLane - 1) * laneDistance;

        // Tính vận tốc ngang (moveX) để kéo nhân vật lướt mượt từ vị trí hiện tại sang targetX
        float moveX = (targetX - transform.position.x) * laneChangeSpeed;

        // Ghép 3 vận tốc lại:
        // - Trục X: Lướt ngang đổi làn (moveX)
        // - Trục Y: Bay lên hoặc rơi xuống (verticalVelocity)
        // - Trục Z: Luôn tự động chạy thẳng về phía trước (forwardSpeed)
        Vector3 moveVector = new Vector3(moveX, verticalVelocity, forwardSpeed);

        // Ra lệnh cho CharacterController di chuyển
        controller.Move(moveVector * Time.deltaTime);
    }

    /// <summary>
    /// Hiệu ứng thu nhỏ chiều cao nhân vật một nửa trong 0.8 giây khi bấm Trượt (S)
    /// </summary>
    private IEnumerator SlideCoroutine()
    {
        isSliding = true;

        // Hạ thấp hộp va chạm xuống còn 1 mét (tâm ở 0.5m)
        controller.height = 1.0f;
        controller.center = new Vector3(0f, 0.5f, 0f);

        // Thu nhỏ hình viên thuốc để mắt thường nhìn thấy nhân vật cúi xuống
        if (bodyVisual != null)
        {
            bodyVisual.localScale = new Vector3(0.8f, 0.5f, 0.8f);
            bodyVisual.localPosition = new Vector3(0f, 0.5f, 0f);
        }

        // Đợi 0.8 giây
        yield return new WaitForSeconds(0.8f);

        // Đứng thẳng dậy về kích thước ban đầu (cao 2 mét, tâm ở 1.0m)
        controller.height = 2.0f;
        controller.center = new Vector3(0f, 1.0f, 0f);

        if (bodyVisual != null)
        {
            bodyVisual.localScale = new Vector3(0.8f, 1.0f, 0.8f);
            bodyVisual.localPosition = new Vector3(0f, 1.0f, 0f);
        }

        isSliding = false;
    }
}
```

---

## 2.3 Tạo Script cho Camera bám theo sau lưng (`SimpleRunnerCamera.cs`)
1. Tạo thêm 1 file C# Script tên là: **`SimpleRunnerCamera`**.
2. Dán đoạn code ngắn gọn sau vào:

```csharp
using UnityEngine;

/// <summary>
/// BƯỚC 2.3: Camera bám theo sau lưng Player mà KHÔNG cần Cinemachine.
/// Giữ độ cao ổn định để khi Player nhảy lên màn hình không bị giật!
/// </summary>
public class SimpleRunnerCamera : MonoBehaviour
{
    [Tooltip("Kéo đối tượng Player từ Hierarchy vào ô này")]
    public Transform player;

    [Tooltip("Khoảng cách từ Player tới Camera: Cao 4.5m, Lùi về sau 6m")]
    public Vector3 offset = new Vector3(0f, 4.5f, -6f);

    // Dùng LateUpdate để Camera luôn di chuyển SAU KHI Player đã bước xong trong khung hình đó
    void LateUpdate()
    {
        if (player == null) return;

        Vector3 newCamPos = transform.position;

        // 1. Trục Z (Sâu): Bám sát 100% theo lưng nhân vật
        newCamPos.z = player.position.z + offset.z;

        // 2. Trục Y (Cao): Giữ cố định ở độ cao 4.5m để khi nhảy màn hình vẫn êm ru
        newCamPos.y = offset.y;

        // 3. Trục X (Ngang): Lướt nhẹ theo 50% độ lệch làn của nhân vật
        newCamPos.x = Mathf.Lerp(transform.position.x, player.position.x * 0.5f, 5f * Time.deltaTime);

        transform.position = newCamPos;
    }
}
```

---

## 2.4 Gắn Code và Chạy Thử Ngay Bước 2!
1. Kéo file **`RunnerPlayer.cs`** thả vào đối tượng **`Player`** trên Hierarchy.
2. Kéo file **`SimpleRunnerCamera.cs`** thả vào đối tượng **`Main Camera`** trên Hierarchy.
3. Chọn **`Main Camera`**, nhìn sang Inspector: Kéo **`Player`** từ Hierarchy thả vào ô **`Player`** của `SimpleRunnerCamera`.
4. **Bấm nút ▶️ PLAY ở đỉnh màn hình Unity!**
   - **Thử nghiệm ngay**:
     - Nhân vật tự động lao về phía trước, Camera đi theo êm ru!
     - Bấm **`A` / `D`** (hoặc `⬅️ / ➡️`): Nhân vật lướt qua lại giữa 3 làn đường!
     - Bấm **`Space` / `W`**: Nhân vật bật nhảy lên cao!
     - Bấm **`S`**: Nhân vật thu nhỏ người trượt trên mặt sàn!

> 🎉 **Chúc mừng bạn!** Vậy là bạn đã hoàn thành xong phần Nhân vật di chuyển. Nhưng khi chạy hết `100 mét` của tấm sàn nháp `TempGround`, nhân vật sẽ rơi xuống vực! Giờ chúng ta sang **BƯỚC 3 & 4** để thay tấm sàn nháp bằng **Hệ thống Đường chạy tự động nối dài vô tận**!

---

# ═══════════════════════════════════════════
# BƯỚC 3: DỰNG ĐOẠN ĐƯỜNG CHUẨN (20 MÉT) BẰNG HÌNH KHỐI 3D
# ═══════════════════════════════════════════

Trước tiên, hãy **XÓA tấm sàn nháp `TempGround`** trên Hierarchy đi (Chọn `TempGround` $\rightarrow$ bấm `Delete`).

## 3.1 Ba quy tắc tọa độ giúp người mới ghép đường khít 100%
Để các đoạn đường dài **`20 mét`** xếp nối đuôi nhau không bị hở khe hay chồng lên nhau, bạn chỉ cần nhớ:
1. **Dùng `Create Empty` làm "Khung chứa" (`RoadTile`)**: Đặt tại `(0, 0, 0)` với `Scale = (1, 1, 1)`. Không bao giờ nhét vật con vào trong một khối `Cube` đã bị kéo giãn!
2. **Đặt tâm mặt đường (`Ground`) ở `Y = -0.5` và `Z = 10`**:
   - Mặt đường dày `1m` (`Scale.Y = 1`) $\rightarrow$ Dìm tâm xuống `Y = -0.5` thì **mặt trên của đường nằm phẳng đúng tại `Y = 0`** (khớp hoàn toàn với chân của `Player`!).
   - Mặt đường dài `20m` (`Scale.Z = 20`) $\rightarrow$ Đẩy tâm tiến lên `Z = 10` thì **mép đầu đoạn đường nằm đúng tại `Z = 0`** và **mép cuối nằm đúng tại `Z = 20`**!

```
Góc nhìn từ trên xuống (Top-Down View):
   Z = 20 (Mép cuối)   ──► ┌───────────────────────────┐
                           │          GROUND           │  ▲
   Z = 10 (Tâm Cube)   ──► │             •             │  │ Dài 20m (Scale.Z = 20)
                           │     Pos: (0, -0.5, 10)    │  ▼
   Z = 0  (Mép đầu)    ──► └─────────────◎─────────────┘
                           ◄─────5m──────▲──────5m─────►
                                         └── Tâm của RoadTile (0, 0, 0)
```

---

## 3.2 Thao tác dựng Đoạn đường chuẩn (`RoadTile_Straight`)

### 1. Tạo Khung chứa cha (`RoadTile_Straight`):
- Chuột phải vào khoảng trống trên Hierarchy $\rightarrow$ **`Create Empty`** $\rightarrow$ Đổi tên thành **`RoadTile_Straight`**.
- Chỉnh **Transform**: `Position = (0, 0, 0)`, `Rotation = (0, 0, 0)`, `Scale = (1, 1, 1)`.

### 2. Tạo Mặt đường (`Ground`):
- Chuột phải vào **`RoadTile_Straight`** $\rightarrow$ **`3D Object -> Cube`** $\rightarrow$ Đổi tên thành **`Ground`**.
- Chỉnh **Transform**:
  - **`Position`**: **`(0, -0.5, 10)`**
  - **`Scale`**: **`(10, 1, 20)`**

### 3. Tạo Tường chắn hai bên mép đường (`LeftWall` & `RightWall`):
- Chuột phải vào **`RoadTile_Straight`** $\rightarrow$ **`3D Object -> Cube`** $\rightarrow$ Đổi tên thành **`LeftWall`**.
  - **`Position`**: **`(-4.75, 0.5, 10)`**
  - **`Scale`**: **`(0.5, 1, 20)`**
- Bấm **`Ctrl + D`** nhân bản `LeftWall` $\rightarrow$ Đổi tên thành **`RightWall`**.
  - **`Position`**: **`(4.75, 0.5, 10)`** *(chỉ bỏ dấu trừ ở `X`)*.

### 4. Tạo Cổng cột đá cổ đại (`Pillar_L`, `Pillar_R`, `ArchBeam`):
- Chuột phải vào **`RoadTile_Straight`** $\rightarrow$ **`3D Object -> Cylinder`** $\rightarrow$ Đổi tên thành **`Pillar_L`**:
  - **`Position`**: **`(-4.75, 2, 10)`** | **`Scale`**: **`(0.8, 2, 0.8)`**
- Bấm **`Ctrl + D`** nhân bản thành **`Pillar_R`**:
  - **`Position`**: **`(4.75, 2, 10)`** | **`Scale`**: **`(0.8, 2, 0.8)`**
- Chuột phải vào **`RoadTile_Straight`** $\rightarrow$ **`3D Object -> Cube`** $\rightarrow$ Đổi tên thành **`ArchBeam`** (Đà ngang trên đỉnh cột):
  - **`Position`**: **`(0, 4.25, 10)`** | **`Scale`**: **`(10.5, 0.5, 1)`**

### 5. Tạo "Mốc Nối Đuôi" (`NextSpawnPoint`) ở cuối đoạn đường:
- Chuột phải vào **`RoadTile_Straight`** $\rightarrow$ chọn **`Create Empty`**.
- Đổi tên chính xác thành: **`NextSpawnPoint`**.
- Chỉnh **Transform**:
  - **`Position`**: **`(0, 0, 20)`** *(Đặt đúng ở vạch kết thúc 20m của đoạn đường!)*

---

# ═══════════════════════════════════════════
# BƯỚC 4: LƯU THÀNH PREFAB & CODE TỰ ĐỘNG NỐI ĐƯỜNG (`RoadSpawner`)
# ═══════════════════════════════════════════

## 4.1 Biến đoạn đường thành "Khuôn đúc" (`Prefab`)
1. Dưới cửa sổ **Project**, tạo thư mục **`Assets/Prefabs`** (nếu chưa có).
2. Giữ chuột trái kéo **`RoadTile_Straight`** từ cửa sổ **Hierarchy** thả vào thư mục **`Assets/Prefabs`**.
3. Sau khi thấy biểu tượng hộp màu xanh `RoadTile_Straight` đã xuất hiện trong `Assets/Prefabs`, hãy **XÓA `RoadTile_Straight` trên cửa sổ Hierarchy đi**!

---

## 4.2 Viết Script Tự Động Nối Đường Vô Tận (`RoadSpawner.cs`)
1. Trong cửa sổ **Project** (`Assets`), tạo một C# Script mới tên là: **`RoadSpawner`**.
2. Mở file lên và dán đoạn code sau vào:

```csharp
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BƯỚC 4: Script tự động đúc (Spawn) các đoạn đường nối đuôi nhau trước mặt Player
/// và tự động xóa đoạn đường cũ phía sau lưng.
/// </summary>
public class RoadSpawner : MonoBehaviour
{
    [Header("1. Danh sách các Khuôn đường (Prefabs)")]
    [Tooltip("Kéo RoadTile_Straight từ thư mục Prefabs vào Element 0")]
    public GameObject[] roadPrefabs;

    [Header("2. Số đoạn đường luôn xuất hiện cùng lúc")]
    public int tilesOnScreen = 6;       // 6 đoạn x 20m = luôn có 120m đường trước mặt

    [Header("3. Kéo nhân vật Player vào đây")]
    public Transform player;

    // Tọa độ và Góc xoay của điểm nối đuôi tiếp theo
    private Vector3 nextSpawnPos = Vector3.zero;
    private Quaternion nextSpawnRot = Quaternion.identity;

    // Hàng đợi lưu các đoạn đường đang có trên màn hình để xóa dần từ đoạn cũ nhất
    private Queue<GameObject> activeRoads = new Queue<GameObject>();

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        // Ngay khi bắt đầu game, xếp nối đuôi sẵn 6 đoạn đường đầu tiên!
        for (int i = 0; i < tilesOnScreen; i++)
        {
            // 2 đoạn đầu tiên luôn lấy Khuôn số 0 (Đường thẳng an toàn)
            if (i < 2)
                SpawnRoad(0);
            else
                SpawnRoad(Random.Range(0, roadPrefabs.Length));
        }
    }

    void Update()
    {
        if (player == null || roadPrefabs.Length == 0) return;

        // Tính khoảng cách từ vị trí hiện tại của Player tới điểm cuối cùng của con đường
        float distanceToEnd = Vector3.Distance(player.position, nextSpawnPos);

        // Nếu phía trước mặt Player chỉ còn dưới 90 mét đường -> Đúc thêm đoạn mới & Xóa đoạn cũ!
        if (distanceToEnd < (tilesOnScreen - 1.5f) * 20f)
        {
            int randomIndex = Random.Range(0, roadPrefabs.Length);
            SpawnRoad(randomIndex);
            DeleteOldestRoad();
        }
    }

    /// <summary>
    /// Đúc 1 đoạn đường mới tại mốc nối đuôi và tìm mốc NextSpawnPoint tiếp theo
    /// </summary>
    private void SpawnRoad(int prefabIndex)
    {
        // 1. Đúc đoạn đường mới tại vị trí nextSpawnPos và góc xoay nextSpawnRot
        GameObject newRoad = Instantiate(roadPrefabs[prefabIndex], nextSpawnPos, nextSpawnRot, transform);
        activeRoads.Enqueue(newRoad);

        // 2. Tìm cái mốc "NextSpawnPoint" nằm ở cuối đoạn đường vừa đúc
        Transform nextPoint = newRoad.transform.Find("NextSpawnPoint");
        if (nextPoint != null)
        {
            // Lưu lại vị trí và góc xoay của mốc đó cho đoạn đường kế tiếp!
            nextSpawnPos = nextPoint.position;
            nextSpawnRot = nextPoint.rotation;
        }
        else
        {
            // Dự phòng nếu quên tạo NextSpawnPoint: tự đẩy tới trước 20m
            nextSpawnPos += newRoad.transform.forward * 20f;
        }
    }

    /// <summary>
    /// Xóa đoạn đường cũ nhất ở phía sau lưng để giải phóng RAM
    /// </summary>
    private void DeleteOldestRoad()
    {
        if (activeRoads.Count > tilesOnScreen)
        {
            GameObject oldest = activeRoads.Dequeue();
            Destroy(oldest);
        }
    }
}
```

---

## 4.3 Kết nối `RoadSpawner` và Chạy Thử Ngay Bước 4!
1. Trên cửa sổ **Hierarchy**, click chuột phải $\rightarrow$ **`Create Empty`** $\rightarrow$ Đổi tên thành **`RoadSpawner`**.
2. Kéo script **`RoadSpawner.cs`** thả vào đối tượng `RoadSpawner`.
3. Nhìn sang **Inspector** của `RoadSpawner`:
   - Mở mục **`Road Prefabs`**, bấm dấu **`+`** thêm 1 phần tử (`Element 0`).
   - Kéo Prefab **`RoadTile_Straight`** từ thư mục **`Assets/Prefabs`** thả vào ô **`Element 0`**.
   - Kéo **`Player`** từ Hierarchy thả vào ô **`Player`**.
4. **Bấm nút ▶️ PLAY!**
   - Bây giờ nhân vật của bạn sẽ chạy trên con đường đền cổ đại được tự động nối dài mãi mãi về phía trước và tự động xóa ở phía sau lưng!

---

# ═══════════════════════════════════════════
# BƯỚC 5: TẠO THÊM BIẾN THỂ ĐƯỜNG (CẦU HẸP, HỐ VỰC & RÀO CHẮN)
# ═══════════════════════════════════════════

Để con đường phong phú đúng chất *Temple Run*, bạn chỉ cần nhân bản Prefab `RoadTile_Straight` trong thư mục `Assets/Prefabs` (Chọn `RoadTile_Straight` $\rightarrow$ bấm **`Ctrl + D`**) để tạo thêm 3 loại đường có thử thách sau, rồi kéo tất cả vào mảng `Road Prefabs` của `RoadSpawner`:

## 5.1 Biến thể 1: `RoadTile_Jump` (Đường có Rào chắn bắt buộc Nhảy)
1. Trong `Assets/Prefabs`, chọn `RoadTile_Straight` $\rightarrow$ bấm `Ctrl + D` $\rightarrow$ Đổi tên bản sao thành **`RoadTile_Jump`**.
2. **Nháy đúp chuột (Double-click)** vào `RoadTile_Jump` để mở chế độ chỉnh sửa Prefab.
3. Chuột phải vào `RoadTile_Jump` $\rightarrow$ **`3D Object -> Cube`** $\rightarrow$ Đổi tên thành **`Barrier`**.
   - **`Position`**: **`(0, 0.5, 12)`**
   - **`Scale`**: **`(9, 1, 0.5)`** *(Rào chắn cao `1m` nằm ngang đường ở vị trí `Z = 12`)*.
   - Tô màu Đỏ cho `Barrier` để dễ nhận biết.
4. Bấm mũi tên `<` ở góc trên bên trái Hierarchy để lưu và thoát chế độ sửa Prefab.

## 5.2 Biến thể 2: `RoadTile_Slide` (Đường có Cổng đá thấp bắt buộc Trượt `S`)
1. Nhân bản `RoadTile_Straight` thành **`RoadTile_Slide`**, nháy đúp mở ra.
2. Tạo 1 khối `Cube` con tên là **`LowGate`**:
   - **`Position`**: **`(0, 2.1, 12)`**
   - **`Scale`**: **`(9, 1.8, 1)`** *(Chừa khe trống cao `1.2m` sát mặt đất — đứng thẳng cao `2m` sẽ đụng đầu, bấm `S` trượt cao `1m` sẽ chui lọt!)*.

## 5.3 Biến thể 3: `RoadTile_NarrowBridge` (Cầu đá hẹp giữa vực sâu)
1. Nhân bản `RoadTile_Straight` thành **`RoadTile_NarrowBridge`**, nháy đúp mở ra.
2. Xóa `LeftWall`, `RightWall`, `Pillar_L`, `Pillar_R`, `ArchBeam`.
3. Chọn khối **`Ground`**, chỉnh **`Scale.X`** nhỏ lại từ `10` xuống **`3.2`**:
   - **`Scale`**: **`(3.2, 1, 20)`** *(Cầu chỉ rộng vừa đúng Làn Giữa `X = 0`, ai đang chạy ở Làn Trái/Phải mà không kịp bấm `A/D` vào làn giữa sẽ bước hụt chân rơi xuống vực!)*.

### Đưa cả 3 biến thể mới vào `RoadSpawner`:
- Chọn **`RoadSpawner`** trên Hierarchy.
- Kéo thêm `RoadTile_Jump`, `RoadTile_Slide`, `RoadTile_NarrowBridge` từ thư mục `Assets/Prefabs` thả vào mảng **`Road Prefabs`** (sao cho danh sách có 4 phần tử).
- Bấm **▶️ PLAY**: Con đường giờ đây sẽ tự động trộn ngẫu nhiên Đường thẳng, Rào nhảy, Cổng trượt và Cầu hẹp!

---

# ═══════════════════════════════════════════
# BƯỚC 6: BẢNG SỬA LỖI NHANH CHO NGƯỜI MỚI
# ═══════════════════════════════════════════

| Hiện tượng gặp phải | Nguyên nhân | Cách khắc phục trong 10 giây |
|---------------------|-------------|------------------------------|
| **1. Bấm Play thấy nhân vật rơi tuột xuyên qua mặt đường xuống vực** | - Vị trí ban đầu của `Player` bị thấp hơn mặt đường, hoặc quên gắn `Character Controller`. | - Chọn `Player`, đặt `Position = (0, 0.1, 2)` và kiểm tra `Character Controller` có **`Center = (0, 1, 0)`**, **`Height = 2`**. |
| **2. Các đoạn đường sinh ra đè chồng hết lên nhau tại một chỗ `(0,0,0)`** | - Quên tạo đối tượng con `NextSpawnPoint` bên trong Prefab đường, hoặc để `NextSpawnPoint` ở `(0,0,0)`. | - Nháy đúp mở Prefab đường, kiểm tra đối tượng con **`NextSpawnPoint`** phải có **`Position = (0, 0, 20)`**. |
| **3. Giữa các đoạn đường có khe hở hoặc chồng lên nhau một nửa** | - Khối `Ground` chưa đặt đúng `Position = (0, -0.5, 10)` và `Scale = (10, 1, 20)`. | - Mở Prefab đường, chọn `Ground` và nhập đúng `Position (0, -0.5, 10)`, `Scale (10, 1, 20)`. |
| **4. Camera không đi theo nhân vật** | - Chưa kéo `Player` vào ô `Player` của script `SimpleRunnerCamera` trên `Main Camera`. | - Chọn `Main Camera` $\rightarrow$ Kéo `Player` từ Hierarchy thả vào ô `Player` bên Inspector. |

---

## 📌 TÓM TẮT QUY TRÌNH 4 BƯỚC CỐT LÕI

- [ ] **Bước 1**: Tạo `Player` (`Create Empty` ở `0,0,0` + `CharacterController` có `Center Y = 1`, `Height = 2` + Con `BodyVisual` `Capsule` ở `0,1,0`).
- [ ] **Bước 2**: Gắn **`RunnerPlayer.cs`** vào `Player` và **`SimpleRunnerCamera.cs`** vào `Main Camera` $\rightarrow$ Chạy thử trên sàn nháp.
- [ ] **Bước 3**: Tạo `RoadTile_Straight` (`Create Empty` ở `0,0,0` + Con `Ground` ở `0, -0.5, 10`, `Scale 10, 1, 20` + Con `NextSpawnPoint` ở `0, 0, 20`).
- [ ] **Bước 4**: Kéo `RoadTile_Straight` vào `Assets/Prefabs`, xóa bản trên Hierarchy, rồi gắn **`RoadSpawner.cs`** để tự động nối đường vô tận!
