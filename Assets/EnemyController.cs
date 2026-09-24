using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Transform[] points;
    private Transform target;
    private int pointIndex = 0;

    public float speed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject CheckPoints = GameObject.FindGameObjectWithTag("CheckPoints");
        points = new Transform[CheckPoints.transform.childCount];
        for (int i = 0; i < CheckPoints.transform.childCount; i++)
        {
            points[i] =  CheckPoints.transform.GetChild(i).transform;
        }
        target = points[pointIndex];
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime);
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation =
                Quaternion.Slerp(transform.rotation, lookRotation, 2f * Time.deltaTime);
            // xoay tuyen tinh theo huong di chuyen 
        }

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
           pointIndex++;
           if (pointIndex >= points.Length)
           {
               Destroy(gameObject); // cong diem, tru mau nha chinh ...
               return;
           }
           target = points[pointIndex];
        }
    }
}
