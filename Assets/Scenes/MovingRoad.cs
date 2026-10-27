using UnityEngine;

public class MovingRoad : MonoBehaviour
{
    public float roadSpeed = 10f;
    public float roadLength = 20f;
    public int totalRoad = 3;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.back * roadSpeed * Time.deltaTime);
        if (transform.position.z < -roadLength)
        {
            transform.position += new Vector3(0,0, roadLength * totalRoad);
        }
    }
}
