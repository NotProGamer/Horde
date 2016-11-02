using UnityEngine;
using System.Collections;

public class Flee : BaseBehaviour
{
    private Movement m_movementScript = null;
    private HumanEvaluations m_humanEvaluationsScript = null;

    private float m_fleeDistance = 100.0f;
    // Use this for initialization
    //void Start () {	}

    // Update is called once per frame
    //void Update () {	}

    public Flee(GameObject pParent) : base(pParent)
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
        m_humanEvaluationsScript = m_parent.GetComponent<HumanEvaluations>();
        if (m_humanEvaluationsScript == null)
        {
            Debug.Log("HumanEvaluations Script not included");
        }
    }

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

        if (m_movementScript && m_humanEvaluationsScript)
        {
            if (m_movementScript.ReachedDestination() /*|| m_movementScript.CheckPathBlocked()*/)
            {
                if (SetNextDestination() == false)
                {
                    Debug.Log("Flee Failed");
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
        
        destination += m_humanEvaluationsScript.m_threatDirection * m_fleeDistance;
        NavMeshHit hit;
        //check for collision on walkable (0) navmesh
        if (NavMesh.SamplePosition(destination, out hit, m_fleeDistance, NavMesh.AllAreas))
        {
            destination = hit.position;
            m_movementScript.SetDestination(destination);
            result = true;
        }
        return result;
    }
}
