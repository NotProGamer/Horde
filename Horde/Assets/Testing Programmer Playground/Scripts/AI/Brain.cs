using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Sense

public class Brain : MonoBehaviour {

    public int m_boredom = 0;
    public int m_boredomMaximum = 10;

    // Get Nearby Objects

    [System.Serializable]
    public class Sight
    {
        public float m_range = 10f;
        public LayerMask m_mask;
        public float m_delay = 1f;
    }
    public Sight m_sight;
    private const int m_maxSightResults = 100;
    private Collider[] m_sightResults = new Collider[m_maxSightResults];

    [System.Serializable]
    public class Hearing
    {
        public float m_range = 10f;
        // perhaps have a mask to filter out unwanted noises.
        public float m_delay = 1f;
    }
    public Hearing m_hearing;

    [System.Serializable]
    public class Response
    {
        public float range = 10f;
        // perhaps have a mask to filter out unwanted assignments.
        public float m_delay = 1f;
    }
    public Response m_responsiveness;
    public List<GameObject> m_nearbyObjects;
    public List<Noise> m_nearbyNoises;
    public List<Assignment> m_nearbyAssignments;

    //public float m_thoughDelay = 0.5f;
    //private float m_thoughtTicker = 0f;

    private Health m_healthScript = null;
    private GameObject m_gameController = null;
    private NoiseManager m_noiseManagerScript = null;

    void Awake()
    {
        m_healthScript = GetComponent<Health>();
        if (m_healthScript == null)
        {
            Debug.Log("Health Not included!");
        }

        m_gameController = GameObject.FindGameObjectWithTag(Labels.Tags.GameController);
        if (m_gameController == null)
        {
            Debug.Log("Health Not included!");
        }
        else
        {
            m_noiseManagerScript = m_gameController.GetComponent<NoiseManager>();
            if (m_noiseManagerScript == null)
            {
                Debug.Log("Health Not included!");
            }
        }

    }


    private float m_lookTicker = 0f;
    private float m_hearingTicker = 0f;
    private float m_responseTicker = 0f;


    // Use this for initialization
    void Start ()
    {
        m_lookTicker = Random.Range(0f, m_sight.m_delay);
        m_hearingTicker = Random.Range(0f, m_hearing.m_delay);
        m_responseTicker = Random.Range(0f, m_responsiveness.m_delay);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Time.time > m_lookTicker)
        {
            m_lookTicker = Time.time + m_sight.m_delay;
            Look();
        }
        if (Time.time > m_hearingTicker)
        {
            m_hearingTicker = Time.time + m_hearing.m_delay;
            Listen();
        }
        if (Time.time > m_responseTicker)
        {
            m_responseTicker = Time.time + m_responsiveness.m_delay;
            RequestAssignments();
        }

        /// Sense 
        // Look
        // Listen
        // Request Assignment


        /// Think
        // Memory Update
        // Add new objects to memory
        // Remove inactive Objects from memory
        // Categories Objects in memory
        // Update counts in memory

        // Evaluate Objects in memory

        /// Act 

        /// ****************************

        /// Evaluate Death
        // Store Current Decision

        // If Current Decision < Enemy Base
        //  /// Sense Look
        //  // Look for nearby GameObjects
        //  // Add New GameObjects To Memory
        //  // Evaluate GameObjects In Memory
        //  // Store Current Decision

        // If Current Decision < Investigate Base
        //  /// Listen for nearby sounds
        //  // Add New Noises To Memory2
        //  // Evaluate Noises In Memory2
        //  // Store Current Decision

        // if Current Decision < Patrol Base
        //  /// Check for Assignment (Guard or Patrol)
        //  // Add New Assignment To Memory3
        //  // Evaluate Assignment In Memory3
        //  // Store Current Decision

        // if Current Decision < Idle Base
        //  // Evaluate Boredom
        //  // Store Current Decision

        /// Decisions
        // Confirm Current Decision is > 0

        /// Act
        // Run Current Decision
    }


    void Look()
    {
        // Initialise Interactable Object Count
        int count = 0;

        count = Physics.OverlapSphereNonAlloc(transform.position, m_sight.m_range, m_sightResults, m_sight.m_mask);

        if (count > 0)
        {
            m_nearbyObjects.Clear();
            for (int i = 0; i < count; i++)
            {
                // possibly filter any unwanted objects
                m_nearbyObjects.Add(m_sightResults[i].gameObject);
            }
        }


    }

    public bool GetNearbyObjects(out List<GameObject> objects)
    {
        bool result = false;
        if (m_nearbyObjects.Count > 0)
        {
            objects = new List<GameObject>();

            objects.AddRange(m_nearbyObjects);

            result = true;
        }
        else
        {
            objects = null;
        }
        return result;
    }

    public bool GetAudibleNoises(out List<Noise> objects)
    {
        bool result = false;
        if (m_nearbyObjects.Count > 0)
        {
            objects = new List<Noise>();

            objects.AddRange(m_nearbyNoises);

            result = true;
        }
        else
        {
            objects = null;
        }
        return result;
    }

    public bool GetNearbyAssignments(out List<Assignment> objects)
    {
        bool result = false;
        if (m_nearbyObjects.Count > 0)
        {
            objects = new List<Assignment>();

            objects.AddRange(m_nearbyAssignments);

            result = true;
        }
        else
        {
            objects = null;
        }
        return result;
    }

    void Listen()
    {
        m_nearbyNoises.Clear();

        if (m_noiseManagerScript)
        {
            m_noiseManagerScript.GetAudibleNoisesAtLocation(m_nearbyNoises, transform.position, m_hearing.m_range);
        }
    }

    //public List<Noise> GetAudibleNoises()
    //{
    //    return m_nearbyNoises;
    //}
    //public List<GameObject> GetNearbyObjects()
    //{
    //    return m_nearbyObjects;
    //}
    //public List<Assignment> GetNearbyAssignments()
    //{
    //    return m_nearbyAssignments;
    //}

    void RequestAssignments()
    {

    }

}
