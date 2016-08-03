using UnityEngine;
using System.Collections;
using System.Collections.Generic;



// References: http://answers.unity3d.com/questions/324589/how-can-i-tell-when-a-navmesh-has-reached-its-dest.html
// References: http://answers.unity3d.com/questions/264150/is-there-a-built-in-notification-when-nav-mesh-age.html


public class HumanMovement : MonoBehaviour {

    private NavMeshAgent m_navMeshAgent = null;
    private Vector3 m_currentDestination/* = new Vector3()*/;
    private int m_destinationIterator;
    public List<Transform> m_patrolPoints/* = new List<Transform>()*/;
    public bool m_patrolling = true;

    private Health m_health = null;

    public Animator m_anim = null;
    void UpdateAnimator()
    {
        Vector3 velocity = new Vector3();
        if (m_navMeshAgent)
        {
            velocity = m_navMeshAgent.velocity;
        }

        if (m_anim)
        {
            m_anim.SetFloat("Velocity", velocity.magnitude);
        }
    }

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

        //m_patrolPoints = new List<Transform>();

    }

    // Use this for initialization
    void Start ()
    {
        InitialiseDestination();
    }

    void InitialiseDestination()
    {
        Transform currentPatrolPoint = null;
        m_destinationIterator = 0;

        if (m_patrolPoints.Count > 0)
        {
            currentPatrolPoint = m_patrolPoints[m_destinationIterator];
        }

        if (currentPatrolPoint != null)
        {
            m_currentDestination = m_patrolPoints[m_destinationIterator].position;
        }
        else
        {
            m_currentDestination = transform.position;
            m_patrolling = false;
            //m_currentDestination = new Vector3(0, 0, 0);
        }
    }

    private bool SetNextDestination()
    {

        bool result = false;

        // get next interator
        m_destinationIterator++;
        if (m_destinationIterator >= m_patrolPoints.Count)
        {
            m_destinationIterator = 0;
        }

        // Get Next Patrol Point
        Transform currentPatrolPoint = null;
        if (m_patrolPoints.Count > 0)
        {
            currentPatrolPoint = m_patrolPoints[m_destinationIterator];
        }
        else
        {
            Debug.Log("Patrol Point list is empty");
        }

        // if current patrol point is valid 
        if (currentPatrolPoint != null)
        {
            //set it
            m_currentDestination = m_patrolPoints[m_destinationIterator].position;
            result = true;
        }
        else
        {
            // use current position
            Debug.Log("invalid patrol point on " + gameObject.name);
            m_currentDestination = transform.position; //m_currentDestination = new Vector3(0, 0, 0);
            m_patrolling = false;
        }

        if (m_navMeshAgent)
        {
            m_navMeshAgent.SetDestination(m_currentDestination);
        }

        return result;
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
        UpdateAnimator();

        if (m_health)
        {
            if (m_health.IsDead())
            {
                //m_patrolPoints.Clear();
                //SetNextDestination();
                Stop();
                //return;
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

    private void Stop()
    {
        m_patrolling = false;
        if (m_navMeshAgent)
        {
            m_navMeshAgent.SetDestination(transform.position);
        } 
    }

    //public void FallToGround()
    //{
    //    Vector3 start = transform.position;
    //    Vector3 end = new Vector3(start.x, 0, start.z);
    //    transform.position = end; //Vector3.Lerp(start, end, 1.0f);
    //}
}
