using UnityEngine;
using System.Collections;

public class Wander : BaseBehaviour {

    private Movement m_movementScript = null;
    //private NavMeshAgent m_nav = null;

    private float wanderRadius = 10f;


    public Wander(GameObject pParent):base(pParent)
    {
        if (m_parent == null)
        {
            Debug.Log("Parent game Object not included.");
            return; // early exit
        }
        m_movementScript = m_parent.GetComponent<Movement>();
        if (m_movementScript == null)
        {
            Debug.Log("Movement Script not included");
        }
    }

    // Update is called once per frame
    public override Status Update()
    {
        if (m_parent == null)
        {
            Debug.Log("Parent GameObject not included.");
            return Status.FAILURE; // early exit
        }

        // if arrived at destination
        // calculate nearby desitination, 
        // find closest destination is on nav mesh
        // head to the destination

        if (m_movementScript)
        {
            if (m_movementScript.ReachedDestination())
            {
                if (SetNextDestination() == false)
                {
                    Debug.Log("Wander Failed");
                    return Status.FAILURE;
                }
            }
        }

        //Debug.Log("Wander");
        return Status.SUCCESS;
    }
    public bool SetNextDestination()
    {
        bool result = false;
        Vector3 destination = m_parent.transform.position;

        destination += Random.insideUnitSphere * wanderRadius;
        NavMeshHit hit;
        //check for collision on walkable (0) navmesh
        if (NavMesh.SamplePosition(destination, out hit, /*100f */wanderRadius * 2.1f, NavMesh.AllAreas)) 
        {
            destination = hit.position;
            m_movementScript.SetDestination(destination);
            result = true;
        }
        return result;
    }

}
