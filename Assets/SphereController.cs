using UnityEngine;

public class SphereController : MonoBehaviour
{
    public float speed = 15f;

    public float startZ = -26f;
    public float endZ = 60f;
    // Update is called once per frame
    void Update()
    {
        // trả ra kết quả luân phiên là 0> 89 > 0> 89 
        // -26 -> 63 -> -26 > 63
        float z = Mathf.PingPong(Time.time * speed, endZ- startZ) + startZ;
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }
}
