using System.Collections;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public static Transform[] points;
    
    public GameObject Tank;
    public GameObject FlyingTank;
    public Transform spawnPoint;

    public int enemyCount = 5;
    public float timeBetweenSpawns = 5f;
    public float timeBeforeStart = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject CheckPoints = GameObject.FindGameObjectWithTag("CheckPoints");
        points = new Transform[CheckPoints.transform.childCount];
        for (int i = 0; i < CheckPoints.transform.childCount; i++)
        {
            points[i] =  CheckPoints.transform.GetChild(i).transform;
        }
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    { 
        yield return new WaitForSeconds(timeBeforeStart);
        for (int i = 0; i < enemyCount; i++)
        {
            Instantiate(FlyingTank, spawnPoint.position, spawnPoint.rotation);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
        
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
