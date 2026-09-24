# 📐 HƯỚNG DẪN TOÁN HỌC 3D TRONG UNITY

> **Mục tiêu**: Nắm vững Vector, Quaternion và các hàm toán học Unity để xử lý chuyển động & định hướng đối tượng trong game 3D.

---

## 📋 MỤC LỤC

| Phần | Nội dung |
|------|----------|
| 1 | Phép toán Vector (Cộng, Trừ, Dot, Cross) |
| 2 | Quaternion và phép quay trong Unity |
| 3 | Các hàm toán học: Lerp, Distance, Angle, LookRotation |
| 4 | Áp dụng vào chuyển động & định hướng |
| 5 | Demo Scripts (từng phần) |
| 6 | Mini Game tổng hợp: "Arrow Guardian" |
| 7 | Bảng tra cứu nhanh |

---

# ═══════════════════════════════════════════
# PHẦN 1: PHÉP TOÁN VECTOR
# ═══════════════════════════════════════════

## 1.1 Vector là gì?

**Vector** là một đại lượng có **hướng** và **độ lớn** (magnitude).

Trong Unity 3D, vector được biểu diễn bằng `Vector3(x, y, z)`:
- **x**: trục ngang (trái/phải)
- **y**: trục dọc (lên/xuống)  
- **z**: trục sâu (trước/sau)

```
        Y (lên)
        |
        |
        |________ X (phải)
       /
      /
     Z (trước - về phía camera)
```

### Các Vector đặc biệt trong Unity:
```csharp
Vector3.zero     = (0, 0, 0)    // Gốc tọa độ
Vector3.one      = (1, 1, 1)    // Vector đơn vị
Vector3.up       = (0, 1, 0)    // Hướng lên
Vector3.down     = (0, -1, 0)   // Hướng xuống
Vector3.left     = (-1, 0, 0)   // Hướng trái
Vector3.right    = (1, 0, 0)    // Hướng phải
Vector3.forward  = (0, 0, 1)    // Hướng trước
Vector3.back     = (0, 0, -1)   // Hướng sau
```

### Độ lớn (Magnitude):
```csharp
// Độ lớn = căn bậc 2 của (x² + y² + z²)
Vector3 v = new Vector3(3, 4, 0);
float mag = v.magnitude;  // = 5  (vì √(9+16+0) = 5)

// Normalized = vector cùng hướng nhưng độ lớn = 1
Vector3 dir = v.normalized; // = (0.6, 0.8, 0)
```

---

## 1.2 Phép Cộng Vector (+)

### 📖 Lý thuyết:
```
A + B = (Ax + Bx, Ay + By, Az + Bz)
```

### 🎯 Ý nghĩa hình học:
Cộng vector = **ghép nối** hai vector lại.
Đặt điểm đầu của vector B vào điểm cuối của vector A → kết quả là vector từ điểm đầu A đến điểm cuối B.

```
         B →
    A → ─────→ 
    ╱          ╲
   ╱    A + B   ╲
  ╱──────────────→
  (gốc)        (đích)
```

### 💡 Ứng dụng trong Game:

**1. Di chuyển đối tượng:**
```csharp
// Vị trí mới = Vị trí hiện tại + Hướng di chuyển × Tốc độ × Thời gian
Vector3 movement = new Vector3(inputX, 0, inputZ);
transform.position = transform.position + movement * speed * Time.deltaTime;

// Viết gọn:
transform.position += movement * speed * Time.deltaTime;
```

**2. Tính vị trí offset:**
```csharp
// Camera đặt phía sau và trên player
Vector3 cameraOffset = new Vector3(0, 5, -10);
camera.position = player.position + cameraOffset;
```

**3. Áp dụng lực:**
```csharp
// Bắn đạn: vị trí nòng súng + hướng bắn
Vector3 bulletSpawn = gunTip.position + gunTip.forward * 0.5f;
```

---

## 1.3 Phép Trừ Vector (−)

### 📖 Lý thuyết:
```
A - B = (Ax - Bx, Ay - By, Az - Bz)
```

### 🎯 Ý nghĩa hình học:
Trừ vector = **tìm hướng và khoảng cách** từ B đến A.
`A - B` cho ra vector **đi từ B hướng về A**.

```
    B ●─────────────→ A
         (A - B)
    
    Điểm B         Điểm A
    (3,0,0)        (7,0,0)
    
    A - B = (4, 0, 0)  ← hướng từ B đến A, khoảng cách = 4
```

### 💡 Ứng dụng trong Game:

**1. Tìm hướng từ đối tượng này đến đối tượng khác:**
```csharp
// Enemy nhìn về phía Player
Vector3 directionToPlayer = player.position - enemy.position;

// Chuẩn hóa để chỉ lấy HƯỚNG (độ lớn = 1)
Vector3 direction = directionToPlayer.normalized;

// Di chuyển enemy về phía player
enemy.position += direction * enemySpeed * Time.deltaTime;
```

**2. Kiểm tra khoảng cách (thủ công):**
```csharp
Vector3 diff = targetPos - myPos;
float distance = diff.magnitude; // Khoảng cách giữa 2 điểm
```

**3. Tính hướng bắn:**
```csharp
// Hướng từ súng đến mục tiêu
Vector3 aimDirection = (target.position - gun.position).normalized;
```

---

## 1.4 Tích Vô Hướng - Dot Product (·)

### 📖 Lý thuyết:
```
A · B = Ax×Bx + Ay×By + Az×Bz
A · B = |A| × |B| × cos(θ)    ← θ là góc giữa A và B
```

### 🎯 Ý nghĩa hình học:
Dot Product cho biết **mức độ cùng hướng** của 2 vector:

```
   Dot > 0          Dot = 0          Dot < 0
   ────────         ────────         ────────
     A↗               A↑               A↑
      ↗ B→              │ B→              │
   (cùng hướng)    (vuông góc)     (ngược hướng)
                                        ↓ B
                                        
 cos(0°) = 1     cos(90°) = 0     cos(180°) = -1
```

**Bảng tóm tắt giá trị Dot Product (khi A và B đã normalized):**

| Giá trị | Góc θ | Ý nghĩa |
|---------|-------|---------|
| 1.0 | 0° | Cùng hướng hoàn toàn |
| 0.7 | ~45° | Gần cùng hướng |
| 0.0 | 90° | Vuông góc |
| -0.7 | ~135° | Gần ngược hướng |
| -1.0 | 180° | Ngược hướng hoàn toàn |

### 💡 Ứng dụng trong Game:

**1. Kiểm tra đối tượng có ở PHÍA TRƯỚC không (FOV Check):**
```csharp
Vector3 toTarget = (target.position - transform.position).normalized;
Vector3 forward = transform.forward;

float dot = Vector3.Dot(forward, toTarget);

if (dot > 0)
{
    Debug.Log("Target ở PHÍA TRƯỚC!");
}
else
{
    Debug.Log("Target ở PHÍA SAU!");
}

// Kiểm tra trong tầm nhìn 60 độ (mỗi bên 30 độ)
// cos(30°) ≈ 0.866
if (dot > 0.866f)
{
    Debug.Log("Target trong tầm nhìn 60°!");
}
```

**2. Tính góc giữa 2 hướng:**
```csharp
float dot = Vector3.Dot(dirA.normalized, dirB.normalized);
float angleRad = Mathf.Acos(dot);          // Radian
float angleDeg = angleRad * Mathf.Rad2Deg; // Độ

// Hoặc dùng hàm có sẵn:
float angle = Vector3.Angle(dirA, dirB);
```

**3. Ánh sáng chiếu vào bề mặt (Lighting):**
```csharp
// Ánh sáng mạnh khi chiếu thẳng vào mặt (dot gần 1)
float lightIntensity = Mathf.Max(0, Vector3.Dot(surfaceNormal, lightDirection));
```

---

## 1.5 Tích Có Hướng - Cross Product (×)

### 📖 Lý thuyết:
```
A × B = (Ay×Bz - Az×By, Az×Bx - Ax×Bz, Ax×By - Ay×Bx)

|A × B| = |A| × |B| × sin(θ)
```

### 🎯 Ý nghĩa hình học:
Cross Product tạo ra **vector thứ 3 VUÔNG GÓC** với cả A và B.

```
         C = A × B
         ↑ (vuông góc lên)
         |
         |
    A ───┼───→ B
         |
    (mặt phẳng chứa A và B)
```

**Quy tắc bàn tay phải:**
- Ngón trỏ chỉ theo A
- Ngón giữa chỉ theo B  
- Ngón cái chỉ theo A × B

> ⚠️ **Lưu ý**: Cross Product KHÔNG có tính giao hoán!
> `A × B ≠ B × A` (ngược hướng nhau: `A × B = -(B × A)`)

### 💡 Ứng dụng trong Game:

**1. Xác định đối tượng ở BÊN TRÁI hay BÊN PHẢI:**
```csharp
Vector3 forward = transform.forward;
Vector3 toTarget = (target.position - transform.position).normalized;

Vector3 cross = Vector3.Cross(forward, toTarget);

if (cross.y > 0)
{
    Debug.Log("Target ở BÊN PHẢI → Quay phải!");
}
else if (cross.y < 0)
{
    Debug.Log("Target ở BÊN TRÁI → Quay trái!");
}
else
{
    Debug.Log("Target ở thẳng trước/sau!");
}
```

**2. Tính vector pháp tuyến bề mặt (Normal):**
```csharp
// Từ 3 điểm A, B, C trên mặt phẳng → tính normal
Vector3 AB = B - A;
Vector3 AC = C - A;
Vector3 normal = Vector3.Cross(AB, AC).normalized;
```

**3. Tạo hệ trục tọa độ cục bộ:**
```csharp
// Biết hướng forward và up → tính right
Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;
```

---

## 📝 Tổng kết Phần 1 - So sánh các phép toán Vector

| Phép toán | Input → Output | Kết quả | Ứng dụng chính |
|-----------|---------------|---------|----------------|
| **Cộng (+)** | Vector + Vector → Vector | Ghép nối, dịch chuyển | Di chuyển, offset |
| **Trừ (−)** | Vector − Vector → Vector | Hướng & khoảng cách | Tìm hướng đến mục tiêu |
| **Dot (·)** | Vector · Vector → **Số** | Mức độ cùng hướng | FOV, góc, ánh sáng |
| **Cross (×)** | Vector × Vector → **Vector** | Vector vuông góc | Trái/phải, normal |

---

# ═══════════════════════════════════════════
# PHẦN 2: QUATERNION VÀ PHÉP QUAY
# ═══════════════════════════════════════════

## 2.1 Vấn đề với góc Euler

**Euler Angles** biểu diễn phép quay bằng 3 góc (x, y, z):
```csharp
// Euler: xoay 45° quanh trục Y
transform.eulerAngles = new Vector3(0, 45, 0);
```

### ❌ Vấn đề Gimbal Lock:
Khi một trục quay trùng với trục khác (thường khi xoay X hoặc Z đạt ±90°), 
ta **mất đi 1 bậc tự do** → không thể xoay theo hướng mong muốn.

```
Bình thường:           Gimbal Lock:
   X──┐               X──┐
   Y──┤  (3 trục      Y──┤  (2 trục trùng nhau
   Z──┘   độc lập)    Z≡X┘   → mất 1 trục!)
```

→ **Giải pháp**: Sử dụng **Quaternion**.

---

## 2.2 Quaternion là gì?

**Quaternion** là một hệ thống số mở rộng biểu diễn phép quay bằng **4 thành phần**: `(x, y, z, w)`

```
q = w + xi + yj + zk

Trong đó:
- (x, y, z) = trục quay × sin(θ/2)   ← phần ảo
- w = cos(θ/2)                         ← phần thực
- θ = góc quay
```

### Hình dung đơn giản:
```
Quaternion = "Quay một GÓC quanh một TRỤC"

Ví dụ: Quay 90° quanh trục Y
         ↑ trục Y
         |  ╱
         | ╱ 90°
    ─────●─────→
         |
         |
         
q = Quaternion.AngleAxis(90, Vector3.up);
// Nội bộ: x=0, y=0.707, z=0, w=0.707
// Vì: sin(45°)=0.707, cos(45°)=0.707
```

### Tại sao dùng Quaternion?
| | Euler Angles | Quaternion |
|---|---|---|
| Dễ hiểu | ✅ Trực quan | ❌ Trừu tượng |
| Gimbal Lock | ❌ Bị | ✅ Không bị |
| Nội suy mượt | ❌ Khó | ✅ Slerp |
| Hiệu suất | ❌ Chậm hơn | ✅ Nhanh hơn |
| Kết hợp phép quay | ❌ Phức tạp | ✅ Đơn giản (nhân) |

---

## 2.3 Quaternion trong Unity

### Tạo Quaternion:
```csharp
// 1. Từ góc Euler (cách phổ biến nhất)
Quaternion rot = Quaternion.Euler(0, 90, 0); // Xoay 90° quanh Y

// 2. Từ trục + góc
Quaternion rot = Quaternion.AngleAxis(90, Vector3.up); // Cũng xoay 90° quanh Y

// 3. Không xoay (identity - mặc định)
Quaternion rot = Quaternion.identity; // (0, 0, 0, 1)

// 4. Xoay nhìn về một hướng
Quaternion rot = Quaternion.LookRotation(Vector3.forward); // Nhìn về phía trước
```

### Áp dụng Quaternion:
```csharp
// Gán rotation trực tiếp
transform.rotation = Quaternion.Euler(0, 90, 0);

// Đọc ngược lại góc Euler
Vector3 angles = transform.rotation.eulerAngles;

// ⚠️ KHÔNG BAO GIỜ gán trực tiếp quaternion.x/y/z/w
// ❌ Sai: transform.rotation.x = 0.5f;
// ✅ Đúng: transform.rotation = Quaternion.Euler(30, 0, 0);
```

### Kết hợp phép quay (Nhân Quaternion):
```csharp
// Xoay thêm 45° quanh Y so với rotation hiện tại
Quaternion additionalRot = Quaternion.Euler(0, 45, 0);
transform.rotation = transform.rotation * additionalRot;

// Viết gọn:
transform.Rotate(0, 45, 0);

// ⚠️ THỨ TỰ QUAN TRỌNG: A * B ≠ B * A
// A * B = áp dụng B trong local space của A
// B * A = áp dụng A trong local space của B
```

### Nội suy Quaternion (Slerp - Spherical Linear Interpolation):
```csharp
// Xoay MỘT từ rotA sang rotB, mượt mà
// t = 0 → rotA, t = 0.5 → giữa, t = 1 → rotB
Quaternion result = Quaternion.Slerp(rotA, rotB, t);

// Ví dụ: Xoay mượt về phía target
Quaternion targetRot = Quaternion.LookRotation(directionToTarget);
transform.rotation = Quaternion.Slerp(
    transform.rotation,   // Rotation hiện tại
    targetRot,            // Rotation mục tiêu
    rotateSpeed * Time.deltaTime  // Tốc độ (0→1)
);
```

---

## 2.4 Các hàm Quaternion hay dùng

```csharp
// ── Tạo ──
Quaternion.identity                        // Không xoay
Quaternion.Euler(x, y, z)                  // Từ góc Euler
Quaternion.AngleAxis(angle, axis)          // Từ trục + góc
Quaternion.LookRotation(forward)           // Nhìn theo hướng
Quaternion.LookRotation(forward, upwards)  // Nhìn theo hướng + up

// ── Nội suy ──
Quaternion.Slerp(a, b, t)      // Nội suy cầu (mượt, ổn định tốc độ)
Quaternion.Lerp(a, b, t)       // Nội suy tuyến tính (nhanh hơn, ít chính xác hơn)
Quaternion.RotateTowards(a, b, maxDegrees) // Xoay tối đa N độ mỗi frame

// ── Phép toán ──
q1 * q2                        // Kết hợp phép quay
q * Vector3                    // Xoay một vector theo quaternion
Quaternion.Inverse(q)          // Phép quay ngược lại
Quaternion.Angle(a, b)         // Góc giữa 2 rotation (độ)

// ── Đọc giá trị ──
q.eulerAngles                  // Chuyển về góc Euler
```

---

# ═══════════════════════════════════════════
# PHẦN 3: CÁC HÀM TOÁN HỌC UNITY
# ═══════════════════════════════════════════

## 3.1 Vector3.Lerp — Nội suy tuyến tính

### 📖 Lý thuyết:
```
Lerp(A, B, t) = A + (B - A) × t = A × (1 - t) + B × t

t = 0   → trả về A (điểm đầu)
t = 0.5 → trả về điểm giữa A và B
t = 1   → trả về B (điểm cuối)
```

### 🎯 Hình dung:
```
t=0      t=0.25    t=0.5     t=0.75    t=1
 A ────────●─────────●─────────●──────── B
(0,0,0)                              (10,0,0)
           (2.5,0,0) (5,0,0)  (7.5,0,0)
```

### 💡 Cách sử dụng:

**Cách 1: Di chuyển từ A đến B trong thời gian cố định (t tăng dần 0→1)**
```csharp
// Di chuyển từ startPos đến endPos trong 2 giây
float duration = 2f;
float elapsed = 0f;

void Update()
{
    elapsed += Time.deltaTime;
    float t = elapsed / duration;                     // 0 → 1
    t = Mathf.Clamp01(t);                             // Giới hạn 0-1
    transform.position = Vector3.Lerp(startPos, endPos, t);
}
```

**Cách 2: Di chuyển mượt kiểu "ease-out" (chậm dần)**
```csharp
// Mỗi frame đi 10% quãng đường còn lại → càng gần càng chậm
void Update()
{
    transform.position = Vector3.Lerp(
        transform.position,  // Vị trí HIỆN TẠI (thay đổi mỗi frame)
        targetPos,           // Đích
        0.1f                 // 10% mỗi frame
    );
}
```

```
Frame 1: ─────────────────────●───────────────────────── Target
                              ↑ đi 10% = di chuyển nhiều
Frame 5: ──────────────────────────────────●──────────── Target  
                                           ↑ 10% còn lại = ít hơn
Frame 10: ────────────────────────────────────────●───── Target
                                                  ↑ rất ít, gần đích
```

**Cách 3: Lerp màu sắc:**
```csharp
// Chuyển đổi màu từ đỏ sang xanh
Color color = Color.Lerp(Color.red, Color.blue, t);
renderer.material.color = color;
```

**Cách 4: Lerp giá trị số (Mathf.Lerp):**
```csharp
// Thanh máu giảm mượt
float displayHP = Mathf.Lerp(displayHP, actualHP, 5f * Time.deltaTime);
healthBar.fillAmount = displayHP / maxHP;
```

---

## 3.2 Vector3.Distance — Khoảng cách

### 📖 Lý thuyết:
```
Distance(A, B) = |B - A| = √((Bx-Ax)² + (By-Ay)² + (Bz-Az)²)
```

Thực chất `Vector3.Distance(A, B)` tương đương `(B - A).magnitude`.

### 💡 Ứng dụng:

**1. Kiểm tra phạm vi tấn công:**
```csharp
float attackRange = 5f;
float dist = Vector3.Distance(transform.position, enemy.position);

if (dist <= attackRange)
{
    Attack(enemy);
}
```

**2. Tìm enemy gần nhất:**
```csharp
Transform FindClosestEnemy(Transform[] enemies)
{
    Transform closest = null;
    float minDist = Mathf.Infinity;

    foreach (Transform enemy in enemies)
    {
        float dist = Vector3.Distance(transform.position, enemy.position);
        if (dist < minDist)
        {
            minDist = dist;
            closest = enemy;
        }
    }
    return closest;
}
```

**3. Trigger vùng (không cần Collider):**
```csharp
// Kích hoạt cutscene khi player đến gần NPC
float triggerDistance = 3f;
if (Vector3.Distance(player.position, npc.position) < triggerDistance)
{
    StartCutscene();
}
```

### ⚡ Mẹo tối ưu hiệu suất:
```csharp
// ❌ Chậm (tính căn bậc 2 mỗi frame)
if (Vector3.Distance(a, b) < 10f) { }

// ✅ Nhanh hơn (so sánh bình phương, bỏ qua căn bậc 2)
if ((b - a).sqrMagnitude < 100f) { }  // 100 = 10²
```

---

## 3.3 Vector3.Angle — Góc giữa 2 hướng

### 📖 Lý thuyết:
```
Angle(A, B) = arccos( (A·B) / (|A|×|B|) )
→ Trả về góc tính bằng ĐỘ (degrees), luôn trong khoảng [0, 180]
```

### 🎯 Hình dung:
```
          B
         ╱
        ╱ θ = Angle(A, B)
   ────●──────→ A
       Origin
```

> ⚠️ `Vector3.Angle` luôn trả về giá trị **dương** (0→180°).
> Nếu cần biết chiều quay (CW/CCW), dùng kết hợp với **Cross Product**.

### 💡 Ứng dụng:

**1. Kiểm tra tầm nhìn (Field of View):**
```csharp
float fovAngle = 60f; // Tổng góc nhìn = 60° (mỗi bên 30°)
Vector3 toTarget = (target.position - transform.position).normalized;
float angle = Vector3.Angle(transform.forward, toTarget);

if (angle < fovAngle / 2f) // So sánh nửa góc
{
    Debug.Log("Mục tiêu trong tầm nhìn!");
}
```

**2. Giới hạn góc xoay (tháp pháo):**
```csharp
float maxTurretAngle = 45f;
Vector3 dirToTarget = (target.position - turret.position).normalized;
float angle = Vector3.Angle(turret.parent.forward, dirToTarget);

if (angle <= maxTurretAngle)
{
    // Cho phép xoay tháp pháo về phía target
    turret.rotation = Quaternion.LookRotation(dirToTarget);
}
else
{
    Debug.Log("Mục tiêu ngoài tầm xoay!");
}
```

**3. Tính góc CÓ DẤU (Signed Angle - biết trái/phải):**
```csharp
// Vector3.SignedAngle trả về -180 đến +180
// Dương = theo chiều kim đồng hồ nhìn từ trên xuống (quanh trục up)
float signedAngle = Vector3.SignedAngle(
    transform.forward,
    toTarget,
    Vector3.up  // Trục tham chiếu
);

if (signedAngle > 0)
    Debug.Log($"Target ở bên PHẢI, góc = {signedAngle}°");
else
    Debug.Log($"Target ở bên TRÁI, góc = {signedAngle}°");
```

---

## 3.4 Quaternion.LookRotation — Xoay nhìn về mục tiêu

### 📖 Lý thuyết:
```
LookRotation(forward)          → Quaternion xoay để trục Z+ hướng theo "forward"
LookRotation(forward, upwards) → Quaternion xoay với trục Z+ theo "forward" 
                                  và trục Y+ hướng gần "upwards" nhất có thể
```

### 🎯 Hình dung:
```
Trước LookRotation:       Sau LookRotation:
                          
  Object ──→ (nhìn phải)    Object
                               ╲
                                ╲──→ Target
                                 (nhìn về target)
```

### 💡 Ứng dụng:

**1. Object nhìn về target ngay lập tức:**
```csharp
Vector3 direction = target.position - transform.position;
// Loại bỏ thành phần Y nếu chỉ muốn xoay ngang
direction.y = 0;

if (direction != Vector3.zero) // ⚠️ Tránh LookRotation với Vector zero!
{
    transform.rotation = Quaternion.LookRotation(direction);
}
```

**2. Object xoay MỘT về phía target (kết hợp Slerp):**
```csharp
float rotateSpeed = 5f;

Vector3 direction = (target.position - transform.position).normalized;
direction.y = 0; // Chỉ xoay trên mặt phẳng ngang

if (direction != Vector3.zero)
{
    Quaternion targetRotation = Quaternion.LookRotation(direction);
    transform.rotation = Quaternion.Slerp(
        transform.rotation,
        targetRotation,
        rotateSpeed * Time.deltaTime
    );
}
```

**3. Đạn bay theo hướng:**
```csharp
void FireBullet(Vector3 from, Vector3 to)
{
    Vector3 dir = (to - from).normalized;
    Quaternion bulletRotation = Quaternion.LookRotation(dir);
    Instantiate(bulletPrefab, from, bulletRotation);
}
```

**4. Camera nhìn theo player (có tùy chỉnh up):**
```csharp
// Camera luôn giữ "up" là Vector3.up (không bị nghiêng)
Vector3 lookDir = player.position - camera.position;
camera.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
```

---

## 📝 Tổng kết Phần 3

| Hàm | Input | Output | Mục đích |
|-----|-------|--------|----------|
| `Vector3.Lerp(a, b, t)` | 2 điểm + t(0→1) | Vector3 | Nội suy mượt giữa 2 điểm |
| `Vector3.Distance(a, b)` | 2 điểm | float | Khoảng cách giữa 2 điểm |
| `Vector3.Angle(a, b)` | 2 hướng | float (0→180°) | Góc giữa 2 hướng |
| `Quaternion.LookRotation(dir)` | Hướng nhìn | Quaternion | Tạo rotation nhìn theo hướng |

---

# ═══════════════════════════════════════════
# PHẦN 4: ÁP DỤNG VÀO CHUYỂN ĐỘNG & ĐỊNH HƯỚNG
# ═══════════════════════════════════════════

## 4.1 Mô hình chuyển động cơ bản

```csharp
// ═══ CÔNG THỨC GỐC ═══
// Vị trí mới = Vị trí cũ + Vận tốc × Thời gian
// position += velocity * deltaTime

public class BasicMovement : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        // 1. Lấy input
        float h = Input.GetAxis("Horizontal"); // -1 đến 1
        float v = Input.GetAxis("Vertical");   // -1 đến 1

        // 2. Tạo vector hướng di chuyển
        //    (Vector cộng: kết hợp 2 hướng)
        Vector3 moveDir = new Vector3(h, 0, v);

        // 3. Chuẩn hóa để di chuyển đều mọi hướng
        //    (Nếu không: đi chéo sẽ nhanh hơn √2 lần)
        if (moveDir.magnitude > 1f)
            moveDir.Normalize();

        // 4. Di chuyển (Vector cộng: position + direction)
        transform.position += moveDir * speed * Time.deltaTime;

        // 5. Xoay nhìn theo hướng di chuyển (LookRotation)
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, targetRot, 10f * Time.deltaTime
            );
        }
    }
}
```

## 4.2 Hệ thống phát hiện Enemy (Detection System)

```csharp
public class DetectionSystem : MonoBehaviour
{
    public float detectRange = 15f;   // Khoảng cách phát hiện
    public float fovAngle = 90f;      // Góc nhìn (Field of View)
    public Transform[] enemies;

    void Update()
    {
        foreach (Transform enemy in enemies)
        {
            if (enemy == null) continue;

            // ═══ BƯỚC 1: Kiểm tra KHOẢNG CÁCH (Distance) ═══
            float dist = Vector3.Distance(transform.position, enemy.position);
            if (dist > detectRange) continue; // Quá xa → bỏ qua

            // ═══ BƯỚC 2: Kiểm tra GÓC NHÌN (Dot Product / Angle) ═══
            Vector3 dirToEnemy = (enemy.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dirToEnemy);
            if (angle > fovAngle / 2f) continue; // Ngoài tầm nhìn → bỏ qua

            // ═══ BƯỚC 3: Xác định BÊN TRÁI/PHẢI (Cross Product) ═══
            Vector3 cross = Vector3.Cross(transform.forward, dirToEnemy);
            string side = cross.y > 0 ? "BÊN PHẢI" : "BÊN TRÁI";

            Debug.Log($"Phát hiện {enemy.name} ở {side}, " +
                      $"khoảng cách: {dist:F1}m, góc: {angle:F1}°");

            // ═══ BƯỚC 4: Xoay về phía enemy (LookRotation + Slerp) ═══
            Quaternion lookRot = Quaternion.LookRotation(dirToEnemy);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, lookRot, 3f * Time.deltaTime
            );
        }
    }

    // Vẽ tầm nhìn trong Scene View (để debug)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        // Vẽ vùng FOV
        Vector3 leftBound = Quaternion.Euler(0, -fovAngle / 2f, 0) * transform.forward;
        Vector3 rightBound = Quaternion.Euler(0, fovAngle / 2f, 0) * transform.forward;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, leftBound * detectRange);
        Gizmos.DrawRay(transform.position, rightBound * detectRange);
    }
}
```

## 4.3 Camera Follow mượt mà

```csharp
public class SmoothCameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 10, -8);
    public float smoothSpeed = 5f;
    public float lookSmoothSpeed = 8f;

    void LateUpdate()
    {
        // ═══ LERP: Di chuyển camera mượt ═══
        Vector3 desiredPos = target.position + offset;
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            smoothSpeed * Time.deltaTime
        );

        // ═══ LOOKROTATION + SLERP: Xoay camera nhìn player ═══
        Vector3 lookDir = target.position - transform.position;
        if (lookDir != Vector3.zero)
        {
            Quaternion desiredRot = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                desiredRot,
                lookSmoothSpeed * Time.deltaTime
            );
        }
    }
}
```

---

# ═══════════════════════════════════════════
# PHẦN 5: DEMO SCRIPTS
# ═══════════════════════════════════════════

> **Hướng dẫn**: Tạo từng script bên dưới, gắn vào Empty GameObject trong scene để demo.
> Mỗi script minh họa 1 nhóm kiến thức. Dùng khi giảng bài để sinh viên thấy trực quan.

## 5.1 Demo: Vector Operations Visualizer

```csharp
using UnityEngine;

/// <summary>
/// [DEMO] Trực quan hóa các phép toán Vector.
/// Gắn vào Empty GameObject. Tạo 2 object "PointA" và "PointB" trong scene.
/// Di chuyển PointA, PointB để thấy kết quả thay đổi real-time.
/// Bật Scene View để thấy Debug.DrawLine.
/// </summary>
public class VectorDemoVisualizer : MonoBehaviour
{
    [Header("Kéo 2 object vào đây")]
    public Transform pointA;
    public Transform pointB;

    [Header("Hiển thị phép toán")]
    public bool showAdd = true;
    public bool showSubtract = true;
    public bool showDotProduct = true;
    public bool showCrossProduct = true;

    void Update()
    {
        if (pointA == null || pointB == null) return;

        Vector3 A = pointA.position;
        Vector3 B = pointB.position;

        // ═══ PHÉP CỘNG: A + B ═══
        if (showAdd)
        {
            Vector3 sum = A + B;
            Debug.DrawLine(Vector3.zero, A, Color.red);       // Vector A (đỏ)
            Debug.DrawLine(Vector3.zero, B, Color.blue);      // Vector B (xanh)
            Debug.DrawLine(Vector3.zero, sum, Color.yellow);  // A+B (vàng)
            // Vẽ hình bình hành
            Debug.DrawLine(A, sum, Color.blue);  // B dịch đến đầu A
            Debug.DrawLine(B, sum, Color.red);   // A dịch đến đầu B
        }

        // ═══ PHÉP TRỪ: B - A (hướng từ A đến B) ═══
        if (showSubtract)
        {
            Vector3 AtoB = B - A;
            Debug.DrawRay(A, AtoB, Color.green); // Mũi tên từ A đến B (xanh lá)
        }

        // ═══ DOT PRODUCT ═══
        if (showDotProduct)
        {
            Vector3 dirA = A.normalized;
            Vector3 dirB = B.normalized;
            float dot = Vector3.Dot(dirA, dirB);
            float angle = Vector3.Angle(A, B);

            // Đổi màu theo dot: xanh(cùng hướng) → đỏ(ngược hướng)
            Color dotColor = dot > 0 ? Color.green : Color.red;
            Debug.DrawRay(Vector3.zero, dirA * 3, dotColor);
            Debug.DrawRay(Vector3.zero, dirB * 3, dotColor);
        }

        // ═══ CROSS PRODUCT ═══
        if (showCrossProduct)
        {
            Vector3 cross = Vector3.Cross(A, B);
            Debug.DrawRay(Vector3.zero, cross.normalized * 3, Color.magenta); // Tím
        }
    }
}
```

## 5.2 Demo: Quaternion Rotation

```csharp
using UnityEngine;

/// <summary>
/// [DEMO] Minh họa các cách xoay bằng Quaternion.
/// Gắn vào object cần xoay. Tạo 1 object "RotationTarget" để demo LookRotation.
/// Chọn DemoMode trong Inspector để chuyển giữa các demo.
/// </summary>
public class QuaternionDemo : MonoBehaviour
{
    [Header("Target để nhìn về")]
    public Transform lookTarget;

    [Header("Chọn chế độ demo")]
    public DemoMode mode = DemoMode.ContinuousRotate;

    [Header("Thông số")]
    public float rotateSpeed = 45f;    // Độ/giây
    public float slerpSpeed = 3f;

    public enum DemoMode
    {
        ContinuousRotate,   // Xoay liên tục quanh trục Y
        LookAtTarget,       // Nhìn về target ngay lập tức
        SlerpToTarget,      // Xoay mượt về target
        AngleAxisDemo,      // Demo xoay quanh trục tùy chỉnh
        EulerVsQuaternion   // So sánh Euler vs Quaternion
    }

    void Update()
    {
        switch (mode)
        {
            // ═══ Demo 1: Xoay liên tục ═══
            case DemoMode.ContinuousRotate:
                transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
                break;

            // ═══ Demo 2: Nhìn về target NGAY LẬP TỨC ═══
            case DemoMode.LookAtTarget:
                if (lookTarget != null)
                {
                    Vector3 dir = lookTarget.position - transform.position;
                    dir.y = 0;
                    if (dir != Vector3.zero)
                        transform.rotation = Quaternion.LookRotation(dir);
                }
                break;

            // ═══ Demo 3: Xoay MỘT về target (Slerp) ═══
            case DemoMode.SlerpToTarget:
                if (lookTarget != null)
                {
                    Vector3 dir = lookTarget.position - transform.position;
                    dir.y = 0;
                    if (dir != Vector3.zero)
                    {
                        Quaternion targetRot = Quaternion.LookRotation(dir);
                        transform.rotation = Quaternion.Slerp(
                            transform.rotation,
                            targetRot,
                            slerpSpeed * Time.deltaTime
                        );
                    }
                }
                break;

            // ═══ Demo 4: Xoay quanh trục nghiêng ═══
            case DemoMode.AngleAxisDemo:
                Vector3 customAxis = new Vector3(0, 1, 1).normalized;
                Quaternion rot = Quaternion.AngleAxis(
                    rotateSpeed * Time.deltaTime, customAxis
                );
                transform.rotation = transform.rotation * rot;
                Debug.DrawRay(transform.position, customAxis * 3, Color.cyan);
                break;

            // ═══ Demo 5: Gimbal Lock ═══
            case DemoMode.EulerVsQuaternion:
                float t = Mathf.PingPong(Time.time * 30f, 90f);
                transform.eulerAngles = new Vector3(t, t * 0.5f, t * 0.3f);
                break;
        }
    }

    // Vẽ các trục tọa độ local của object
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 2);  // Forward (Z)
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.right * 2);    // Right (X)
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.up * 2);       // Up (Y)
    }
}
```

## 5.3 Demo: Math Functions (Lerp, Distance, Angle, LookRotation)

```csharp
using UnityEngine;

/// <summary>
/// [DEMO] Minh họa Lerp, Distance, Angle, LookRotation.
/// Gắn vào object. Tạo "StartPoint", "EndPoint", "Target" trong scene.
/// Chọn MathDemo mode trong Inspector.
/// </summary>
public class MathFunctionsDemo : MonoBehaviour
{
    [Header("Các điểm tham chiếu")]
    public Transform startPoint;
    public Transform endPoint;
    public Transform target;

    [Header("Chọn chế độ demo")]
    public MathDemo mode = MathDemo.LerpPingPong;

    [Header("Thông số")]
    public float lerpDuration = 2f;
    public float rotateSpeed = 5f;

    private float elapsed = 0f;

    public enum MathDemo
    {
        LerpPingPong,       // Di chuyển qua lại giữa 2 điểm
        LerpEaseOut,        // Di chuyển ease-out đến target
        DistanceCheck,      // Hiển thị khoảng cách + đổi màu
        AngleDisplay,       // Hiển thị góc giữa forward và target
        LookRotationSmooth  // Xoay mượt nhìn về target
    }

    void Update()
    {
        switch (mode)
        {
            // ═══ Demo Lerp: Ping-Pong giữa 2 điểm ═══
            case MathDemo.LerpPingPong:
                if (startPoint && endPoint)
                {
                    elapsed += Time.deltaTime;
                    float t = Mathf.PingPong(elapsed / lerpDuration, 1f);
                    transform.position = Vector3.Lerp(
                        startPoint.position, endPoint.position, t
                    );
                    Debug.DrawLine(startPoint.position, endPoint.position, Color.gray);
                }
                break;

            // ═══ Demo Lerp: Ease-out (chậm dần) ═══
            case MathDemo.LerpEaseOut:
                if (target)
                {
                    transform.position = Vector3.Lerp(
                        transform.position, target.position,
                        3f * Time.deltaTime
                    );
                    Debug.DrawLine(transform.position, target.position, Color.yellow);
                }
                break;

            // ═══ Demo Distance: Đổi màu theo khoảng cách ═══
            case MathDemo.DistanceCheck:
                if (target)
                {
                    float dist = Vector3.Distance(transform.position, target.position);
                    Color lineColor;
                    if (dist < 3f) lineColor = Color.red;
                    else if (dist < 8f) lineColor = Color.yellow;
                    else lineColor = Color.green;

                    Debug.DrawLine(transform.position, target.position, lineColor);
                    Debug.Log($"Khoảng cách: {dist:F2}m");
                }
                break;

            // ═══ Demo Angle: Hiển thị góc ═══
            case MathDemo.AngleDisplay:
                if (target)
                {
                    Vector3 toTarget = (target.position - transform.position).normalized;
                    float angle = Vector3.Angle(transform.forward, toTarget);
                    float signedAngle = Vector3.SignedAngle(
                        transform.forward, toTarget, Vector3.up
                    );

                    Debug.DrawRay(transform.position, transform.forward * 5, Color.blue);
                    Debug.DrawRay(transform.position, toTarget * 5, Color.red);

                    string side = signedAngle > 0 ? "Phải" : "Trái";
                    Debug.Log($"Góc: {angle:F1}° | Hướng: {side} ({signedAngle:F1}°)");
                }
                break;

            // ═══ Demo LookRotation: Xoay mượt ═══
            case MathDemo.LookRotationSmooth:
                if (target)
                {
                    Vector3 dir = (target.position - transform.position);
                    dir.y = 0;
                    if (dir != Vector3.zero)
                    {
                        Quaternion lookRot = Quaternion.LookRotation(dir);
                        transform.rotation = Quaternion.Slerp(
                            transform.rotation, lookRot,
                            rotateSpeed * Time.deltaTime
                        );
                    }
                    Debug.DrawLine(transform.position, target.position, Color.magenta);
                }
                break;
        }
    }
}
```

---

# ═══════════════════════════════════════════
# PHẦN 6: MINI GAME - "ARROW GUARDIAN"
# ═══════════════════════════════════════════

> **Concept**: Player điều khiển 1 nhân vật (Capsule), tự động xoay nhìn về enemy gần nhất,
> bắn khi enemy vào tầm. Enemies (Spheres) spawn xung quanh và tiến về player.
> UI hiển thị real-time các giá trị toán học để học viên quan sát.

## 6.1 Setup Scene

```
Hướng dẫn setup trong Unity:

1. Tạo Scene mới: File → New Scene → Basic (Built-in)

2. Tạo Player:
   - GameObject → 3D Object → Capsule
   - Đặt tên: "Player", Tag: "Player"
   - Position: (0, 1, 0)
   - Thêm Rigidbody (Freeze Rotation X, Y, Z)
   - Gắn script: ArrowGuardian.cs

3. Tạo Ground:
   - GameObject → 3D Object → Plane
   - Scale: (5, 1, 5) ← sân 50×50 units

4. Tạo Enemy Prefab:
   - GameObject → 3D Object → Sphere
   - Đặt tên: "Enemy", Tag: "Enemy"  (thêm tag mới trong Project Settings)
   - Thêm Rigidbody
   - Gắn script: EnemyAI.cs
   - Đổi material màu đỏ
   - Kéo vào Prefabs folder → thành Prefab → Xóa trong scene

5. Tạo Bullet Prefab:
   - GameObject → 3D Object → Sphere
   - Scale: (0.2, 0.2, 0.2)
   - Đặt tên: "Bullet", Tag: "Bullet"  (thêm tag mới)
   - Thêm Rigidbody
   - Đổi material màu vàng
   - Kéo vào Prefabs folder → thành Prefab → Xóa trong scene

6. Tạo Spawn Manager:
   - GameObject → Create Empty → đặt tên "SpawnManager"
   - Gắn script: EnemySpawner.cs
   - Kéo Enemy Prefab vào field

7. Tạo Game Manager:
   - GameObject → Create Empty → đặt tên "GameManager"
   - Gắn script: GameManager_MathGame.cs

8. Tạo UI:
   - GameObject → UI → Canvas
   - Gắn script: MathDebugUI.cs
   - Tạo các Text (Legacy hoặc TextMeshPro):
     - "ScoreText" (góc trên trái)
     - "DebugText" (góc dưới trái, font nhỏ)
     - "GameOverPanel" (ẩn mặc định, chứa text "GAME OVER - Nhấn R")

9. Camera:
   - Gắn script CameraFollow.cs vào Main Camera
   - Kéo Player vào target field
```

## 6.2 ArrowGuardian.cs — Player Controller

```csharp
using UnityEngine;

/// <summary>
/// MINI GAME - Player Controller
/// 
/// KIẾN THỨC TOÁN HỌC SỬ DỤNG:
/// ─────────────────────────────
/// • Vector Cộng (+)     → Di chuyển: position += direction * speed * dt
/// • Vector Trừ (−)      → Tìm hướng đến enemy: enemy.pos - player.pos
/// • Dot Product          → Kiểm tra enemy phía trước (dot > 0) + FOV check
/// • Cross Product        → Xác định enemy ở bên trái/phải (cross.y)
/// • Distance             → Kiểm tra tầm bắn
/// • Angle                → Kiểm tra FOV (góc nhìn)
/// • LookRotation         → Xoay player nhìn về enemy
/// • Slerp                → Xoay mượt (không giật)
/// </summary>
public class ArrowGuardian : MonoBehaviour
{
    [Header("=== DI CHUYỂN (Vector Cộng) ===")]
    public float moveSpeed = 8f;

    [Header("=== XOAY (LookRotation + Slerp) ===")]
    public float rotateSpeed = 10f;

    [Header("=== BẮN (Vector Trừ + LookRotation) ===")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public float fireRate = 0.3f;
    public float fireRange = 12f;

    [Header("=== PHÁT HIỆN (Distance + Dot + Cross) ===")]
    public float detectRange = 20f;
    public float fovAngle = 120f;

    // Trạng thái nội bộ
    private float nextFireTime;
    private Transform closestEnemy;
    private Rigidbody rb;

    // ── Thông tin debug (để MathDebugUI đọc) ──
    [HideInInspector] public float debugDistance;
    [HideInInspector] public float debugAngle;
    [HideInInspector] public float debugDot;
    [HideInInspector] public Vector3 debugCross;
    [HideInInspector] public string debugSide = "---";
    [HideInInspector] public bool debugInFOV;
    [HideInInspector] public bool debugInRange;
    [HideInInspector] public int score;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleMovement();
        FindClosestEnemy();
        HandleRotation();
        HandleShooting();
    }

    // ─────────────────────────────────────────────
    // DI CHUYỂN - Sử dụng VECTOR CỘNG
    // position += direction * speed * deltaTime
    // ─────────────────────────────────────────────
    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal"); // -1 đến 1
        float v = Input.GetAxis("Vertical");   // -1 đến 1

        // VECTOR CỘNG: Kết hợp 2 hướng thành hướng di chuyển
        Vector3 moveDir = new Vector3(h, 0, v);

        // Chuẩn hóa: đi chéo không nhanh hơn đi thẳng
        if (moveDir.magnitude > 1f)
            moveDir.Normalize();

        // VECTOR CỘNG: Vị trí mới = Vị trí hiện tại + hướng × tốc độ × thời gian
        Vector3 newPos = rb.position + moveDir * moveSpeed * Time.deltaTime;
        rb.MovePosition(newPos);
    }

    // ─────────────────────────────────────────────
    // TÌM ENEMY GẦN NHẤT - Sử dụng DISTANCE + DOT + CROSS + ANGLE
    // ─────────────────────────────────────────────
    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDist = Mathf.Infinity;
        closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            // DISTANCE: Tính khoảng cách đến enemy
            float dist = Vector3.Distance(transform.position, enemy.transform.position);

            if (dist < detectRange && dist < minDist)
            {
                minDist = dist;
                closestEnemy = enemy.transform;
            }
        }

        // ── Cập nhật debug info ──
        if (closestEnemy != null)
        {
            // VECTOR TRỪ: Tìm hướng từ player đến enemy
            Vector3 toEnemy = (closestEnemy.position - transform.position).normalized;

            // DOT PRODUCT: Mức độ cùng hướng (-1 đến 1)
            debugDot = Vector3.Dot(transform.forward, toEnemy);

            // CROSS PRODUCT: Xác định enemy ở bên trái/phải
            debugCross = Vector3.Cross(transform.forward, toEnemy);
            debugSide = debugCross.y > 0.01f ? "BÊN PHẢI ►" :
                        debugCross.y < -0.01f ? "◄ BÊN TRÁI" : "THẲNG TRƯỚC";

            // ANGLE: Góc giữa hướng nhìn và hướng đến enemy
            debugAngle = Vector3.Angle(transform.forward, toEnemy);
            debugDistance = minDist;
            debugInFOV = debugAngle < fovAngle / 2f;
            debugInRange = minDist <= fireRange;
        }
        else
        {
            debugDistance = 0;
            debugAngle = 0;
            debugDot = 0;
            debugCross = Vector3.zero;
            debugSide = "---";
            debugInFOV = false;
            debugInRange = false;
        }
    }

    // ─────────────────────────────────────────────
    // XOAY - Sử dụng LOOKROTATION + SLERP
    // ─────────────────────────────────────────────
    void HandleRotation()
    {
        if (closestEnemy == null) return;

        // VECTOR TRỪ: Hướng từ player đến enemy
        Vector3 direction = closestEnemy.position - transform.position;
        direction.y = 0;

        if (direction == Vector3.zero) return;

        // LOOKROTATION: Tạo Quaternion xoay nhìn về enemy
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // SLERP: Xoay mượt (không giật)
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotateSpeed * Time.deltaTime
        );
    }

    // ─────────────────────────────────────────────
    // BẮN - Sử dụng VECTOR TRỪ + LOOKROTATION + DISTANCE + DOT
    // ─────────────────────────────────────────────
    void HandleShooting()
    {
        if (closestEnemy == null) return;
        if (Time.time < nextFireTime) return;

        // DISTANCE: Chỉ bắn khi enemy trong tầm
        float dist = Vector3.Distance(transform.position, closestEnemy.position);
        if (dist > fireRange) return;

        // VECTOR TRỪ + NORMALIZE: Hướng đến enemy
        Vector3 toEnemy = (closestEnemy.position - transform.position).normalized;

        // ANGLE: Chỉ bắn khi enemy trong tầm nhìn
        float angle = Vector3.Angle(transform.forward, toEnemy);
        if (angle > fovAngle / 2f) return;

        // DOT PRODUCT: Chỉ bắn khi đang nhìn khá thẳng về enemy
        float dot = Vector3.Dot(transform.forward, toEnemy);
        if (dot < 0.8f) return; // cos(~37°) ≈ 0.8

        // ═══ BẮN ĐẠN ═══
        // VECTOR CỘNG: Vị trí spawn đạn = player + offset phía trước
        Vector3 spawnPos = transform.position + transform.forward * 1f
                           + Vector3.up * 0.5f;

        // LOOKROTATION: Đạn xoay theo hướng bắn
        Quaternion bulletRot = Quaternion.LookRotation(toEnemy);
        GameObject bullet = Object.Instantiate(bulletPrefab, spawnPos, bulletRot);

        // VECTOR NHÂN SCALAR: Vận tốc = hướng × tốc độ
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.useGravity = false;
            bulletRb.linearVelocity = toEnemy * bulletSpeed;
        }

        Destroy(bullet, 3f);
        nextFireTime = Time.time + fireRate;
    }

    // Vẽ debug trong Scene View
    void OnDrawGizmosSelected()
    {
        // Vòng tròn phát hiện (vàng)
        Gizmos.color = new Color(1, 1, 0, 0.15f);
        Gizmos.DrawWireSphere(transform.position, detectRange);

        // Vòng tròn tầm bắn (đỏ)
        Gizmos.color = new Color(1, 0, 0, 0.25f);
        Gizmos.DrawWireSphere(transform.position, fireRange);

        // Hai cạnh FOV (xanh lá)
        Vector3 leftFOV = Quaternion.Euler(0, -fovAngle / 2f, 0) * transform.forward;
        Vector3 rightFOV = Quaternion.Euler(0, fovAngle / 2f, 0) * transform.forward;
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, leftFOV * detectRange);
        Gizmos.DrawRay(transform.position, rightFOV * detectRange);
    }
}
```

## 6.3 EnemyAI.cs — Enemy di chuyển về phía Player

```csharp
using UnityEngine;

/// <summary>
/// MINI GAME - Enemy AI
/// 
/// KIẾN THỨC TOÁN HỌC:
/// • Vector Trừ       → Tìm hướng đến player
/// • Normalize        → Chỉ lấy hướng, bỏ khoảng cách
/// • Vector Cộng      → Di chuyển position += dir * speed * dt
/// • LookRotation     → Xoay nhìn về player
/// • Slerp            → Xoay mượt
/// </summary>
public class EnemyAI : MonoBehaviour
{
    public float speed = 3f;
    public float rotateSpeed = 5f;
    public int damage = 1;

    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return;

        // VECTOR TRỪ: Hướng từ enemy đến player
        Vector3 direction = player.position - transform.position;
        direction.y = 0;

        // NORMALIZE: Chỉ lấy hướng, độ lớn = 1
        Vector3 moveDir = direction.normalized;

        // VECTOR CỘNG: Di chuyển về phía player
        transform.position += moveDir * speed * Time.deltaTime;

        // LOOKROTATION + SLERP: Xoay mượt nhìn về player
        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, targetRot,
                rotateSpeed * Time.deltaTime
            );
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var gm = Object.FindFirstObjectByType<GameManager_MathGame>();
            if (gm != null) gm.PlayerTakeDamage(damage);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            var gm = Object.FindFirstObjectByType<GameManager_MathGame>();
            if (gm != null) gm.AddScore(10);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
```

## 6.4 EnemySpawner.cs — Spawn Enemy trên vòng tròn

```csharp
using UnityEngine;

/// <summary>
/// MINI GAME - Enemy Spawner
/// 
/// KIẾN THỨC TOÁN HỌC:
/// • Quaternion × Vector  → Xoay vector để tạo vị trí trên vòng tròn
/// • Vector Cộng          → spawnPos = player.pos + offset
/// • LookRotation         → Enemy quay mặt về player khi spawn
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRadius = 25f;
    public float spawnInterval = 2f;
    public int maxEnemies = 15;
    public float minInterval = 0.5f;
    public float difficultyRate = 0.02f;

    private Transform player;
    private float nextSpawnTime;
    private float currentInterval;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        currentInterval = spawnInterval;
    }

    void Update()
    {
        if (player == null || enemyPrefab == null) return;
        if (Time.time < nextSpawnTime) return;

        int currentEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (currentEnemies >= maxEnemies) return;

        SpawnEnemy();
        currentInterval = Mathf.Max(minInterval, spawnInterval - Time.time * difficultyRate);
        nextSpawnTime = Time.time + currentInterval;
    }

    void SpawnEnemy()
    {
        // Bước 1: Vector phía trước × bán kính
        Vector3 spawnOffset = Vector3.forward * spawnRadius;

        // Bước 2: QUATERNION: Xoay vector một góc ngẫu nhiên quanh Y
        float randomAngle = Random.Range(0f, 360f);
        Quaternion rotation = Quaternion.Euler(0, randomAngle, 0);

        // Bước 3: QUATERNION × VECTOR = xoay vector
        spawnOffset = rotation * spawnOffset;

        // Bước 4: VECTOR CỘNG: Vị trí spawn = player + offset
        Vector3 spawnPos = player.position + spawnOffset;
        spawnPos.y = 1f;

        // LOOKROTATION: Enemy nhìn về player ngay khi spawn
        Vector3 dirToPlayer = (player.position - spawnPos).normalized;
        Quaternion spawnRot = Quaternion.LookRotation(dirToPlayer);

        Instantiate(enemyPrefab, spawnPos, spawnRot);
    }
}
```

## 6.5 GameManager_MathGame.cs — Quản lý Game

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// MINI GAME - Game Manager
/// Quản lý điểm, HP, trạng thái game.
/// </summary>
public class GameManager_MathGame : MonoBehaviour
{
    public static GameManager_MathGame Instance { get; private set; }

    public int maxHP = 5;

    [HideInInspector] public int currentHP;
    [HideInInspector] public int score;
    [HideInInspector] public bool isGameOver;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentHP = maxHP;
        score = 0;
        isGameOver = false;
    }

    public void AddScore(int points)
    {
        if (isGameOver) return;
        score += points;

        var player = Object.FindFirstObjectByType<ArrowGuardian>();
        if (player != null) player.score = score;
    }

    public void PlayerTakeDamage(int damage)
    {
        if (isGameOver) return;
        currentHP -= damage;
        if (currentHP <= 0)
        {
            currentHP = 0;
            isGameOver = true;
            Debug.Log($"GAME OVER! Score: {score}");
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
            RestartGame();
    }
}
```

## 6.6 CameraFollow.cs — Camera theo Player

```csharp
using UnityEngine;

/// <summary>
/// MINI GAME - Camera Follow
/// 
/// KIẾN THỨC TOÁN HỌC:
/// • Vector Cộng      → desiredPos = target.pos + offset
/// • Lerp             → Di chuyển camera mượt
/// • LookRotation     → Camera nhìn về player
/// • Slerp            → Xoay camera mượt
/// </summary>
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 12, -8);
    public float followSpeed = 5f;
    public float lookSpeed = 8f;

    void LateUpdate()
    {
        if (target == null) return;

        // VECTOR CỘNG: Vị trí mong muốn = Player + Offset
        Vector3 desiredPosition = target.position + offset;

        // LERP: Di chuyển camera mượt mà
        transform.position = Vector3.Lerp(
            transform.position, desiredPosition,
            followSpeed * Time.deltaTime
        );

        // LOOKROTATION + SLERP: Camera nhìn về player
        Vector3 lookDirection = target.position - transform.position;
        if (lookDirection != Vector3.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, desiredRotation,
                lookSpeed * Time.deltaTime
            );
        }
    }
}
```

## 6.7 MathDebugUI.cs — Hiển thị toán học Real-time

```csharp
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// MINI GAME - Math Debug UI
/// 
/// Hiển thị real-time các giá trị toán học khi chơi game.
/// Giúp học viên liên kết lý thuyết với thực hành:
/// - Thấy Dot Product thay đổi khi xoay
/// - Thấy Cross Product cho biết trái/phải
/// - Thấy Distance và Angle thay đổi khi di chuyển
/// 
/// Setup: Gắn vào Canvas. Tạo 2 UI Text và 1 Panel, kéo vào fields.
/// </summary>
public class MathDebugUI : MonoBehaviour
{
    [Header("UI References")]
    public Text scoreText;
    public Text mathDebugText;
    public GameObject gameOverPanel;

    private ArrowGuardian player;
    private GameManager_MathGame gm;

    void Start()
    {
        player = Object.FindFirstObjectByType<ArrowGuardian>();
        gm = GameManager_MathGame.Instance;
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (gm == null) gm = GameManager_MathGame.Instance;
        UpdateScoreUI();
        UpdateMathDebug();
        UpdateGameOver();
    }

    void UpdateScoreUI()
    {
        if (scoreText == null || gm == null) return;
        string hpBar = new string('#', gm.currentHP)
                     + new string('_', gm.maxHP - gm.currentHP);
        scoreText.text = $"HP: [{hpBar}]\nScore: {gm.score}";
    }

    void UpdateMathDebug()
    {
        if (mathDebugText == null || player == null) return;

        string info = "=== MATH DEBUG ===\n\n";

        info += $"[Distance] {player.debugDistance:F1}m";
        info += player.debugInRange ? "  <IN RANGE>\n" : "\n";

        info += $"[Dot Product] {player.debugDot:F3}";
        info += $"  ({GetDotMeaning(player.debugDot)})\n";

        info += $"[Cross.Y] {player.debugCross.y:F3}";
        info += $"  -> {player.debugSide}\n";

        info += $"[Angle] {player.debugAngle:F1} deg";
        info += player.debugInFOV ? "  <IN FOV>\n" : "\n";

        info += $"\nPos: {player.transform.position:F1}";
        info += $"\nFwd: {player.transform.forward:F2}";

        mathDebugText.text = info;
    }

    string GetDotMeaning(float dot)
    {
        if (dot > 0.9f) return "Nhìn thẳng";
        if (dot > 0.5f) return "Gần cùng hướng";
        if (dot > 0.0f) return "Lệch bên";
        if (dot > -0.5f) return "Gần vuông góc";
        return "Phía sau";
    }

    void UpdateGameOver()
    {
        if (gameOverPanel == null || gm == null) return;
        if (gm.isGameOver) gameOverPanel.SetActive(true);
    }
}
```

---

# ═══════════════════════════════════════════
# PHẦN 7: BẢNG TRA CỨU NHANH
# ═══════════════════════════════════════════

## Cheat Sheet

```
┌──────────────────────────────────────────────────────────────────────┐
│                    VECTOR3 - CHEAT SHEET                            │
├──────────────────────────────────────────────────────────────────────┤
│ v.magnitude           │ Độ lớn vector                               │
│ v.normalized          │ Vector đơn vị (hướng, magnitude=1)          │
│ v.sqrMagnitude        │ Bình phương magnitude (nhanh hơn)           │
│                       │                                             │
│ Vector3.Dot(a, b)     │ Tích vô hướng → float                      │
│ Vector3.Cross(a, b)   │ Tích có hướng → Vector3 vuông góc           │
│ Vector3.Angle(a, b)   │ Góc (0-180°) → float                       │
│ Vector3.SignedAngle()  │ Góc có dấu (-180 đến 180°) → float        │
│ Vector3.Distance()    │ Khoảng cách → float                        │
│ Vector3.Lerp(a,b,t)   │ Nội suy tuyến tính → Vector3               │
│ Vector3.MoveTowards() │ Di chuyển tốc độ cố định → Vector3         │
│ Vector3.Project()     │ Chiếu vector lên hướng → Vector3            │
│ Vector3.Reflect()     │ Phản xạ qua normal → Vector3                │
├──────────────────────────────────────────────────────────────────────┤
│                  QUATERNION - CHEAT SHEET                           │
├──────────────────────────────────────────────────────────────────────┤
│ Quaternion.identity       │ Không xoay                              │
│ Quaternion.Euler()        │ Euler → Quaternion                      │
│ Quaternion.AngleAxis()    │ Trục + Góc → Quaternion                 │
│ Quaternion.LookRotation() │ Hướng nhìn → Quaternion                 │
│ Quaternion.Slerp()        │ Nội suy cầu (mượt)                     │
│ Quaternion.Lerp()         │ Nội suy tuyến tính                     │
│ Quaternion.RotateTowards()│ Xoay tối đa N°                         │
│ Quaternion.Inverse()      │ Phép quay ngược                        │
│ Quaternion.Angle()        │ Góc giữa 2 rotation                    │
│ q * Vector3               │ Xoay vector                            │
│ q1 * q2                   │ Kết hợp phép quay                      │
└──────────────────────────────────────────────────────────────────────┘
```

## Khi nào dùng gì?

```
Muốn...                           → Dùng...
──────────────────────────────────────────────────────
Di chuyển object                   → position += dir * speed * dt
Tìm hướng A→B                     → (B.pos - A.pos).normalized
Kiểm tra phía trước/sau           → Dot Product (> 0 = trước)
Kiểm tra trái/phải                → Cross Product (.y > 0 = phải)
Tính khoảng cách                  → Vector3.Distance()
Tính góc                          → Vector3.Angle()
Xoay nhìn mục tiêu               → Quaternion.LookRotation()
Xoay mượt                         → Quaternion.Slerp()
Di chuyển mượt                    → Vector3.Lerp()
Spawn trên vòng tròn              → Quaternion.Euler(0,angle,0) * Vector3.forward * r
Bắn đạn theo hướng               → Rigidbody.velocity = dir * speed
Camera follow                     → Lerp + LookRotation
```

---

## 📌 LƯU Ý QUAN TRỌNG

> ⚠️ **Time.deltaTime**: LUÔN nhân khi di chuyển/xoay trong `Update()` 
> để tốc độ đồng nhất trên mọi máy (FPS khác nhau).

> ⚠️ **Normalize**: LUÔN chuẩn hóa vector hướng trước khi nhân tốc độ. 
> Nếu không, đi chéo nhanh hơn √2 ≈ 1.414 lần.

> ⚠️ **Vector3.zero**: LUÔN kiểm tra `direction != Vector3.zero` trước khi gọi 
> `LookRotation()`, nếu không sẽ gây lỗi.

> ⚠️ **Quaternion**: KHÔNG BAO GIỜ gán trực tiếp `.x`, `.y`, `.z`, `.w`. 
> Luôn dùng `Quaternion.Euler()` hoặc các hàm factory.

---

*Tài liệu được tạo cho project Demo3D — Dùng để giảng dạy toán học 3D trong Unity.*
