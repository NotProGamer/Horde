using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
///  Code Author: Reece Howe
///  Project: Horde
///  Engine: Unity
///  Platform: Mobile and PC
///  Notes: Basic Zombie Movement Script. Zombies maybe be given multiple destinations, these are used to calculate the average destination and modify zombie movement
///  Notes: this could be further tweaked using navmesh agent , speed, acceleration, angularspeed, etc.
///  Status: Complete
/// </summary>
/// 

public class ZombieMovement : MonoBehaviour {

    private Transform m_destinationTarget;

    private NavMeshAgent m_nav = null;

    public Queue<Vector3> m_destinations = new Queue<Vector3>();
    public Vector3 currentDestination;
    public int lastCount = 0;
    public int m_maxQueueSize = 5;

    public float baseSpeed = 3.5f;
    public float baseAngularSpeed = 120;
    private bool m_recalculateDestination = false;
    public float m_destinationExpireRate = 1.0f;
    private float m_nextExpire = 0.0f;
    // calculate average destination


    private Health m_health = null;

    void OnEnable()
    {
        if (m_nav)
        {
            m_recalculateDestination = true;
            m_nav.Resume();
        }
    }

    void OnDisable()
    {
        m_destinations = new Queue<Vector3>();
    }

    void Awake()
    {
        //m_destination

        m_nav = GetComponent<NavMeshAgent>();
        if (m_nav == null)
        {
            Debug.Log("NavMeshAgent not included");
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
        // initialise current destination
        m_nav.SetDestination(transform.position);
        lastCount = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {


        // if zombie has a destination. move to destination
        if (m_nav)
        {
            if (m_health)
            {
                if (m_health.IsDead())
                {
                    m_nav.Stop();
                    return; // if dead you can't move;
                }
            }

            if (m_destinationTarget)
            {
                m_nav.SetDestination(m_destinationTarget.position);
            }


            

            // if list changed recalculate destination
                // if list empty set destination to transform.position
                //else
                    // calculate new destination
                    // update Timers

            // check for change in destination array
            if (lastCount != m_destinations.Count || m_recalculateDestination)
            {
                //m_recalculateDestination = true;
                lastCount = m_destinations.Count;
                if (m_recalculateDestination)
                {
                    m_recalculateDestination = false;
                }

                if (m_destinations.Count == 0)
                {
                    m_nav.SetDestination(transform.position);
                    m_nav.speed = baseSpeed;
                }
                else if(m_destinations.Count == 1)
                {
                    m_nav.SetDestination(m_destinations.Peek());
                    m_nav.speed = baseSpeed;
                }
                else
                {

                    Vector3 averageDestination = new Vector3(0, 0, 0);

                    foreach (Vector3 destination in m_destinations)
                    {
                        averageDestination += destination;
                    }

                    if (m_destinations.Count > 1)
                    {
                        averageDestination /= m_destinations.Count;
                    }

                    //averageDestination = averageDestination.normalized * (averageDestination.magnitude - 0.6f);

                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(averageDestination, out hit, 0.5f, NavMesh.AllAreas))
                    {
                        // be aware of y axis in that it might not find the right area
                        m_recalculateDestination = false;
                        currentDestination = hit.position;
                        Debug.Log("New Destination : " + currentDestination.ToString());
                        currentDestination.y = 0.0f;
                        m_nav.SetDestination(currentDestination);
                        m_nav.speed = baseSpeed * m_destinations.Count;
                    }
                }
            }

            if (m_destinations.Count > 1)
            {
                if (Time.time > m_nextExpire)
                {
                    RemoveDestination();
                }
            }

        }

    }

    public void SetDestination(Vector3 pDestination)
    {
        // case list 
        // Add First destination
        // Set nextExpireTime
        // Add another destination
        // don't change expire time

        if (m_destinations.Count == 0)
        {
            SetNextExpire();
        }
        m_destinations.Enqueue(pDestination);
        if(m_destinations.Count > (int)Mathf.Abs(Mathf.Max(1, m_maxQueueSize)))
        {
            RemoveDestination();
            m_recalculateDestination = true;
        }

    }

    void SetNextExpire()
    {
        m_nextExpire = Time.time + m_destinationExpireRate;
    }

    void RemoveDestination()
    {
        Vector3 removedDestination = m_destinations.Dequeue();
        Debug.Log("Dequeue: " + removedDestination.ToString());
        SetNextExpire();
    }

    //void FixedUpdate()
    //{
    //    if (m_nav)
    //    {
    //        if (testVelocity)
    //        {
    //            m_nav.velocity = m_velocity;
    //        }
    //    }

    //}

}
