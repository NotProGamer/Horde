using UnityEngine;
using System.Collections;
using System.Collections.Generic;



// References: http://answers.unity3d.com/questions/324589/how-can-i-tell-when-a-navmesh-has-reached-its-dest.html
// References: http://answers.unity3d.com/questions/264150/is-there-a-built-in-notification-when-nav-mesh-age.html


public class HumanMovement : MonoBehaviour {

    private NavMeshAgent m_navMeshAgent = null;
    private Vector3 m_currentDestination;
    private int m_destinationIterator;
    public List<Transform> m_patrolPoints;
    public bool m_patrolling = true;

    private Health m_health = null;



    void Awake()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        if (m_navMeshAgent == null)
        {
            Debug.Log("navMeshAgent not included");
        }
        m_health = GetComponent<Health>();
        if (m_health == null)
        {
            Debug.Log("Health not included");
        }

    }

    // Use this for initialization
    void Start ()
    {
        InitialiseDestination();
    }

    void InitialiseDestination()
    {
        if (m_patrolPoints.Count > 0)
        {
            m_destinationIterator = 0;
            m_currentDestination = m_patrolPoints[m_destinationIterator].position;
        }
        else
        {
            m_currentDestination = transform.position;
            //m_currentDestination = new Vector3(0, 0, 0);
        }

    }

    void OnEnable()
    {
        InitialiseDestination();
    }


    void OnDisable()
    {
        m_patrolPoints = new List<Transform>();
        m_patrolling = false;
    }
    // Update is called once per frame
    void Update ()
    {
        if (m_health)
        {
            if (m_health.IsDead())
            {
                m_patrolPoints.Clear();
                SetNextDestination();
                return;
            }
        }
        if (m_patrolling)
        {
            if (ReachedDestination())
            {
                SetNextDestination();
            }
        }
    }

    private bool ReachedDestination()
    {
        float distanceFromDestination = m_navMeshAgent.remainingDistance;
        return distanceFromDestination != Mathf.Infinity && m_navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && distanceFromDestination <= float.Epsilon;
    }

    private bool SetNextDestination()
    {
        if (m_patrolPoints.Count < 1)
        {
            m_currentDestination = transform.position;
            if (m_navMeshAgent)
            {
                m_navMeshAgent.SetDestination(m_currentDestination);
            }
            return false; // early exit
        }

        m_destinationIterator++;
        if (m_destinationIterator >= m_patrolPoints.Count)
        {
            m_destinationIterator = 0;
        }

        m_currentDestination = m_patrolPoints[m_destinationIterator].position;
        if (m_navMeshAgent)
        {
            m_navMeshAgent.SetDestination(m_currentDestination);
        }

        return true;
    }

}
