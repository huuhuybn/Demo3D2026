using UnityEngine;
using UnityEngine.AI;

public class LMHTController : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject clickEffectPrefab; // hieu ung click chuot
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            MoveToCursor();
        }
    }

    void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            agent.SetDestination(hit.point);
            if (clickEffectPrefab != null)
            {
                Instantiate(clickEffectPrefab, hit.point, Quaternion.identity);
            }
        }
    }
}
