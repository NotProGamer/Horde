using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    protected NavMeshAgent m_nav = null;
    public Vector3 m_currentDestination = new Vector3();
    public float m_touchRange = 1f; // may need to get this from attackRange or moveRange

    public float m_minSpeed = 3f;
    public float m_maxSpeed = 20f; // was 10
    protected float m_currentSpeed = 3f;

    private float m_angularVelocity = 0;
    public float m_currentAngle = 0;

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

        //m_nav.updateRotation = false;
    }


    bool test = false;
    // Update is called once per frame
    protected void Update()
    {
        if (m_nav)
        {
            if (ReachedDestination())
            {
                m_state = State.Idle;
            }

            //transform.LookAt(m_currentDestination, Vector3.up);
            if (test)
            {
                if (m_currentDestination != transform.position)
                {
                    //Vector3 lookTarget = m_currentDestination;
                    //lookTarget.y = transform.position.y;

                    //transform.LookAt(lookTarget, Vector3.up);

                    //FaceMovementDirection();
                }

                //transform.LookAt(m_currentDestination, Vector3.up);
            }
        }


    }

    void LateUpdate()
    {
        if (m_currentDestination != transform.position)
        {
            FaceMovementDirection();
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

    public bool m_unableToReachCurrentDestination = false;

    public void SetDestination(Vector3 position)
    {
        if (m_nav)
        {
            m_nav.Resume();
            if (m_currentDestination != position)
            {
                m_state = State.Moving;
                m_nav.speed = m_currentSpeed;
                m_currentDestination = position;
                //if (Labels.Tags.IsZombie(gameObject))
                //{
                //    //Debug.Log(m_currentDestination.ToString() + " Set Destination");
                //}

                transform.LookAt(m_currentDestination);

                m_nav.SetDestination(m_currentDestination);
                test = true;
            }

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

            if (Labels.Tags.IsZombie(gameObject) )
            {
                if (m_nav.path.status == NavMeshPathStatus.PathPartial)
                {
                    //Debug.Log("Still Partial Path Destination Selected");
                    m_unableToReachCurrentDestination = true;
                }
                else
                {
                    m_unableToReachCurrentDestination = false;
                }
            }
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

    public void Stop()
    {
        SetDestination(transform.position);
    }

    //public bool CheckPathBlocked()
    //{
    //    return m_nav.pathStatus == NavMeshPathStatus.PathPartial;
    //}
    //public bool CheckAtEndOfBlockedPath()
    //{
    //    return m_nav.pathStatus == NavMeshPathStatus.PathPartial && !m_nav.hasPath;
    //}
    //private float timer = 1.0f;
    //private float delay = 1.0f;
    //public bool RecalculatePath()
    //{
    //    bool result = false;
    //    if (timer < Time.time)
    //    {
    //        if (CheckPathBlocked())
    //        {
    //            // reset Destination
    //            m_nav.ResetPath();
    //            SetDestination(m_currentDestination);
    //            result = true;
    //        }
    //        timer = delay + Time.time;
    //    }
    //    return result;
    //}

    //public void Test()
    //{
    //    m_nav.ResetPath();
    //}

    public bool IsMoving()
    {
        //return m_nav.velocity.sqrMagnitude != 0;
        return m_nav.velocity.sqrMagnitude > 1f;
    }

    public void FaceMovementDirection()
    {
        Vector3 lookTarget;
        if ((transform.position - m_nav.destination).magnitude <= m_nav.stoppingDistance)
        {
            lookTarget = m_nav.destination;
        }
        else
        {
            lookTarget = m_nav.steeringTarget;
        }

        //lookTarget = m_nav.steeringTarget;
        lookTarget.y = transform.position.y;
        //transform.LookAt(lookTarget, Vector3.up);

        
        lookTarget = lookTarget - transform.position;
        float desiredAngle = Mathf.Rad2Deg * Mathf.Atan2(lookTarget.x, lookTarget.z);
        //float currentAngle = transform.eulerAngles.y;
        //float deltaAngle = desiredAngle - currentAngle;
        //if (deltaAngle > 180)
        //    deltaAngle -= 360;
        //if (deltaAngle < -180)
        //    deltaAngle += 360;

        //if (deltaAngle > 0)
        //    currentAngle += Mathf.Min(deltaAngle, 1);
        //if(deltaAngle < 0)
        //    currentAngle += Mathf.Max(deltaAngle, -1);

        //currentAngle += Mathf.DeltaAngle(currentAngle, Mathf.Lerp(currentAngle, desiredAngle, 0.1f));

        m_currentAngle = Mathf.SmoothDampAngle(m_currentAngle, desiredAngle, ref m_angularVelocity, 0.25f);

        transform.eulerAngles = new Vector3(0, m_currentAngle, 0);


        //Quaternion newRot = Quaternion.LookRotation(lookTarget);
        //transform.rotation = newRot; // Quaternion.Lerp(transform.rotation, newRot, 0.5f);

    }
}
