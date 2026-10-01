using System;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Transform target;
    public float speed = 20f;
    public int damage = 10;

    public void Seek(Transform target)
    {
        this.target = target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector3 direction = target.position - 
                            transform.position;
      transform.Translate(
          direction.normalized * speed * 
          Time.deltaTime, Space.World);
      transform.LookAt(target);
    }

    void HitTarget()
    {
        
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            HitTarget();
        }
    }
}
