using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HumanMovement : MonoBehaviour {

    private NavMeshAgent m_nav = null;
    public Vector3 m_currentDestination = new Vector3();
    private float m_touchRange = 1f; // may need to get this from attackRange or moveRange

    public float m_minSpeed = 3f;
    public float m_maxSpeed = 10f;
    private float m_currentSpeed = 3f;
    

    public enum State
    {
        Idle,
        Moving,
    }
    public State m_state = State.Idle;

    void Awake()
    {
        m_nav = GetComponent<NavMeshAgent>();
        if (m_nav == null)
        {
            Debug.Log("NavMeshAgent not included");
        }

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
    void Start ()
    {

        m_currentSpeed = m_minSpeed;
        m_currentDestination = transform.position;
        RequestPatrolRoute();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Patrol();
        if (m_nav)
        {
            if (ReachedDestination())
            {
                m_state = State.Idle;
            }
        }
    }

    public void SetDestination(Vector3 position)
    {
        if (m_nav)
        {
            m_state = State.Moving;
            m_currentDestination = position;
            m_nav.speed = m_currentSpeed;
            m_nav.SetDestination(m_currentDestination);
        }
    }
    private bool ReachedDestination()
    {
        bool result = false;
        if (m_nav)
        {
            result = m_nav.remainingDistance <= m_touchRange;
        }

        return result;
    }
    public void SetSpeed(float speed)
    {
        m_currentSpeed = Mathf.Clamp(speed, m_minSpeed, m_maxSpeed);
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
                else
                {
                    Debug.Log("Invalid PatrolRoute: no patrol points in " + m_patrolRoute.m_identifier + "!");
                }
            }
        }
    }

}
