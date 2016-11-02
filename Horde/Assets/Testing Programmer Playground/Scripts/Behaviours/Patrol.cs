using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Patrol : BaseBehaviour
{
    private Movement m_movementScript = null;
    private Brain m_brainScript = null;
    private Transform m_currentDestinationTransform = null;

    public Patrol(GameObject pParent):base(pParent)
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

        m_brainScript = m_parent.GetComponent<Brain>();
        if (m_brainScript == null)
        {
            Debug.Log("Brain Script not included");
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

         // go to patrol point
         // if destination reached
         // next


        // if arrived at destination
        // calculate nearby desitination, 
        // find closest destination is on nav mesh
        // head to the destination

        if (m_movementScript && m_brainScript)
        {

            Vector3 destination = m_parent.transform.position;
            List<Assignment> assignment;
            if (m_brainScript.GetNearbyAssignments(out assignment))
            {
                if (assignment != null)
                {
                    if (m_currentDestinationTransform != null)
                    {
                        if (m_movementScript.ReachedDestination())
                        {
                            assignment[0].NextTask();
                        }

                    }

                    Transform taskTransform = assignment[0].GetCurrent();
                    if (taskTransform != null)
                    {
                        destination = taskTransform.position;
                        m_currentDestinationTransform = taskTransform;
                    }
                    else
                    {
                        // invalid task transform
                        m_currentDestinationTransform = null;
                    }
                }
                else
                {
                    // invalid assignment
                    m_currentDestinationTransform = null;
                }
            }


            NavMeshHit hit;
            //check for collision on walkable (0) navmesh
            if (NavMesh.SamplePosition(destination, out hit, 100f, NavMesh.AllAreas))
            {
                destination = hit.position;
                m_movementScript.SetDestination(destination);
            }
        }
        else
        {
            Debug.Log("Patrol Failed");
            return Status.FAILURE;
        }

        //Debug.Log("Wander");
        return Status.SUCCESS;
    }
    //public bool GoToPatrolPoint()
    //{
    //    bool result = false;

    //    return result;
    //}
}
