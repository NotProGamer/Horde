using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    protected NavMeshAgent m_nav = null;
    public Vector3 m_currentDestination = new Vector3();
    public float m_touchRange = 1f; // may need to get this from attackRange or moveRange

    public float m_minSpeed = 3f;
    public float m_maxSpeed = 10f;
    protected float m_currentSpeed = 3f;


    public enum State
    {
        Idle,
        Moving,
    }
    public State m_state = State.Idle;

    protected void Awake()
    {
        m_nav = GetComponent<NavMeshAgent>();
        if (m_nav == null)
        {
            Debug.Log("NavMeshAgent not included");
        }
    }

    // Use this for initialization
    protected void Start()
    {

        m_currentSpeed = m_minSpeed;
        m_currentDestination = transform.position;

    }

    // Update is called once per frame
    protected void Update()
    {
        if (m_nav)
        {
            if (ReachedDestination())
            {
                m_state = State.Idle;
            }
        }
        

    }

    void MoveToPartialPath()
    {
        Debug.Log("Move To partial path");
        if (m_nav.pathStatus == NavMeshPathStatus.PathPartial)
        {
            int lastCornerIndex = m_nav.path.corners.Length;
            if (lastCornerIndex > 0)
            {
                Vector3 test = m_nav.path.corners[lastCornerIndex - 1];
                SetDestination(test);
            }

        }

    }

    
    public void SetDestination(Vector3 position)
    {
        if (m_nav)
        {
            
            m_state = State.Moving;
            m_nav.speed = m_currentSpeed;
            m_currentDestination = position;
            m_nav.SetDestination(m_currentDestination);

            //if (m_currentDestination != position)
            //{
            //    NavMeshPath path = new NavMeshPath();
            //    m_nav.CalculatePath(position, path);
            //    if (path.status == NavMeshPathStatus.PathPartial)
            //    {
            //        int lastCornerIndex = m_nav.path.corners.Length;
            //        if (lastCornerIndex > 0)
            //        {
            //            Vector3 partialPosition = m_nav.path.corners[lastCornerIndex - 1];
            //            NavMeshHit hit;
            //            if (NavMesh.SamplePosition(partialPosition, out hit, 100f, NavMesh.AllAreas))
            //            {
            //                if (m_currentDestination != hit.position)
            //                {
            //                    Debug.Log("Partial Path Destination Selected");
            //                    m_currentDestination = hit.position;
            //                    m_nav.SetDestination(m_currentDestination);
            //                }
            //                else
            //                {
            //                    Debug.Log("No change to partial path");
            //                }
            //            }
            //            else
            //            {
            //                Debug.Log("could not locate navmesh point");
            //            }
            //        }
            //        else
            //        {
            //            Debug.Log("no Corners");
            //        }
            //    }
            //    else if (path.status == NavMeshPathStatus.PathComplete)
            //    {
            //        Debug.Log("Path Destination Selected");
            //        m_currentDestination = position;
            //        m_nav.SetDestination(m_currentDestination);
            //    }
            //    else
            //    {
            //        Debug.Log("Path Destination Invalid");
            //    }
            //    if (m_nav.path.status == NavMeshPathStatus.PathPartial)
            //    {
            //        Debug.Log("Still Partial Path Destination Selected");
            //    }
        //}
        }
    }
    public bool ReachedDestination()
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
    public float GetSpeed()
    {
        return m_currentSpeed;
    }
}
