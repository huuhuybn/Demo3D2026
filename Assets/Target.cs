using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Target : MonoBehaviour
    {
        public int scoreValue = 10;
        Rigidbody rb;
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void TakeHit(Vector3 hitForce, Vector3 hitPoint)
        {
            rb.AddForceAtPosition(hitForce , 
                hitPoint, ForceMode.Impulse);
        }
    }
}