# 🕺 HƯỚNG DẪN TOÀN TẬP: TẠO NHÂN VẬT VỚI NHIỀU ANIMATION TỪ MIXAMO VÀ SỬ DỤNG TRONG UNITY 3D

> **Mục tiêu**: Tự tay tải một nhân vật 3D cùng bộ chuyển động (Đứng yên, Đi bộ, Chạy, Nhảy, Tấn công...) từ trang web **Adobe Mixamo**, đưa vào **Unity 3D**, xử lý vật liệu (Materials), thiết lập **Animator Controller** và viết **Script C#** điều khiển mượt mà.

---

## 📋 MỤC LỤC

| Phần | Nội dung |
|------|----------|
| **1** | Tổng quan quy trình (Workflow chuẩn) |
| **2** | Tải Nhân vật (Model) và nhiều Animation trên Mixamo |
| **3** | Đưa vào Unity 3D: Xử lý Texture, Rigging (Humanoid) & Loop |
| **4** | Thiết lập Animator Controller (State Machine & Blend Tree) |
| **5** | Viết Script C# điều khiển Nhân vật + Animation |
| **6** | Bảng tra cứu lỗi thường gặp & Cách khắc phục (Troubleshooting) |

---

# ═══════════════════════════════════════════
# PHẦN 1: TỔNG QUAN QUY TRÌNH (WORKFLOW)
# ═══════════════════════════════════════════

Khi làm việc với Mixamo và Unity 3D, sai lầm phổ biến nhất của người mới là **tải kèm cả xác nhân vật (With Skin) cho mọi file animation**, khiến dự án nặng gấp chục lần và khó quản lý.

**Quy trình chuẩn công nghiệp như sau:**

```
┌─────────────────────────────────────────────────────────────────────────┐
│ BƯỚC 1: TRÊN WEB MIXAMO (mixamo.com)                                    │
│  ├─ 1. Chọn 1 Nhân vật (Character) ──► Tải 1 file gốc T-Pose (WITH SKIN)│
│  └─ 2. Chọn các Animation (Idle, Walk, Run, Jump, Attack...)            │
│        ──► Tải từng file chỉ lấy xương chuyển động (WITHOUT SKIN)       │
├─────────────────────────────────────────────────────────────────────────┤
│ BƯỚC 2: TRONG UNITY 3D (Import & Cấu hình)                              │
│  ├─ 1. Giải nén Texture & Material từ file Nhân vật gốc                 │
│  ├─ 2. Chuyển Rig của Nhân vật + tất cả Animation sang kiểu HUMANOID    │
│  └─ 3. Tick "Loop Time" cho các animation lặp lại (Idle, Walk, Run)     │
├─────────────────────────────────────────────────────────────────────────┤
│ BƯỚC 3: ANIMATOR & CODE C#                                              │
│  ├─ 1. Tạo Animator Controller, nối các State / Blend Tree              │
│  └─ 2. Gắn CharacterController + Script C# để điều khiển bằng bàn phím  │
└─────────────────────────────────────────────────────────────────────────┘
```

---

# ═══════════════════════════════════════════
# PHẦN 2: TẢI NHÂN VẬT & ANIMATION TRÊN MIXAMO
# ═══════════════════════════════════════════

## 2.1 Đăng nhập và Chọn Nhân Vật (Character)

1. Truy cập trang web: **[https://www.mixamo.com](https://www.mixamo.com)** và đăng nhập bằng tài khoản Adobe (hoặc Google/Facebook hoàn toàn miễn phí).
2. Nhấn vào tab **Characters** (ở góc trên bên trái).
3. Chọn một nhân vật 3D bạn thích (ví dụ: *Y Bot*, *X Bot*, *Paladin*, *Erika Archer*, *Swat*...).
   - *(Tuỳ chọn)*: Nếu bạn có sẵn file 3D nhân vật riêng (`.fbx`, `.obj`), bạn có thể bấm nút **Upload Character** để Mixamo tự động gắn xương (Auto-Rig).

---

## 2.2 Tải File Nhân Vật Gốc (T-Pose)

Trước khi chọn bất kỳ hành động nào, hãy tải **xác nhân vật gốc ở tư thế chữ T (T-Pose)**:

1. Nhấn nút **Download** (màu cam, bên phải màn hình).
2. Cài đặt thông số như sau:
   - **Format**: `FBX for Unity (.fbx)` *(Rất quan trọng để tương thích hệ trục và chất liệu của Unity)*.
   - **Pose**: `T-Pose`.
3. Nhấn **Download**.
4. Sau khi tải về máy, hãy đổi tên file cho gọn, ví dụ: **`PlayerModel.fbx`**.

---

## 2.3 Tìm và Tải Nhiều Animation (Idle, Walk, Run, Jump, Attack)

Bây giờ, giữ nguyên nhân vật đó trên màn hình và chuyển sang tab **Animations** (góc trên bên trái).

Chúng ta sẽ tìm và tải lần lượt **5 animation cơ bản**:

| STT | Hành động | Từ khóa tìm kiếm gợi ý trên Mixamo | Cần tick `In Place`? |
|-----|-----------|------------------------------------|----------------------|
| 1 | **Đứng yên (Idle)** | `Idle`, `Breathing Idle`, `Happy Idle` | Không có / Không cần |
| 2 | **Đi bộ (Walk)** | `Walking`, `Standard Walk` | ✅ **BẮT BUỘC TICK** |
| 3 | **Chạy (Run)** | `Running`, `Fast Run` | ✅ **BẮT BUỘC TICK** |
| 4 | **Nhảy (Jump)** | `Jump`, `Jumping Up` | ✅ Nên tick (nếu có) |
| 5 | **Tấn công (Attack)** | `Punching`, `Sword And Shield Slash`, `Kick` | Không cần (hoặc tick nếu có) |

### ⚠️ LƯU Ý SỐNG CÒN KHI TẢI ANIMATION:

#### 1. Luôn tick vào ô `In Place` (Chạy tại chỗ) cho Walk / Run!
Khi bạn chọn animation `Walking` hoặc `Running`, ở bảng bên phải sẽ có một ô checkbox tên là **`In Place`**:
```
[✓] In Place   <── HÃY TICK VÀO Ô NÀY!
```
- **Tại sao?** Nếu không tick `In Place`, trong animation nhân vật sẽ tự lao về phía trước rồi giật lùi lại vị trí cũ, làm lệch hoàn toàn với `Collider` và `CharacterController` trong Unity! Khi tick `In Place`, nhân vật sẽ bước chân tại chỗ, còn việc dịch chuyển vị trí thật trong thế giới 3D sẽ do **Script C#** của chúng ta đảm nhiệm.

#### 2. Cấu hình Download cho các file Animation (`Without Skin`)
Với mỗi animation sau khi chọn xong, bấm nút **Download** và thiết lập:
- **Format**: `FBX for Unity (.fbx)`
- **Skin**: **`Without Skin`** *(Chỉ tải dữ liệu chuyển động của xương, nhẹ chỉ vài trăm KB!)*
- **Frames per Second**: `30` (Mặc định)
- **Keyframe Reduction**: `none`

#### 3. Quy ước đặt tên file theo chuẩn Unity (`TênNhânVật@TênAnimation.fbx`)
Ngay khi tải từng animation về máy, hãy đổi tên theo cấu trúc `@` để Unity tự động nhận diện tên Animation Clip bên trong:
- `Player@Idle.fbx`
- `Player@Walk.fbx`
- `Player@Run.fbx`
- `Player@Jump.fbx`
- `Player@Attack.fbx`

> 💡 **Câu hỏi thường gặp: "Có thể chọn 6 animation trên Mixamo rồi tải về trong 1 file `.fbx` duy nhất được không?"**
> - **Câu trả lời là KHÔNG trực tiếp trên web Mixamo**: Mixamo chỉ cho phép tải **từng animation một** (ngay cả khi bạn chọn các gói *Animation Pack* có sẵn trên Mixamo, nó cũng tải về 1 file `.zip` mà bên trong vẫn là nhiều file `.fbx` rời).
> - **Nhưng đừng lo!** Việc tải rời `1 Model (With Skin)` + `6 Animation (Without Skin)` rồi đưa vào **`Animator Controller`** của Unity thực chất **không phải là "ghép xương thủ công"**, mà Unity sẽ tự động ánh xạ (Retarget) qua chuẩn **`Humanoid`**.
> - **Mẹo làm gọn file trong Unity (Rút trích `.anim`)**: Sau khi cấu hình `Humanoid` và `Loop Time` xong, bạn có thể bấm mũi tên `►` mở rộng từng file `Player@Walk.fbx`, chọn biểu tượng tam giác **Animation Clip** bên trong rồi nhấn **`Ctrl + D` (Duplicate)**. Unity sẽ tách riêng chuyển động đó ra thành 1 file `.anim` độc lập, lúc này bạn có thể **xóa bỏ hoàn toàn các file `.fbx` animation rời** mà chỉ cần giữ lại đúng 1 file `PlayerModel.fbx`!

---

# ═══════════════════════════════════════════
# PHẦN 3: ĐƯA VÀO UNITY 3D & CẤU HÌNH CHUẨN
# ═══════════════════════════════════════════

## 3.1 Tổ chức thư mục trong Unity
Trong cửa sổ **Project** của Unity (`Assets/`), hãy tạo một thư mục gọn gàng như sau:
```
Assets/
 └── MixamoPlayer/
      ├── Models/        (Bỏ file PlayerModel.fbx và các file Player@....fbx vào đây)
      ├── Textures/      (Để chứa ảnh bề mặt trích xuất ra)
      ├── Materials/     (Để chứa vật liệu trích xuất ra)
      └── Animations/    (Chứa Animator Controller)
```
Kéo toàn bộ 6 file `.fbx` vừa tải (`PlayerModel.fbx` và 5 file `Player@....fbx`) thả vào thư mục `Assets/MixamoPlayer/Models/`.

---

## 3.2 Xử lý lỗi Nhân Vật Bị Trắng Xóa (Extract Textures & Materials)

Khi mới kéo `PlayerModel.fbx` vào Unity, nhân vật thường bị trắng bệch hoặc xám xịt vì Texture đang bị nén bên trong file `.fbx`. Hãy giải nén chúng:

1. Click chọn file **`PlayerModel`** trong cửa sổ Project.
2. Nhìn sang cửa sổ **Inspector**, chọn tab **Materials** (nằm ở hàng trên cùng: *Model | Rig | Animation | Materials*).
3. Thực hiện 2 bước theo đúng thứ tự:
   - **Bước A**: Nhấn nút **`Extract Textures...`** → Chọn thư mục `Textures` bạn đã tạo → Bấm *Select Folder*.
     - 💡 *Nếu hiện bảng thông báo hỏi về Normal Map, hãy bấm **`Fix Now`**.*
   - **Bước B**: Nhấn nút **`Extract Materials...`** → Chọn thư mục `Materials` → Bấm *Select Folder*.

> 🛠️ **Mẹo nếu nhân vật bị màu Hồng (Pink) trong dự án URP**:
> Nếu project của bạn dùng **Universal Render Pipeline (URP)** và các Material vừa giải nén bị màu hồng tím:
> 1. Chọn tất cả các Material trong thư mục `Materials`.
> 2. Vào menu trên cùng của Unity: **Edit → Rendering → Materials → Convert Selected Built-in Materials to URP** (Hoặc trong Inspector của Material, đổi *Shader* thành `Universal Render Pipeline/Lit`).

---

## 3.3 Cấu hình Hệ Xương (Rigging — Chuyển sang `Humanoid`)

Để các file animation rời (`Without Skin`) có thể điều khiển được nhân vật `PlayerModel`, cả nhân vật và các animation đều phải dùng chung chuẩn xương người (**Humanoid**) của Unity.

### Bước 1: Cấu hình Rig cho Nhân Vật Gốc (`PlayerModel`)
1. Chọn file **`PlayerModel`** trong Project.
2. Trong Inspector, chuyển sang tab **Rig**.
3. Thiết lập:
   - **Animation Type**: Chọn **`Humanoid`**.
   - **Avatar Definition**: Chọn **`Create From This Model`**.
4. Nhấn nút **`Apply`** ở góc dưới.
   *(Bạn sẽ thấy dấu tích `✓` kế bên nút Configure, báo hiệu Unity đã nhận diện hoàn hảo bộ xương Mixamo!)*

### Bước 2: Cấu hình Rig cho TẤT CẢ các file Animation (`Player@...`)
1. Giữ phím `Ctrl` (hoặc `Shift`) chọn cùng lúc cả 5 file: `Player@Idle`, `Player@Walk`, `Player@Run`, `Player@Jump`, `Player@Attack`.
2. Trong Inspector, chuyển sang tab **Rig**.
3. Thiết lập:
   - **Animation Type**: Chọn **`Humanoid`**.
   - **Avatar Definition**: Chọn **`Copy From Other Avatar`**.
   - **Source**: Bấm vào biểu tượng tròn nhỏ bên phải ô *Source* (hoặc kéo thả) chọn **`PlayerModelAvatar`** (chính là Avatar vừa được tạo ra bên trong file `PlayerModel` ở Bước 1).
4. Nhấn nút **`Apply`**.

---

## 3.4 Cấu hình Lặp lại (Loop Time) và Chống Lệch Hướng cho Animation

Mặc định các file tải từ Mixamo về chỉ chạy 1 lần rồi dừng. Chúng ta cần bật lặp lại cho các hành động liên tục:

1. Chọn lần lượt từng file **`Player@Idle`**, **`Player@Walk`**, **`Player@Run`**.
2. Trong Inspector, chuyển sang tab **Animation**.
3. Kéo xuống dưới phần tên clip (ví dụ: `Idle`, `Walk`, `Run`) và chỉnh:
   - ✅ Tick chọn **`Loop Time`** *(Bắt buộc cho Idle, Walk, Run!)*
   - ✅ Tick chọn **`Loop Pose`** *(Giúp điểm đầu và điểm cuối khớp nhau mượt mà)*
   - **Root Transform Rotation**: Tick **`Bake Into Pose`** → Mục *Based Upon* chọn **`Original`** *(Giúp nhân vật không bị tự xoay chéo người khi đang đi/chạy)*.
   - **Root Transform Position (Y)**: Tick **`Bake Into Pose`** → Mục *Based Upon* chọn **`Original`** (hoặc `Feet`) *(Giúp nhân vật không bị trồi sụt độ cao Y)*.
4. Nhấn nút **`Apply`** ở dưới cùng!

> ⚠️ **Đối với `Player@Jump` và `Player@Attack`**:
> - **KHÔNG** tick `Loop Time` (vì nhảy hoặc chém chỉ thực hiện 1 lần mỗi khi bấm phím).
> - Vẫn nên tick `Bake Into Pose` (Based Upon: `Original`) ở mục **Root Transform Rotation** và **Root Transform Position (Y)** (riêng Jump nếu là nhảy tại chỗ bằng vật lý code thì tick Bake Y, còn nếu animation đã tự nhấc chân lên thì kiểm tra trong cửa sổ Preview bên dưới). Nhớ bấm **Apply**.

---

# ═══════════════════════════════════════════
# PHẦN 4: THIẾT LẬP ANIMATOR CONTROLLER
# ═══════════════════════════════════════════

## 4.1 Tạo và Gắn Animator Controller vào Nhân Vật

1. Kéo file **`PlayerModel`** từ cửa sổ Project thả vào cửa sổ **Scene** (hoặc **Hierarchy**). Đổi tên GameObject trên Hierarchy thành **`Player`**.
2. Trong thư mục `Assets/MixamoPlayer/Animations/`, click chuột phải → **Create → Animation → Animator Controller** (hoặc *Create → Animator Controller*). Đặt tên là **`PlayerAnimator`**.
3. Chọn GameObject **`Player`** trên Hierarchy, nhìn vào component **`Animator`** bên Inspector:
   - Kéo **`PlayerAnimator`** thả vào ô **Controller**.
   - Pastikan ô **Avatar** đang là `PlayerModelAvatar`.
   - ❌ **BỎ TICK ô `Apply Root Motion`** *(Vì chúng ta sẽ di chuyển nhân vật bằng Script C#, tắt cái này để animation không tự dịch chuyển làm kẹt nhân vật)*.

---

## 4.2 Tạo các Tham Số Điều Khiển (Parameters)

1. Nhấp đúp vào **`PlayerAnimator`** để mở cửa sổ **Animator**.
2. Ở góc trên bên trái cửa sổ Animator, chọn tab **Parameters** (cạnh tab *Layers*).
3. Nhấn dấu **`+`** để tạo **3 Parameters** sau (chú ý viết đúng chữ hoa/thường):

| Tên Parameter | Kiểu (Type) | Ý nghĩa sử dụng trong Code C# |
|---------------|-------------|-------------------------------|
| **`Speed`** | `Float` | Tốc độ di chuyển: `0` = Đứng yên (Idle), `0.5` = Đi bộ (Walk), `1.0` = Chạy (Run) |
| **`Jump`** | `Trigger` | Kích hoạt nhảy khi bấm phím `Space` |
| **`Attack`** | `Trigger` | Kích hoạt tấn công khi bấm chuột trái |

---

## 4.3 Thiết lập Sơ Đồ Trạng Thái (2 Cách: Cơ Bản & Chuyên Nghiệp)

Bạn có thể chọn **Cách A (Blend Tree - Khuyên dùng vì cực kỳ mượt và gọn)** hoặc **Cách B (Nối từng State truyền thống)**.

### 🌟 Cách A (Khuyên dùng): Gộp Idle, Walk, Run bằng `Blend Tree`
Thay vì tạo 3 ô `Idle`, `Walk`, `Run` với hàng chục mũi tên rối rắm, **Blend Tree** giúp pha trộn mượt mà từ Đứng yên → Đi bộ → Chạy chỉ trong 1 khối duy nhất!

1. Trong vùng lưới của cửa sổ **Animator**, click chuột phải → **Create State → From New Blend Tree**.
2. Click chọn ô `Blend Tree` vừa tạo (nó sẽ có màu cam vì là State mặc định), đổi tên trong Inspector thành **`Locomotion`** (Chuyển động).
3. **Nhấp đúp (Double-click)** vào ô **`Locomotion`** để đi vào bên trong Blend Tree:
   - Click chọn node `Blend Tree` ở giữa.
   - Nhìn sang **Inspector**:
     - Đảm bảo **Parameter** đang chọn là **`Speed`**.
     - Ở danh sách **Motion**, bấm dấu **`+`** → chọn **`Add Motion Field`** **3 lần** (để tạo 3 dòng).
     - Bỏ tick ô **`Automate Thresholds`** để tự nhập mốc tốc độ:
       - **Dòng 1**: Kéo clip `Idle` (bên trong mũi tên mở rộng của `Player@Idle`) vào → Nhập **Threshold = `0`**.
       - **Dòng 2**: Kéo clip `Walk` (bên trong `Player@Walk`) vào → Nhập **Threshold = `0.5`**.
       - **Dòng 3**: Kéo clip `Run` (bên trong `Player@Run`) vào → Nhập **Threshold = `1`**.
4. Bấm vào chữ **`Base Layer`** ở thanh đường dẫn phía trên trái cửa sổ Animator để quay ra ngoài.

### ⚔️ Thêm trạng thái `Jump` và `Attack`:
1. Mở rộng dấu mũi tên `►` ở file `Player@Jump` và `Player@Attack` trong Project, kéo clip **`Jump`** và **`Attack`** thả vào cửa sổ **Animator**.
2. Tạo các mũi tên chuyển tiếp (**Make Transition**):

```
                    ┌──────────────┐
       ┌───────────►│     Jump     │───────────┐
       │ (Trigger:  └──────────────┘           ▼ (Has Exit Time: BẬT)
┌─────────────┐      Jump)              ┌──────────────┐
│  Any State  │                         │  Locomotion  │◄── Entry (Mặc định)
└─────────────┘      Attack)            │ (Blend Tree) │
       │ (Trigger:  ┌──────────────┐           ▲ (Has Exit Time: BẬT)
       └───────────►│    Attack    │───────────┘
                    └──────────────┘
```

#### Chi tiết cài đặt từng mũi tên (Transition) — CỰC KỲ QUAN TRỌNG:

1. **Mũi tên từ `Any State` → `Jump`**:
   - Click chuột phải vào `Any State` → *Make Transition* → chỉ vào `Jump`.
   - Click vào đường mũi tên vừa tạo, nhìn sang **Inspector**:
     - ❌ **BỎ TICK `Has Exit Time`** *(Để bấm phím là nhảy ngay lập tức!)*
     - Mở rộng *Settings*: ❌ **Bỏ tick `Can Transition To Self`** *(Tránh đang nhảy lại tự lặp lại nhảy từ đầu)*.
     - Ở mục **Conditions** (dưới cùng), bấm dấu `+` → chọn **`Jump`**.

2. **Mũi tên từ `Jump` → `Locomotion`**:
   - Click chuột phải vào `Jump` → *Make Transition* → chỉ vào `Locomotion`.
   - Click vào mũi tên đó, nhìn sang **Inspector**:
     - ✅ **GIỮ TICK `Has Exit Time`** (Chỉnh *Exit Time* khoảng `0.85` để nhảy gần xong thì tự chuyển về đứng/chạy).
     - Mục **Conditions** để trống.

3. **Mũi tên từ `Any State` → `Attack`**:
   - Click chuột phải vào `Any State` → *Make Transition* → chỉ vào `Attack`.
   - Click vào mũi tên đó:
     - ❌ **BỎ TICK `Has Exit Time`**.
     - Mở rộng *Settings*: ❌ **Bỏ tick `Can Transition To Self`**.
     - Ở mục **Conditions**, bấm dấu `+` → chọn **`Attack`**.

4. **Mũi tên từ `Attack` → `Locomotion`**:
   - Click chuột phải vào `Attack` → *Make Transition* → chỉ vào `Locomotion`.
   - Click vào mũi tên đó:
     - ✅ **GIỮ TICK `Has Exit Time`** (*Exit Time* khoảng `0.85` - `0.9`).
     - Mục **Conditions** để trống.

---

# ═══════════════════════════════════════════
# PHẦN 5: VIẾT SCRIPT C# ĐIỀU KHIỂN NHÂN VẬT
# ═══════════════════════════════════════════

## 5.1 Gắn `CharacterController` cho Nhân vật

Trước khi gắn code, nhân vật cần có khung va chạm và bộ điều khiển di chuyển:
1. Chọn GameObject **`Player`** trên Hierarchy.
2. Nhấn **Add Component** → tìm và thêm **`Character Controller`**.
3. Chỉnh thông số của **Character Controller** sao cho khung капсул (màu xanh lá) vừa khít người nhân vật Mixamo:
   - **Center**: `X = 0`, **`Y = 1`**, `Z = 0` *(Mặc định Y=0 sẽ làm một nửa người chìm dưới đất! Chỉnh Y = 1 hoặc bằng một nửa Height)*.
   - **Radius**: `0.3`
   - **Height**: `1.8` hoặc `2.0`

---

## 5.2 Script C# Hoàn Chỉnh (`MixamoPlayerController.cs`)

Tạo một file C# Script mới tên là **`MixamoPlayerController.cs`** và dán đoạn code dưới đây vào, sau đó kéo gắn vào GameObject **`Player`**:

```csharp
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class MixamoPlayerController : MonoBehaviour
{
    [Header("=== TỐC ĐỘ DI CHUYỂN ===")]
    public float walkSpeed = 3f;          // Tốc độ khi đi bộ (WASD)
    public float runSpeed = 6f;           // Tốc độ khi chạy (Giữ thêm Left Shift)
    public float rotationSpeed = 12f;     // Tốc độ xoay mặt nhân vật

    [Header("=== NHẢY & TRỌNG LỰC ===")]
    public float jumpHeight = 1.5f;       // Độ cao cú nhảy
    public float gravity = -19.62f;       // Trọng lực kéo xuống
    public float animationDampTime = 0.1f;// Độ mượt khi chuyển đổi Idle <-> Walk <-> Run

    [Header("=== THAM CHIẾU CAMERA (TÙY CHỌN) ===")]
    public Transform cameraTransform;     // Kéo Main Camera vào đây nếu muốn di chuyển theo góc nhìn Camera

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;             // Vận tốc trục Y (rơi tự do / nhảy)

    // Mã băm (Hash) cho các Parameter trong Animator giúp tối ưu hiệu năng
    private readonly int speedHash = Animator.StringToHash("Speed");
    private readonly int jumpHash = Animator.StringToHash("Jump");
    private readonly int attackHash = Animator.StringToHash("Attack");

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // Tự động lấy Main Camera nếu chưa gán trong Inspector
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        HandleMovementAndRotation();
        HandleJumpAndGravity();
        HandleActions();
    }

    /// <summary>
    /// 1. Xử lý Di chuyển (WASD), Xoay mặt nhân vật và cập nhật Blend Tree "Speed"
    /// </summary>
    private void HandleMovementAndRotation()
    {
        // Lấy tín hiệu phím A/D (Horizontal) và W/S (Vertical)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(h, 0f, v).normalized;

        // Kiểm tra xem người chơi có đang bấm phím di chuyển không
        if (inputDir.magnitude >= 0.1f)
        {
            // Kiểm tra giữ phím Left Shift để Chạy nhanh (Run), ngược lại là Đi bộ (Walk)
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float currentSpeed = isRunning ? runSpeed : walkSpeed;

            // Tính hướng di chuyển dựa theo hướng nhìn của Camera (nếu có)
            Vector3 moveDir = inputDir;
            if (cameraTransform != null)
            {
                Vector3 camForward = cameraTransform.forward;
                Vector3 camRight = cameraTransform.right;
                camForward.y = 0f;
                camRight.y = 0f;
                camForward.Normalize();
                camRight.Normalize();

                moveDir = (camForward * v + camRight * h).normalized;
            }

            // Xoay mặt nhân vật mượt mà về hướng di chuyển bằng Quaternion.Slerp + LookRotation
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            // Di chuyển nhân vật bằng CharacterController
            controller.Move(moveDir * currentSpeed * Time.deltaTime);

            // Cập nhật Parameter "Speed" trong Animator: 0.5f = Walk, 1.0f = Run
            float targetAnimSpeed = isRunning ? 1.0f : 0.5f;
            animator.SetFloat(speedHash, targetAnimSpeed, animationDampTime, Time.deltaTime);
        }
        else
        {
            // Khi thả phím: đưa Parameter "Speed" mượt mà về 0 (Idle)
            animator.SetFloat(speedHash, 0f, animationDampTime, Time.deltaTime);
        }
    }

    /// <summary>
    /// 2. Xử lý Nhảy (Phím Space) và Trọng lực rơi xuống sàn
    /// </summary>
    private void HandleJumpAndGravity()
    {
        bool isGrounded = controller.isGrounded;

        // Giữ nhân vật bám sát mặt đất khi đang đứng trên sàn
        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        // Bấm Space để nhảy khi đang chạm đất
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Công thức vật lý tính vận tốc nhảy: v = sqrt(h * -2 * g)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            // Kích hoạt Trigger "Jump" trong Animator
            animator.SetTrigger(jumpHash);
        }

        // Áp dụng trọng lực theo thời gian
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    /// <summary>
    /// 3. Xử lý Tấn công (Chuột trái hoặc phím J)
    /// </summary>
    private void HandleActions()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.J))
        {
            animator.SetTrigger(attackHash);
        }
    }
}
```

### 🎮 Cách điều khiển khi bấm Play:
- **`W, A, S, D`** (hoặc phím Mũi tên): Đi bộ (`Walk`) và tự động xoay mặt theo hướng đi.
- **Giữ `Left Shift` + `W, A, S, D`**: Chạy nhanh (`Run`).
- **Thả phím**: Đứng thở (`Idle`).
- **Phím `Space`**: Nhảy (`Jump`).
- **Chuột trái (`Left Click`) hoặc phím `J`**: Tấn công (`Attack`).

---

# ═══════════════════════════════════════════
# PHẦN 6: BẢNG TRA CỨU LỖI THƯỜNG GẶP & CÁCH SỬA
# ═══════════════════════════════════════════

Trong quá trình làm việc với Mixamo & Unity 3D, nếu bạn gặp bất kỳ hiện tượng lạ nào dưới đây, hãy tra bảng này để xử lý ngay:

| Hiện tượng lỗi | Nguyên nhân gốc rễ | Cách khắc phục triệt để |
|----------------|--------------------|-------------------------|
| **1. Nhân vật đứng dang 2 tay chữ T (T-Pose) trượt đi như tượng** | - Quên chuyển Rig sang `Humanoid`.<br>- Chưa gắn `Animator Controller` vào nhân vật. | - Kiểm tra cả file `PlayerModel` và các file `Player@...` ở tab **Rig** đã chọn `Humanoid` + bấm **Apply** chưa.<br>- Kiểm tra ô **Controller** trong component `Animator` của nhân vật trên Scene. |
| **2. Nhân vật bước đi một đoạn rồi tự giật lùi về vị trí cũ** | - Quên tick **`In Place`** khi tải animation trên Mixamo.<br>- Đang bật `Apply Root Motion` trong Animator. | - Tải lại animation Walk/Run trên Mixamo và nhớ tick **`[✓] In Place`**.<br>- Bỏ tick **`Apply Root Motion`** trên component `Animator` của Player. |
| **3. Bấm phím đi/đánh nhưng phải đợi 1-2 giây sau nhân vật mới đổi động tác** | - Đang bật **`Has Exit Time`** ở mũi tên chuyển tiếp (Transition) đi vào động tác đó. | - Click vào mũi tên chuyển từ `Any State` → `Attack`/`Jump` (hoặc từ `Idle` → `Walk`), nhìn sang Inspector và **BỎ TICK `Has Exit Time`**. |
| **4. Animation Đi bộ / Chạy chỉ bước đúng 1 bước rồi đơ cứng** | - Chưa bật **`Loop Time`** cho Animation Clip. | - Chọn file `Player@Walk`, `Player@Run`, `Player@Idle` trong Project → sang tab **Animation** → Tick chọn **`Loop Time`** → Bấm **Apply**. |
| **5. Một nửa thân dưới của nhân vật bị chìm xuống mặt đất** | - Tâm (`Center`) của `CharacterController` đang ở `(0, 0, 0)` trong khi gốc tọa độ (Pivot) của Mixamo nằm ở dưới lòng bàn chân. | - Chỉnh **`Center`** của `CharacterController` thành **`(0, 1, 0)`** (với `Height = 2`), sao cho đáy vòng xanh lá chạm đúng lòng bàn chân nhân vật. |
| **6. Khi chạy nhân vật bị lệch chéo người (không nhìn thẳng)** | - Animation trên Mixamo có góc xoay hông gốc hơi lệch. | - Chọn file Animation → tab **Animation** → mục **Root Transform Rotation** → Tick **`Bake Into Pose`** và chọn *Based Upon*: **`Original`** → Bấm **Apply**. |
| **7. Nhân vật màu trắng bệch hoặc màu hồng tím (Pink)** | - Chưa Extract Textures/Materials.<br>- Dự án dùng URP nhưng Material đang ở chuẩn Built-in cũ. | - Vào tab **Materials** của `PlayerModel` → Bấm `Extract Textures` rồi `Extract Materials`.<br>- Nếu màu hồng: đổi Shader của Material sang **`Universal Render Pipeline/Lit`**. |

---

## 📌 TÓM TẮT NHANH (CHECKLIST 1 PHÚT)

- [ ] **Trên Mixamo**: Tải 1 Model (`With Skin`, `T-Pose`) + Tải nhiều Animation (`Without Skin`, nhớ tick **`In Place`** cho Walk/Run).
- [ ] **Tab Materials**: `Extract Textures` → `Extract Materials`.
- [ ] **Tab Rig**: Đổi tất cả sang **`Humanoid`** (Model chọn *Create From This Model*, Animation chọn *Copy From Other Avatar*).
- [ ] **Tab Animation**: Tick **`Loop Time`** cho `Idle`, `Walk`, `Run` + Tick **`Bake Into Pose`** (`Original`) cho Rotation & Position Y.
- [ ] **Animator**: Tạo Blend Tree (`Speed`) + Trigger (`Jump`, `Attack`). Nhớ **bỏ tick `Has Exit Time`** khi bắt đầu chuyển hành động.
- [ ] **Inspector**: Gắn `CharacterController` (`Center Y = 1`, `Height = 2`) + Bỏ tick `Apply Root Motion` + Gắn Script `MixamoPlayerController.cs`.
