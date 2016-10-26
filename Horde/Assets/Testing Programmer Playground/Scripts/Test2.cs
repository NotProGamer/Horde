using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Test2 : MonoBehaviour {

    public List<Transform> m_patrolPoints;
    public int m_currentPatrolIndex = 0;
    private NavMeshAgent m_navAgent = null;
    public Transform m_currentDestination;
    void Awake()
    {
        m_navAgent = GetComponent<NavMeshAgent>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (ReachedDestination())
        {
            m_currentDestination = GetNextPatrolPoint();
            m_navAgent.SetDestination(m_currentDestination.position);
        }
	}

    private bool ReachedDestination()
    {
        return m_navAgent.remainingDistance < 1.0f;
    }

    private Transform GetNextPatrolPoint()
    {
        if (m_patrolPoints.Count <= 0)
        {
            return null;
        }
        m_currentPatrolIndex++;
        if (m_currentPatrolIndex >= m_patrolPoints.Count)
        {
            m_currentPatrolIndex = 0;
        }
        return m_patrolPoints[m_currentPatrolIndex];
    }
}
