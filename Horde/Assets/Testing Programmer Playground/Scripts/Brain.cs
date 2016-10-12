using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Sense

public class Brain : MonoBehaviour {

    
    // Get Nearby Objects
    public List<GameObject> m_nearbyObjects;
    public List<Noise> m_nearbyNoises;
    public List<Assignment> m_nearbyAssignments;

    public float m_thoughDelay = 0.5f;
    private float m_thoughtTicker = 0f;

    private Health m_health = null;

    void Awake()
    {
        m_health = GetComponent<Health>();
        if (m_health == null)
        {
            Debug.Log("Health Not included!");
        }
    }


    // Use this for initialization
    void Start ()
    {
        m_thoughtTicker = Random.Range(0f, m_thoughDelay);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Time.time > m_thoughtTicker)
        {
            m_thoughtTicker = Time.time + m_thoughDelay;





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

    }


    void Look()
    {
    }

    void Listen()
    {
    }

    void RequestAssignments()
    {

    }

}
