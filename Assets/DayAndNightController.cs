using UnityEngine;

public class DayAndNightController : MonoBehaviour
{
    private Light sunLight;
    public float dayDurationInSeconds = 120f;
    public float timeOfDay = 12f;
    public Gradient sunColorGradient;
    public AnimationCurve sunCurve;
    
    public GameObject[] nightLights; // hàng bóng đèn 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sunLight = GetComponent<Light>();
    }
    // Update is called once per frame
    void Update()
    {
        // tính thời gian trôi qua theo mỗi khung hình 
        timeOfDay += (Time.deltaTime / dayDurationInSeconds) * 24f;
        if (timeOfDay >= 24f) timeOfDay = 0f;
        
        float normalizedTime = timeOfDay / 24f; 
        // xoay nguồn sáng theo giờ 
        // 6h  0 dộ , 12h 90 độ , 18h 180 độ 
        float sunAngle = (normalizedTime * 360f) - 90f;
        sunLight.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);
        
        //  màu và cường độ ánh sáng tính theo normalizedTime
        sunLight.color = sunColorGradient.Evaluate(normalizedTime);
        sunLight.intensity = sunCurve.Evaluate(normalizedTime);
        
        bool isNight = (timeOfDay >= 18.5f || timeOfDay <= 5.5f);
        // Bat den 
        
        
    }
}
