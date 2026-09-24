using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject prefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Hàm gọi hàm Spawn, 1 giây 1 lần, lặp lại , bắt đầu ngay khi chạy chương trình 
        InvokeRepeating(nameof(Spawn), 0, 1);
    }

    void Spawn()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
