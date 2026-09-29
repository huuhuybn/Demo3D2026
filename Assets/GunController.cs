using System;
using DefaultNamespace;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject BulletPrefab;
    // Attack : Tan Cong
    // Idle : dung yen
    public float attackRange = 3f;
    public string enemyTag = "Enemy";
    public GunState currentState = GunState.Idle;
    public Transform target;
    SphereCollider detectCollider;
    void Start()
    {
        detectCollider = GetComponent<SphereCollider>();
        detectCollider.isTrigger = true;
        detectCollider.radius = attackRange;
    }
    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GunState.Idle:
                break;
            case GunState.Attack:
                if (target != null)
                {
                    transform.LookAt(target); // nhìn theo 
                    Fire();
                }
                break;
        }
    }

    private void Fire()
    {
        GameObject bulletGo = 
            Instantiate(BulletPrefab, transform.position, 
                Quaternion.identity);
        BulletController  bullet = 
            bulletGo.GetComponent<BulletController>();
        bullet.Seek(target);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag) && target == null)
        {
            target = other.transform;
            currentState = GunState.Attack;
            Debug.Log("Attack");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(enemyTag)  && other.transform == target)
        {
            target = null;
            currentState = GunState.Idle;
            Debug.Log("Idle");
        }
    }
}
