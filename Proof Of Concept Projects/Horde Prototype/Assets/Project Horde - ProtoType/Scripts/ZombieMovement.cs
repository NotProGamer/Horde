using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieMovement : MonoBehaviour {

    private Transform m_destinationTarget;

    private NavMeshAgent m_nav;

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

    
    


    void Awake()
    {
        //m_destination

        m_nav = GetComponent<NavMeshAgent>();
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
            if (lastCount != m_destinations.Count)
            {
                //m_recalculateDestination = true;
                lastCount = m_destinations.Count;

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
                        Debug.Log("New Destination : " + currentDestination.ToString());
                        currentDestination = hit.position;
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
                    SetNextExpire();
                    Vector3 removedDestination = m_destinations.Dequeue();
                    Debug.Log("Dequeue: " + removedDestination.ToString());
                }

                
            }





            //if (m_destinations.Count > 0)
            //{
            //    if (m_recalculateDestination)
            //    {
            //        // calculate average destination
            //        // clamp destination in bounds
            //        // by finding nearest point on navmesh
            //        Vector3 averageDestination = new Vector3(0, 0, 0);
            //        foreach (Vector3 destination in m_destinations)
            //        {
            //            averageDestination += destination;
            //        }
            //        if (m_destinations.Count > 1)
            //        {
            //            averageDestination /= m_destinations.Count;
            //        }
            //        //averageDestination = averageDestination.normalized * (averageDestination.magnitude - 0.6f);
            //        NavMeshHit hit;
            //        if (NavMesh.SamplePosition(averageDestination, out hit, 0.5f, NavMesh.AllAreas))
            //        {
            //            // be aware of y axis in that it might not find the right area
            //            m_recalculateDestination = false;
            //            Debug.Log("New Destination : " + currentDestination.ToString());
            //            currentDestination = hit.position;
            //            currentDestination.y = 0.0f;
            //            m_nav.SetDestination(currentDestination);
            //            m_nav.speed = baseSpeed * m_destinations.Count;
            //        }
            //    }
            //}
            //else
            //{
            //    m_nav.SetDestination(transform.position);
            //}


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
            Vector3 removedDestination = m_destinations.Dequeue();
            Debug.Log("Dequeue: " + removedDestination.ToString());
        }


        //m_recalculateDestination = true;
        //if (m_nextExpire <= Time.time)
        //{
        //    m_nextExpire = Time.time + m_nextExpire;
        //}

    }

    void SetNextExpire()
    {
        m_nextExpire = Time.time + m_destinationExpireRate;
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
