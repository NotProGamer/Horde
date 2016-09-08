using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HumanMovement : Movement {

    new void Awake()
    {
        base.Awake();
        GameObject obj = GameObject.FindGameObjectWithTag(Labels.Tags.GameController);
        if (obj)
        {
            m_patrolManagerScript = obj.GetComponent<PatrolManager>();
            if (m_patrolManagerScript == null)
            {
                Debug.Log("PatrolManager not included!");
            }
        }
        else
        {
            Debug.Log("GameController not included!");
        }
    }

    // Use this for initialization
    new void Start ()
    {
        base.Start();
        RequestPatrolRoute();
    }

    // Update is called once per frame
    new void Update ()
    {
        Patrol();
        base.Update();
    }



    // The following code is more to do with behaviour and will later be move out of this script

    public PatrolManager.PatrolRoute m_patrolRoute = null;
    public string m_patrolIdentifier = "";
    private PatrolManager m_patrolManagerScript = null;
    public int m_currentPatrolPointIndex = -1;
    
    void RequestPatrolRoute()
    {
        m_patrolRoute = m_patrolManagerScript.GetPatrolRoute(m_patrolIdentifier);
    }

    private void Patrol()
    {
        
        if (m_patrolRoute != null)
        {
            if (!m_patrolRoute.IsEmpty())
            {
                if (ReachedDestination())
                {
                    Vector3 nextDestination = transform.position;
                    // set current patrol index
                    m_currentPatrolPointIndex++;
                    if (m_currentPatrolPointIndex >= m_patrolRoute.m_patrolPoints.Count)
                    {
                        m_currentPatrolPointIndex = 0;
                    }
                    // get current patrol transform
                    Transform nextTransform = m_patrolRoute.m_patrolPoints[m_currentPatrolPointIndex];
                    // set next destination
                    if (nextTransform != null)
                    {
                        nextDestination = nextTransform.position;
                        SetDestination(nextDestination);
                    }
                    else
                    {
                        Debug.Log("Invalid PatrolPoint: no patrol point at index: "+ m_currentPatrolPointIndex + "!");
                    }
                    // Set Destination
                    //SetDestination(nextDestination);
                }
            }
            else
            {
                Debug.Log("Invalid PatrolRoute: no patrol points in " + m_patrolRoute.m_identifier + "!");
            }
        }
    }

}
