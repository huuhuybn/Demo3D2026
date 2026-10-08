using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    [Header("Mục tiêu cần đi tới")]
    public Transform target;
    
    public enum AIState { Patrol, Chase }  
    public AIState currentState = AIState.Patrol;
    
    // Cau hinh tuan tra 
    public Transform[] waypoints;
    private int currentWaypoint = 0;
    public float patrolSpeed = 3f;

    public Transform targetPlayer;
    public float chaseDistance = 10f;
    public float chaseSpeed = 6f;
    
    NavMeshAgent navMeshAgent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        /*if (target != null)
        {
            navMeshAgent.SetDestination(target.position); // Di chuyen toi vi tri muc tieu 
        }*/
        if (targetPlayer == null)
        {
            targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (waypoints.Length > 0)
        {
            navMeshAgent.speed = patrolSpeed;
            navMeshAgent.SetDestination(waypoints[currentWaypoint].position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case AIState.Patrol:
                DoPatrol();
                break;
            case AIState.Chase:
                DoChase();
                break;
        }
    }

    void DoPatrol()
    {
        if (targetPlayer != null && Vector3.Distance(transform.position, targetPlayer.position) < chaseDistance)
        {
            currentState = AIState.Chase;
            navMeshAgent.speed = chaseSpeed;
            return;
        }
        // Neu da di den gan diem waypoint hien tai 
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            currentWaypoint++;
            if (currentWaypoint > waypoints.Length)
            {
                currentWaypoint = 0;
            }
            navMeshAgent.SetDestination(waypoints[currentWaypoint].position);
        }
    }

    void DoChase()
    {
        if (targetPlayer != null)
        {
            navMeshAgent.SetDestination(targetPlayer.position);
            float distance = Vector3.Distance(transform.position, targetPlayer.position);
            if (distance < chaseDistance * 1.5f)
            {
                currentState = AIState.Patrol;
                navMeshAgent.speed = patrolSpeed;
                if (waypoints.Length > 0)
                {
                    navMeshAgent.SetDestination(waypoints[currentWaypoint].position);
                }
            }

        }
    }
}
