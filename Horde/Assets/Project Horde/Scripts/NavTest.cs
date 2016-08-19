using UnityEngine;
using System.Collections;

public class NavTest : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform dest1;
    public Transform dest2;

    // Use this for initialization
    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(dest1.position);
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (gameObject.transform.position.z == dest1.position.z && gameObject.transform.position.x == dest1.position.x)
        {
            agent.SetDestination(dest2.position);
        }

        if (gameObject.transform.position.z == dest2.position.z && gameObject.transform.position.x == dest2.position.x)
        {
            agent.SetDestination(dest1.position);
        }


    }
}
