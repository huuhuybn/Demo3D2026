# 🔦 HƯỚNG DẪN FPS & ĐÈN PIN (PHIÊN BẢN ĐƠN GIẢN NHẤT)

> **Mục tiêu**: Xoay chuột thì Camera xoay nhìn xung quanh, đèn pin tự động chiếu theo hướng nhìn, bấm phím `F` để bật/tắt đèn pin. Cực kỳ ngắn gọn, dễ hiểu và dễ làm!

---

## 💡 NGUYÊN LÝ CỐT LÕI

Trong Unity, bạn **không cần viết code xoay đèn pin phức tạp**. 

Chỉ cần đặt **`Flashlight` làm con của `Main Camera`**:
👉 Khi Camera xoay theo chuột, **đèn pin sẽ tự động quay và chiếu sáng theo hướng nhìn 100%!**

```
▼ Player (Thân người)
   └── Main Camera (Đầu & Mắt)       <-- Xoay theo chuột
        └── Flashlight (Spot Light)  <-- Tự động chiếu theo Camera
```

---

## BƯỚC 1: SẮP XẾP HIERARCHY TRONG UNITY

1. **Tạo Player**:
   * Chuột phải trong Hierarchy chọn `3D Object > Capsule`, đổi tên thành **`Player`**.
   * Xóa component `Capsule Collider` đi, bấm **`Add Component > Character Controller`**.
2. **Gắn Camera làm con của Player**:
   * Kéo `Main Camera` thả vào bên trong `Player`.
   * Chỉnh vị trí (Position) của `Main Camera`: **`X: 0, Y: 1.6, Z: 0`** (ngang tầm mắt).
3. **Gắn Đèn pin làm con của Main Camera**:
   * Chuột phải vào `Main Camera`, chọn **`Light > Spot Light`**, đổi tên thành **`Flashlight`**.
   * Chỉnh Position của `Flashlight`: **`X: 0, Y: 0, Z: 0`** (hoặc lệch nhẹ `X: 0.2` sang tay phải).
   * Chỉnh Rotation của `Flashlight`: **`X: 0, Y: 0, Z: 0`**.

---

## BƯỚC 2: CODE XOAY CHUỘT & DI CHUYỂN (FPSController)

Đoạn code siêu ngắn gọn xử lý cả **xoay chuột theo góc nhìn** và **di chuyển WASD**.

Tạo script C# tên là **`SimpleFPSController.cs`** gắn vào **`Player`**:

```csharp
using UnityEngine;

public class SimpleFPSController : MonoBehaviour
{
    [Header("--- GÓC NHÌN CHUỘT ---")]
    public Transform playerCamera;
    public float mouseSensitivity = 2f;
    private float xRotation = 0f;

    [Header("--- DI CHUYỂN ---")]
    public float moveSpeed = 5f;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Khóa con trỏ chuột vào giữa màn hình
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 1. XOAY CHUỘT
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Xoay đầu ngước lên / cúi xuống (giới hạn 90 độ tránh lộn ngược đầu)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Xoay thân người sang trái / phải
        transform.Rotate(Vector3.up * mouseX);

        // 2. DI CHUYỂN WASD
        float x = Input.GetAxis("Horizontal"); // A, D
        float z = Input.GetAxis("Vertical");   // W, S

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }
}
```

* 👉 **Cách gắn**: Chọn `Player`, kéo `Main Camera` vào ô **Player Camera** của script.

---

## BƯỚC 3: CODE BẬT / TẮT ĐÈN PIN BẰNG PHÍM F

Đoạn code siêu ngắn (chỉ 15 dòng) để bật/tắt đèn pin.

Tạo script C# tên là **`SimpleFlashlight.cs`** gắn vào chính GameObject **`Flashlight`**:

```csharp
using UnityEngine;

public class SimpleFlashlight : MonoBehaviour
{
    public Light flashlight;

    void Start()
    {
        if (flashlight == null) 
            flashlight = GetComponent<Light>();
    }

    void Update()
    {
        // Bấm phím F để bật hoặc tắt đèn
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}
```

---

## BƯỚC 4: THIẾT LẬP THÔNG SỐ SPOT LIGHT

Chọn GameObject **`Flashlight`**, trong Inspector chỉnh:
* **Type**: `Spot`
* **Range**: `15` đến `20` (tầm xa chiếu sáng)
* **Spot Angle**: `50` (độ mở rộng của chùm sáng)
* **Color**: Màu trắng ngà hoặc vàng nhạt
* **Intensity**: `2.5` đến `3.5` (độ sáng)
* **Shadows**: `Soft Shadows` (đổ bóng mềm mịn)

---

## 🎯 KẾT QUẢ

1. Bấm **Play**.
2. **Rê chuột**: Camera xoay nhìn khắp nơi $\rightarrow$ Đèn pin tự động chiếu sáng theo đúng hướng bạn đang nhìn!
3. **Bấm phím `W, A, S, D`**: Đi lại khám phá không gian.
4. **Bấm phím `F`**: Bật / Tắt đèn pin tức thì.
