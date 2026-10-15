# 💡 CẨM NANG TOÀN DIỆN VỀ LIGHTING TRONG UNITY 3D

> **Mục tiêu**: Làm chủ toàn bộ hệ sinh thái ánh sáng trong Unity 3D (Đặc biệt tối ưu cho **URP - Universal Render Pipeline**). Hiểu sâu sắc bản chất vật lý của từng loại đèn, giải mã tường tận các chỉ số trong Inspector, kỹ thuật triệt tiêu lỗi đồ họa (Shadow Acne, Peter Panning) và 3 đoạn code Demo thực hành tích hợp sẵn bên dưới.

---

## 📋 MỤC LỤC

1. [Phần 1: Tổng quan về Ánh Sáng & 3 Chế độ Chiếu Sáng](#1-tong-quan-ve-anh-sang--3-che-do-chieu-sang)
2. [Phần 2: Chi tiết 4 Loại Nguồn Sáng Cơ Bản](#2-chi-tiet-4-loai-nguon-sang-co-ban)
3. [Phần 3: Giải mã Chi Tiết Từng Chỉ Số Trong Inspector](#3-giai-ma-chi-tiet-tung-chi-so-trong-inspector)
4. [Phần 4: Chữa lỗi Bóng Đổ: Shadow Acne & Peter Panning](#4-chua-loi-bong-do-shadow-acne--peter-panning)
5. [Phần 5: Ánh Sáng Môi Trường (Environment) & Light Probes](#5-anh-sang-moi-truong-environment--light-probes)
6. [Phần 6: Thực Hành 3 Đoạn Code Demo Ngắn (Copy & Dùng Ngay)](#6-thuc-hanh-3-doan-code-demo-ngan-copy--dung-ngay)
7. [Phần 7: Quy Tắc Vàng Tối Ưu Hiệu Năng](#7-quy-tac-vang-toi-uu-hieu-nang)
8. [Phần 8: Bảng Tra Cứu Nhanh (Cheat Sheet)](#8-bang-tra-cuu-nhanh-cheat-sheet)

---

## 1. Tổng Quan Về Ánh Sáng & 3 Chế Độ Chiếu Sáng

Ánh sáng (Lighting) không chỉ đơn thuần là làm sáng vật thể để người chơi nhìn thấy đường đi, mà là linh hồn quyết định:
- **Chiều sâu không gian (Depth & Volume)**: Phân tách tiền cảnh, trung cảnh và hậu cảnh.
- **Tâm lý & Bầu không khí (Mood & Atmosphere)**: Kinh dị u ám, ấm áp thanh bình, hay viễn tưởng lạnh lẽo.
- **Điều hướng thị giác (Visual Guidance)**: Hướng mắt người chơi đến cửa ra, rương kho báu hoặc kẻ địch.

```
       [ ÁNH SÁNG TRỰC TIẾP ] (Direct Light)
         Tia sáng chiếu thẳng từ đèn vào bề mặt
                     \
                      \  Bề mặt phản xạ
                       v  /
       [ ÁNH SÁNG GIÁN TIẾP ] (Indirect / Bounce Light)
         Tia sáng nảy từ sàn nhà hắt lên tường / trần
```

### 3 Chế độ hoạt động cốt lõi (Light Mode):

| Chế độ | Cơ chế tính toán | Ưu điểm | Nhược điểm | Trường hợp sử dụng |
| :--- | :--- | :--- | :--- | :--- |
| **`Realtime`** | Tính toán lại từng frame trên GPU bằng Shader | Linh hoạt 100%, bóng đổ di chuyển mượt mà theo nhân vật/đèn | Tốn GPU nhất, không tự nảy tia sáng gián tiếp (Bounce GI) | Đèn pin nhân vật, mặt trời chuyển động, chớp lửa đạn |
| **`Baked`** | Tính trước trong Editor và "vẽ chết" vào texture (Lightmap) | **0% chi phí tính toán GPU** khi chơi game, hỗ trợ GI phản xạ ánh sáng cực chân thực | Hoàn toàn bất động, không tương tác với nhân vật/quái di chuyển | Toàn bộ kiến trúc tĩnh (nhà cửa, cây cối, địa hình, đèn tường) |
| **`Mixed`** | Kết hợp cả 2: Vật thể tĩnh nhận ánh sáng nảy tĩnh, vật thể động vẫn nhận bóng realtime | Cân bằng hoàn hảo giữa đồ họa đỉnh cao và bóng động | Cần hiểu rõ setup (Subtractive, Shadowmask, Baked Indirect) | Nguồn sáng chính của màn chơi (Mặt trời, đèn chùm lớn) |

---

## 2. Chi Tiết 4 Loại Nguồn Sáng Cơ Bản

Unity cung cấp 4 hình thái nguồn sáng tiêu chuẩn:

```
    Directional Light          Point Light              Spot Light              Area Light
       |   |   |   |                *                      \  |  /              ┌──────────┐
       v   v   v   v             /  |  \                    \ | /               │          │
   (Tia song song vô cực)      *---( o )---*                  o                 │  Bề mặt  │
       |   |   |   |             \  |  /                    / | \               └────┬─────┘
       v   v   v   v                *                      /  |  \                   vvvvv
```

### 2.1. Directional Light (Ánh sáng Mặt trời / Mặt trăng)
* **Bản chất vật lý**: Tượng trưng cho nguồn sáng ở khoảng cách cực kỳ xa (vô cực). Các tia sáng luôn luôn **song song** với nhau trên toàn bộ bản đồ.
* **Đặc điểm sống còn**: 
  * **Tọa độ vị trí (`Position`) KHÔNG CÓ Ý NGHĨA**. Đặt đèn ở `(0, 0, 0)` hay `(9999, 9999, 9999)` đều cho kết quả sáng hệt như nhau!
  * **Góc xoay (`Rotation`) quyết định tất cả**: Hướng quay của đèn quyết định góc chiếu của mặt trời, hướng bóng đổ và thời gian trong ngày (bình minh, trưa, hoàng hôn).
* **Ứng dụng**: Mặt trời ban ngày, ánh trăng ban đêm, ánh sáng môi trường bao trùm toàn bộ Scene.

---

### 2.2. Point Light (Đèn tròn phát quang 360 độ)
* **Bản chất vật lý**: Tỏa ánh sáng từ một điểm cố định ra không gian xung quanh theo mọi hướng (hình cầu 360°).
* **Quy luật suy giảm (Attenuation)**: Càng xa tâm đèn, cường độ sáng càng yếu dần theo định luật nghịch đảo bình phương ($1 / d^2$).
* **Đặc điểm sống còn**:
  * Đèn có bán kính giới hạn gọi là **`Range`**. Vật thể nằm ngoài bán kính này sẽ không bị ảnh hưởng.
  * **Cực kỳ tốn tài nguyên bóng đổ (Shadows)**: Để Point Light đổ bóng, Unity phải tạo một **Cubemap Shadow Map (tương đương 6 lần render bóng)** bao quanh 6 hướng (Trước, Sau, Trái, Phải, Trên, Dưới).
* **Ứng dụng**: Đèn lồng, bóng đèn dây tóc trần nhà, đốm lửa trại, viên đạn plasma phát sáng, quả cầu ma thuật.

---

### 2.3. Spot Light (Đèn rọi hình nón / Đèn pha)
* **Bản chất vật lý**: Tỏa ánh sáng từ một điểm nhưng bị giới hạn trong một góc hình nón (**Cone**) hướng về một phía cụ thể.
* **Đặc điểm sống còn**:
  * Điều chỉnh được 2 góc: **`Inner Spot Angle`** (vùng sáng cực đại ở giữa) và **`Outer Spot Angle`** (ranh giới tắt dần ra mép ngoài).
  * Đổ bóng nhẹ hơn Point Light rất nhiều vì chỉ cần render bóng trên 1 mặt phẳng chiếu (2D Shadow Depth Map) thay vì 6 mặt.
* **Ứng dụng**: Đèn pin cầm tay góc nhìn thứ nhất (FPS Flashlight), đèn pha ô tô, đèn rọi sân khấu (Stage spotlight), đèn rọi tranh nghệ thuật, đèn đường rọi xuống vỉa hè.

---

### 2.4. Area Light (Ánh sáng mặt phẳng / Disc Light)
* **Bản chất vật lý**: Ánh sáng không phát ra từ 1 điểm đơn lẻ, mà phát xạ đồng đều từ toàn bộ diện tích của một bề mặt (hình chữ nhật hoặc hình tròn).
* **Đặc tính độc nhất**: Tạo ra hiệu ứng **Bóng mờ suy biến (Penumbra)** siêu thực — ở gần vật cản bóng rất nét, nhưng càng ra xa bóng càng loãng và nhòe dần tự nhiên.
* **Giới hạn kỹ thuật**: 
  * Trong **Built-in** và **URP chuẩn**, Area Light **CHỈ HỖ TRỢ CHẾ ĐỘ BAKED** (Không thể bật Realtime khi game đang chạy).
  * Mọi vật thể muốn nhận ánh sáng từ Area Light bắt buộc phải được tick chọn **`Static`**.
* **Ứng dụng**: Cửa sổ đón ánh sáng trời hắt vào phòng tối, màn hình TV/máy tính phát sáng, hộp đèn quảng cáo neon, bóng đèn tuýp huỳnh quang dài.

---

### 🖥️ HƯỚNG DẪN THỰC HÀNH: TẠO MÀN HÌNH VI TÍNH PHÁT SÁNG (GLOWING MONITOR)

Trong game 3D, nếu bạn chỉ đặt một bóng đèn thông thường, màn hình trông sẽ giả và vô hồn. Để tạo màn hình máy tính phát sáng chân thực như game AAA, bạn cần áp dụng **"Công thức Kiềng 3 Chân"**:

```
                       [ 1. MẶT KÍNH MÀN HÌNH ] 
                       Vật liệu tự phát quang (Emissive Material)
                       -> Nhìn trực diện thấy màn hình phát sáng chói lóa
                                    |
                                    v
                       [ 2. AREA LIGHT (RECTANGLE) ]
                       Đèn mặt phẳng đặt sát mặt kính, chiếu ra ngoài
                       -> Hắt ánh sáng mềm mại lên bàn phím, chuột, tường
                                    |
                                    v
                       [ 3. POST-PROCESSING BLOOM ]
                       Hiệu ứng camera xử lý quầng sáng
                       -> Tạo quầng hào quang (glow) lan tỏa vào không khí
```

#### 📌 Bước 1: Tạo Mô Hình Màn Hình Máy Tính
1. Nếu chưa có model màn hình 3D, bạn có thể tạo nhanh bằng:
   * Thân màn hình: Tạo 1 `Cube` mỏng (Scale: `X: 0.8, Y: 0.45, Z: 0.03`).
   * Chân đế: 1 `Cube` nhỏ bên dưới đỡ màn hình.
   * Mặt kính hiển thị: Mặt phẳng phía trước của Cube.

#### 📌 Bước 2: Tạo Vật Liệu Tự Phát Sáng (Emissive Material)
Area Light chiếu ra ngoài nhưng **bản thân nó là vô hình trước Camera**. Ta cần một Material để chính mặt kính màn hình tự phát sáng:
1. Trong cửa sổ **Project**, click chuột phải `Create > Material`, đặt tên là `M_MonitorScreen`.
2. Chọn Material vừa tạo, trong cửa sổ **Inspector**:
   * **Shader**: Chọn `Universal Render Pipeline/Lit`.
   * **Base Map**: Kéo hình ảnh màn hình máy tính (giao diện Windows, đoạn mã code, màn hình xanh hacker...) vào ô này.
   * Tick chọn ô **`Emission`**:
     * **Color**: Chọn tông màu màn hình (ví dụ: xanh dương nhạt `#66CCFF` hoặc màu trắng). Bấm vào bảng màu, chỉnh thanh trượt **HDR Intensity** lên khoảng `+1.5` đến `+2.5` (màu sẽ rực sáng lên).
     * **Emission Map**: Kéo chính hình ảnh giao diện máy tính vào ô này để các chữ/hình ảnh tự phát sáng.
3. Kéo thả Material `M_MonitorScreen` vào mặt trước của màn hình máy tính.

#### 📌 Bước 3: Tạo Area Light Để Hắt Sáng Lên Bàn Phím
1. Click chuột phải trong Hierarchy: **`Light > Area Light`**.
2. Đổi tên thành `Screen_AreaLight`, kéo nó làm con (Child) của màn hình máy tính.
3. Trong Inspector của `Screen_AreaLight`:
   * **Type**: `Rectangle` (Hình chữ nhật).
   * **Shape**: Chỉnh `Width` và `Height` khớp chính xác với kích thước màn hình (ví dụ: `Width = 0.8`, `Height = 0.45`).
   * **Color**: Chọn màu xanh dương nhạt hoặc cùng tông với hình nền màn hình.
   * **Intensity**: Đặt khoảng `3.0 - 6.0` (tùy độ tối của phòng).
4. **Căn chỉnh vị trí**: Đặt Area Light **hơi nhô ra phía trước mặt kính một chút** (cách mặt kính khoảng `0.01m`) và xoay trục Z màu xanh hướng thẳng ra phía trước bàn phím.

#### 📌 Bước 4: Thiết Lập Static & Nướng Ánh Sáng (Bake Lightmap)
Vì Area Light trong URP là Baked Light:
1. Chọn bàn làm việc, ghế ngồi, tường và màn hình máy tính: Trong Inspector, tick chọn ô **`Static`** (hoặc tích `Contribute GI`).
2. Mở cửa sổ: **`Window > Rendering > Lighting`**.
3. Bấm nút **`Generate Lighting`** ở góc dưới cùng bên phải.
   * *Kết quả*: Unity sẽ nướng ánh sáng từ Area Light lên mặt bàn và bàn phím. Bạn sẽ thấy bóng đổ mịn màng (Penumbra) cực kỳ chân thực hắt ra từ màn hình!

> 💡 **Mẹo cho Scene Realtime (Không muốn Bake)**:
> Nếu game của bạn không dùng Baked GI (ví dụ màn chơi thời gian thực hoàn toàn), hãy thay `Area Light` bằng một **`Spot Light`** đặt ngay trước màn hình:
> * Đặt `Spot Angle = 110 - 120` (rọi rộng).
> * Đặt `Range = 1.5m` (vừa chạm bàn phím và mặt nhân vật).
> * Chỉnh `Shadows = Soft Shadows`. Cách này chạy Realtime 100% không cần nướng!

#### 📌 Bước 5: Bật Quầng Sáng Điện Ảnh (Bloom)
Để màn hình có quầng sáng huyền ảo (Glow) như trong phim Cyberpunk:
1. Tạo một GameObject: chuột phải trong Hierarchy chọn `Volume > Global Volume`.
2. Tạo mới một `Profile` (bấm nút New).
3. Bấm **`Add Override > Post-processing > Bloom`**.
4. Tick bật các thông số:
   * **Threshold**: Đặt `1.0` (Chỉ các vật thể có HDR Intensity > 1.0 như màn hình mới phát sáng).
   * **Intensity**: Đặt `1.5 - 2.5`.
   * **Scatter**: Đặt `0.7` (Độ lan tỏa của quầng sáng).
   * Đảm bảo trên **Main Camera**, ô **Post Processing** đã được tick chọn `true`.

#### 📌 Code Bổ Trợ: Giả Lập Màn Hình Đang Phát Video (Nhấp Nháy Nhẹ)
Nếu muốn màn hình sống động (ánh sáng chập chờn thay đổi như đang chiếu phim/lướt web), gắn đoạn script C# sau vào màn hình:

```csharp
using UnityEngine;

public class MonitorScreenEffect : MonoBehaviour
{
    [Header("--- THIẾT LẬP MÀN HÌNH ---")]
    public Renderer screenRenderer; // MeshRenderer của mặt kính màn hình
    public Light screenLight;       // Spot Light hoặc Area Light chiếu sáng
    public float baseIntensity = 3.0f;
    public float flickerSpeed = 5.0f;
    public float flickerAmount = 0.5f;

    private Material screenMat;
    private Color originalEmissionColor;

    void Start()
    {
        if (screenRenderer != null)
        {
            screenMat = screenRenderer.material;
            originalEmissionColor = screenMat.GetColor("_EmissionColor");
        }
    }

    void Update()
    {
        // 1. Dùng Perlin Noise tạo dao động ánh sáng như nội dung video thay đổi
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
        float currentIntensity = baseIntensity + (noise - 0.5f) * flickerAmount;

        // 2. Thay đổi độ sáng của đèn hắt ra bàn
        if (screenLight != null)
            screenLight.intensity = currentIntensity;

        // 3. Thay đổi độ chói HDR của chính mặt kính
        if (screenMat != null)
        {
            Color currentEmission = originalEmissionColor * Mathf.LinearToGammaSpace(currentIntensity);
            screenMat.SetColor("_EmissionColor", currentEmission);
        }
    }
}
```

---

## 3. Giải Mã Chi Tiết Từng Chỉ Số Trong Inspector

Khi chọn một Component `Light` trong Unity, dưới đây là ý nghĩa chính xác của từng thông số:

```
[ Inspector: Light Component ]
┌──────────────────────────────────────────────┐
│ Type:               [ Spot                v] │
│ Mode:               [ Realtime            v] │
│ Color:              [ ████ #FFE0A3        v] │
│ Use Color Temp:     [v] Temperature: [ 3200] │
│ Intensity:          [ 2.5                  ] │
│ Indirect Multiplier:[ 1.0                  ] │
│ Range:              [ 12.0                 ] │
│ Spot Angle:         [ 45.0  (Inner: 25.0)  ] │
│ Shadows:            [ Soft Shadows        v] │
│   - Strength:       [ 0.85                 ] │
│   - Bias:           [ 0.05                 ] │
│   - Normal Bias:    [ 0.4                  ] │
│   - Near Plane:     [ 0.2                  ] │
└──────────────────────────────────────────────┘
```

### 3.1. Color & Color Temperature (Nhiệt Độ Màu Kelvin)
* **`Color`**: Mã màu RGB tùy ý.
* **`Use Color Temperature` (Khuyên dùng)**: Kích hoạt mô phỏng vật lý theo thang đo nhiệt độ màu Kelvin ($K$). Giúp màu sắc cảnh giới đạt độ tự nhiên cao:

| Nhiệt độ ($K$) | Sắc thái thị giác | Nguồn sáng thực tế |
| :---: | :--- | :--- |
| **1500K - 1800K** | Đỏ cam đậm ấm | Ngọn nến, đốm than hoa, diêm quẹt |
| **2700K - 3000K** | Vàng dịu ấm | Đèn sợi đốt phòng ngủ, đèn bàn cổ điển |
| **4000K - 4500K** | Trắng ngà tự nhiên | Đèn huỳnh quang, ánh sáng trăng rằm |
| **5500K - 6500K** | Trắng tinh khiết | Ánh nắng mặt trời trưa hè chuẩn quốc tế ($D65$) |
| **7500K - 9000K** | Xanh da trời lạnh | Trời nhiều mây u ám, ánh sáng bóng râm |
| **10000K+** | Xanh dương đậm | Bầu trời tuyết cực bắc, ánh sáng ma mị Sci-fi |

---

### 3.2. Intensity & Đơn Vị Đo (Units)
* **`Intensity`**: Cường độ sáng của đèn.
* Trong URP / HDRP, Unity hỗ trợ cả đơn vị vật lý:
  * **Lux (Lumen/m²)**: Đo độ rọi trên bề mặt (thường dùng cho Directional Light mặt trời).
  * **Lumen (lm)**: Đo tổng lượng quang thông phát ra mọi phía (thường dùng cho Point Light / Spot Light).
* **Giá trị thực tế tham khảo**:
  * Ban đêm có trăng: `0.1 - 0.25 Lux`
  * Phòng làm việc tiêu chuẩn: `300 - 500 Lux`
  * Ngoài trời ngày âm u: `1,000 - 5,000 Lux`
  * Nắng gắt buổi trưa mùa hè: `80,000 - 100,000 Lux`

---

### 3.3. Indirect Multiplier (Hệ số tán xạ nảy sáng)
* Điều chỉnh mức độ năng lượng của nguồn sáng này khi **nảy sang các bề mặt khác** trong tính toán Global Illumination (GI).
* `1.0`: Nảy sáng bình thường theo định luật vật lý.
* `0.0`: Tắt hoàn toàn ánh sáng nảy (chỉ có tia sáng trực tiếp, hốc tối sẽ đen kịt).
* `> 1.0`: Khuếch đại ánh sáng phản xạ (rất hữu ích trong kiến trúc nội thất khi muốn phòng sáng bừng chỉ với một ô cửa sổ nhỏ).

---

### 3.4. Range & Spot Angles (Point & Spot Light)
* **`Range`**: Khoảng cách tối đa (mét) mà tia sáng truyền tới trước khi tắt hẳn về 0.
  > ⚠️ **Mẹo hiệu năng**: Đừng kéo `Range` quá rộng không cần thiết! Unity dùng hình cầu `Range` này để kiểm tra va chạm ánh sáng với các Mesh. `Range` càng to, càng nhiều Mesh bị tính toán ánh sáng -> Tụt FPS.
* **`Inner Spot Angle`**: Góc nón bên trong nhận 100% cường độ sáng.
* **`Outer Spot Angle`**: Góc nón bên ngoài cùng. Khoảng chênh lệch giữa Inner và Outer sẽ tạo độ nhòe viền êm ái (Feather/Falloff).

---

### 3.5. Shadows (Các loại bóng đổ)
* **`None`**: Không đổ bóng. Ánh sáng xuyên qua mọi bức tường và vật thể (Nhẹ nhất, dùng cho ánh sáng phụ/đèn nhấp nháy nhỏ).
* **`Hard Shadows`**: Mép bóng sắc lẹm, phân định rõ ràng sáng - tối. Rẻ về GPU, nhưng trông khô cứng và phi thực tế.
* **`Soft Shadows`**: Mép bóng mềm mại, loang dần theo bộ lọc làm mờ. Trông chân thực và cao cấp nhất.
* **`Shadow Strength` (0 đến 1)**: Độ đậm của bóng. 
  * `= 1.0`: Vùng bóng tối hoàn toàn chặn đứng tia sáng trực tiếp.
  * `= 0.5`: Vùng bóng mờ mờ, tia sáng vẫn lọt qua 50%.

---

## 4. Chữa Lỗi Bóng Đổ: Shadow Acne & Peter Panning

Khi làm việc với Realtime Shadows, hầu như 100% lập trình viên đều gặp 2 lỗi kinh điển sau:

```
[ 1. SHADOW ACNE (Mụn bóng sọc rằn ri) ]     [ 2. PETER PANNING (Bóng bay lơ lửng) ]
         /////////////////                                 ┌──────┐
        ┌─────────────────┐                                │ Cube │ (Chân vật thể chạm đất)
        │   Mặt phẳng     │                                └──────┘
        └─────────────────┘                        ------------------------ (Mặt đất)
     (Xuất hiện các vằn sọc                                ░░░░░░░░
      đen loang lổ như da hổ)                    (Bóng nằm tít phía dưới, tách rời!)
```

### Nguyên nhân và cách trị dứt điểm:

| Hiện tượng | Nguyên nhân gốc rễ | Cách khắc phục trong Inspector |
| :--- | :--- | :--- |
| **Shadow Acne** (Vệt sọc ngang rằn ri khắp bề mặt) | Độ phân giải Shadow map hữu hạn, bề mặt tự tính toán đổ bóng đè lên chính nó (Z-fighting giữa tia sáng và bề mặt phẳng). | **TĂNG nhẹ `Bias`** (ví dụ từ `0.0` lên `0.05`) hoặc tăng **`Normal Bias`** (từ `0.0` lên `0.4 - 0.75`). |
| **Peter Panning** (Bóng bị tách rời, lơ lửng khỏi chân nhân vật) | Đẩy `Bias` quá cao làm Unity dời mặt phẳng bóng đi quá xa bề mặt thực tế. | **GIẢM `Bias` và `Normal Bias` xuống** vừa đủ cho tới khi sọc biến mất nhưng bóng vẫn dính sát chân vật thể. |

---

## 5. Ánh Sáng Môi Trường (Environment) & Light Probes

Ánh sáng trực tiếp từ đèn chỉ là một nửa câu chuyện. Nửa còn lại là **Ánh sáng môi trường (Environment Lighting)**.

Mở qua: **`Window > Rendering > Lighting` > Tab `Environment`**:

```
                       ┌─────────────────────────┐
                       │   Bầu trời (Skybox)     │
                       └────────────┬────────────┘
                                    │ (Chiếu đều từ vòm trời)
                                    v
                            ┌──────────────┐
                            │    Vật thể   │
                            └──────────────┘
                                    ^
                                    │ (Hắt từ mặt sàn lên)
                       ┌────────────┴────────────┐
                       │  Màu nền đất (Ground)   │
                       └─────────────────────────┘
```

1. **Source (Nguồn phát sáng môi trường)**:
   * **Skybox**: Dùng hình ảnh bầu trời làm nguồn sáng ambient bao quanh toàn cảnh. Cảnh có mây xanh nắng vàng sẽ hắt tone xanh tự nhiên.
   * **Gradient**: Chia làm 3 tầng màu riêng biệt:
     * *Sky Color*: Màu trần/vòm trời hắt xuống đỉnh đầu.
     * *Equator Color*: Màu đường chân trời hắt vào thân người.
     * *Ground Color*: Màu mặt đất hắt ngược lên dưới cằm.
   * **Color**: Dùng một màu phẳng duy nhất (ít tự nhiên nhất, nhưng rẻ nhất).

2. **Reflection Probes (Hộp phản xạ kim loại)**:
   * Nếu vật liệu có độ bóng cao (`Smoothness = 0.9` và `Metallic = 1.0`), nó sẽ phản chiếu lại thế giới.
   * Nếu không có Reflection Probe, kim loại sẽ phản chiếu bầu trời Skybox mặc định, trông rất giả khi đứng bên trong nhà kín.
   * Đặt **Reflection Probe** trong phòng để vật phẩm kim loại/vũng nước phản chiếu đúng đồ đạc nội thất xung quanh!

3. **Light Probes (Cảm biến ánh sáng mẫu cho nhân vật động)**:
   * Khi bạn Bake ánh sáng cho công trình tĩnh, cảnh nhà rất đẹp nhưng **Player di chuyển vào phòng sẽ không nhận được ánh sáng nảy (Bounce Light)** mà vẫn tối đen.
   * **Giải pháp**: Rải một cụm mạng lưới `Light Probe Group` trong không gian. Unity sẽ nướng thông tin ánh sáng vào các điểm neo (Probes) này. Khi Player đi ngang qua, shader sẽ lấy mẫu ánh sáng giữa các probe gần nhất và thắp sáng Player một cách mượt mà!

---

## 6. Thực Hành 3 Đoạn Code Demo Ngắn (Copy & Dùng Ngay)

Dưới đây là 3 đoạn script hoàn chỉnh, được tối ưu sẵn. Bạn chỉ cần tạo script trong Unity và dán mã nguồn vào:

### Demo 1: Đèn Pin FPS Sinh Tồn (Spot Light)

Script mô phỏng đèn pin góc nhìn thứ nhất: bật/tắt bằng phím `F`, thay pin bằng phím `R`, tiêu hao năng lượng và **nhấp nháy chập chờn khi pin yếu**, đi kèm hiệu ứng quán tính (Sway) lắc lư theo chuột:

```csharp
using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlashlightDemo : MonoBehaviour
{
    [Header("--- THIẾT LẬP ĐÈN PIN ---")]
    public Light flashlight;
    public KeyCode toggleKey = KeyCode.F;
    public KeyCode reloadKey = KeyCode.R;
    public float maxIntensity = 2.5f;

    [Header("--- HỆ THỐNG PIN ---")]
    public float maxBatteryDuration = 60f; // Sống được 60 giây
    [Range(0f, 1f)] public float currentBattery = 1f;
    public float lowBatteryThreshold = 0.2f; // Dưới 20% thì chập chờn

    [Header("--- QUÁN TÍNH CHUỘT (SWAY) ---")]
    public bool enableSway = true;
    public float swayAmount = 1.5f;
    public float swaySmoothness = 6f;

    private bool isOn = true;
    private Quaternion initialLocalRotation;

    void Start()
    {
        if (flashlight == null) flashlight = GetComponent<Light>();
        initialLocalRotation = transform.localRotation;
    }

    void Update()
    {
        // 1. Phím bật / tắt đèn (F)
        if (Input.GetKeyDown(toggleKey))
        {
            isOn = !isOn;
            flashlight.enabled = isOn && (currentBattery > 0f);
        }

        // 2. Phím thay pin (R)
        if (Input.GetKeyDown(reloadKey))
        {
            currentBattery = 1f;
            if (isOn) flashlight.enabled = true;
            Debug.Log("Đã nạp đầy pin 100%!");
        }

        // 3. Xử lý tụt pin
        if (isOn && currentBattery > 0f)
        {
            currentBattery -= (Time.deltaTime / maxBatteryDuration);
            currentBattery = Mathf.Clamp01(currentBattery);

            float targetIntensity = maxIntensity * currentBattery;

            // Nhấp nháy chập chờn khi sắp cạn pin
            if (currentBattery <= lowBatteryThreshold)
            {
                float flicker = Mathf.PerlinNoise(Time.time * 25f, 0f);
                targetIntensity *= (flicker > 0.4f ? 1f : 0.1f);
            }

            flashlight.intensity = targetIntensity;

            if (currentBattery <= 0f) flashlight.enabled = false;
        }

        // 4. Hiệu ứng trễ quán tính (Sway)
        if (enableSway)
        {
            float mouseX = Input.GetAxis("Mouse X") * swayAmount;
            float mouseY = Input.GetAxis("Mouse Y") * swayAmount;
            Quaternion targetRot = initialLocalRotation * Quaternion.AngleAxis(-mouseY, Vector3.right) * Quaternion.AngleAxis(mouseX, Vector3.up);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, Time.deltaTime * swaySmoothness);
        }
    }
}
```

* **Cách gắn trên Scene**:
  1. Tạo 1 `Spot Light` làm con của `Main Camera`.
  2. Gắn script `FlashlightDemo` vào Spot Light.
  3. Chỉnh `Spot Angle: 45`, `Range: 15`, `Shadows: Soft Shadows`. Bấm **Play** và nhấn phím **F**!

---

### Demo 2: Chu Kỳ Ngày - Đêm Tự Động (Directional Light)

Script tự động xoay mặt trời theo chu kỳ 24h ảo, chuyển sắc độ màu mượt mà bằng `Gradient` và tắt/bật đèn đường khi đêm xuống:

```csharp
using UnityEngine;

public class DayNightCycleDemo : MonoBehaviour
{
    [Header("--- THIẾT LẬP MẶT TRỜI ---")]
    public Light sunLight;
    public float dayDurationInSeconds = 120f; // 120s = 1 ngày ảo
    [Range(0f, 24f)] public float timeOfDay = 12f;

    [Header("--- MÀU SẮC & CƯỜNG ĐỘ ---")]
    public Gradient sunColorGradient;
    public AnimationCurve sunIntensityCurve;

    [Header("--- ĐÈN ĐÊM (BẬT KHI TRỜI TỐI) ---")]
    public GameObject[] nightLights;

    void Start()
    {
        if (sunLight == null) sunLight = GetComponent<Light>();
    }

    void Update()
    {
        // 1. Tiến trình thời gian
        timeOfDay += (Time.deltaTime / dayDurationInSeconds) * 24f;
        if (timeOfDay >= 24f) timeOfDay = 0f;

        float normalizedTime = timeOfDay / 24f;

        // 2. Xoay góc mặt trời (6h = 0 độ, 12h = 90 độ đỉnh đầu, 18h = 180 độ)
        float sunAngle = (normalizedTime * 360f) - 90f;
        sunLight.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

        // 3. Áp dụng màu và cường độ
        if (sunColorGradient != null) sunLight.color = sunColorGradient.Evaluate(normalizedTime);
        if (sunIntensityCurve != null) sunLight.intensity = sunIntensityCurve.Evaluate(normalizedTime);

        // 4. Bật đèn đêm từ 18h30 đến 5h30 sáng
        bool isNight = (timeOfDay >= 18.5f || timeOfDay <= 5.5f);
        if (nightLights != null)
        {
            foreach (var lightObj in nightLights)
            {
                if (lightObj != null && lightObj.activeSelf != isNight)
                    lightObj.SetActive(isNight);
            }
        }
    }
}
```

* **Cách gắn trên Scene**:
  1. Chọn GameObject `Directional Light` trong Scene.
  2. Gắn script `DayNightCycleDemo`.
  3. Kéo `Directional Light` vào ô `Sun Light`. Chỉnh `dayDurationInSeconds = 60` để thấy cảnh sắc chuyển mình nhanh chóng.

---

### Demo 3: Ngọn Đuốc / Lửa Trại Bập Bùng (Point Light)

Sử dụng thuật toán toán học **Perlin Noise** để tạo dao động ánh sáng mềm mại, biến thiên sắc độ ấm nóng và rung rinh vị trí tự nhiên:

```csharp
using UnityEngine;

[RequireComponent(typeof(Light))]
public class TorchFlickerDemo : MonoBehaviour
{
    [Header("--- NGUỒN SÁNG ĐUỐC/LỬA ---")]
    public Light fireLight;
    public float minIntensity = 1.8f;
    public float maxIntensity = 3.2f;
    public float flickerSpeed = 8.0f;

    [Header("--- RUNG RINH VỊ TRÍ ---")]
    public bool enableJitter = true;
    public float jitterRadius = 0.05f;

    [Header("--- BIẾN THIÊN SẮC ĐỘ ---")]
    public Color warmColor = new Color(1.0f, 0.55f, 0.15f); // Cam lửa
    public Color hotColor = new Color(1.0f, 0.85f, 0.4f);   // Vàng sáng

    private Vector3 initialPos;
    private float seed;

    void Start()
    {
        if (fireLight == null) fireLight = GetComponent<Light>();
        initialPos = transform.localPosition;
        seed = Random.Range(0f, 1000f);
    }

    void Update()
    {
        if (fireLight == null) return;

        // 1. Cường độ chập chờn tự nhiên qua Perlin Noise
        float noise = Mathf.PerlinNoise(seed + Time.time * flickerSpeed, 0f);
        fireLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);

        // 2. Đổi màu theo độ rực
        fireLight.color = Color.Lerp(warmColor, hotColor, noise);

        // 3. Dao động vị trí ngọn lửa trong gió
        if (enableJitter)
        {
            float jX = (Mathf.PerlinNoise(seed + Time.time * 4f, 10f) - 0.5f) * 2f * jitterRadius;
            float jY = (Mathf.PerlinNoise(seed + Time.time * 4f, 20f) - 0.5f) * 2f * jitterRadius;
            float jZ = (Mathf.PerlinNoise(seed + Time.time * 4f, 30f) - 0.5f) * 2f * jitterRadius;
            transform.localPosition = initialPos + new Vector3(jX, jY, jZ);
        }
    }
}
```

* **Cách gắn trên Scene**:
  1. Tạo 1 `Point Light` đặt tại đống lửa trại hoặc ngọn đuốc.
  2. Gắn script `TorchFlickerDemo` vào Point Light.
  3. Đặt `Range: 6 - 8`. Ánh lửa sẽ bập bùng sống động tức thì!

---

## 7. Quy Tắc Vàng Tối Ưu Hiệu Năng

Ánh sáng là thủ phạm số 1 gây tụt khung hình trong các game 3D. Hãy tuân thủ các nguyên tắc sau:

1. **Giới hạn số lượng Realtime Lights chiếu vào 1 Object (URP Additional Lights Limit)**:
   * Trong Forward Renderer của URP, mỗi vật thể chỉ có thể nhận tối đa **4 hoặc 8 Additional Lights** cùng lúc (thiết lập trong URP Asset). Nếu vượt quá số này, ánh sáng sẽ bị chớp tắt hoặc rớt sang tính toán Vertex rẻ tiền.
2. **Hạn chế tối đa Shadow trên Point Light**:
   * Chỉ bật Soft Shadows cho 1 Directional Light chính và 1 vài Spot Light quan trọng. Tuyệt đối tránh bật Realtime Shadows trên hàng chục chiếc Point Light cùng lúc!
3. **Phân vùng Culling Mask & Light Layers (URP)**:
   * Nếu bạn làm một ánh sáng "Rim Light" (ánh sáng ven viền làm nổi bật nhân vật chính), hãy dùng **Light Layers** để đèn này chỉ tác động lên layer `Player`, hoàn toàn bỏ qua layer `Default/Environment`.
4. **Nguyên tắc "Tĩnh thì Bake, Động thì Realtime"**:
   * 80% kiến trúc tĩnh của màn chơi (nhà cửa, đá, cây lớn, tường rào) nên được tick **`Static`** và Bake Lightmap. Vừa có chất lượng ánh sáng nảy đẳng cấp AAA, vừa tối ưu hoá dung lượng draw call!

---

## 8. Bảng Tra Cứu Nhanh (Cheat Sheet)

| Bối cảnh màn chơi | Loại đèn khuyên dùng | Color / Temp | Intensity | Shadow Type | Range / Angle |
| :--- | :--- | :---: | :---: | :---: | :---: |
| **Trưa nắng gắt (Ngoại cảnh sa mạc/rừng)** | Directional Light | 5800K (Trắng ấm nhẹ) | `1.2 - 2.0` | Soft Shadows (Strength: 0.8) | Toàn map |
| **Đêm trăng thanh u ám** | Directional Light | 8500K (Xanh tím lạnh) | `0.1 - 0.25` | Soft Shadows (Strength: 0.5) | Toàn map |
| **Đèn pin thám hiểm hầm tối** | Spot Light | 6000K (Trắng lạnh) | `2.5 - 3.5` | Soft Shadows (Bias: 0.05) | Range: 15m, Spot: 45° |
| **Đuốc lửa hầm ngục (Dungeon Torch)** | Point Light + Flicker | 2000K (Cam vàng rực) | `1.5 - 2.5` | Hard Shadows hoặc None | Range: 6 - 8m |
| **Đèn đường phố ban đêm** | Spot Light rọi xuống | 3000K (Vàng ấm) | `3.0 - 5.0` | Soft Shadows | Range: 10m, Spot: 65° |
| **Cửa sổ kính hắt sáng vào phòng** | Area Light (Baked) | 6500K (Trắng trong) | `4.0 - 8.0` | Baked Shadows (Mịn màng) | Tùy kích thước khung cửa |
