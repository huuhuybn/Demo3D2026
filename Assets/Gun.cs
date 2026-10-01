using DefaultNamespace;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float dame = 100f;
    public float range = 100f;
    public float hitForce = 10f;
    public Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Fire();
        }
    }
    void Fire()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            // Xay ra su kien va cham giua 1 duong thang tu camera ra phia truoc, độ dài là range 
            Target target  = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeHit(cam.transform.forward *
                               hitForce, hit.point);
            }
        }
    }
    
}
