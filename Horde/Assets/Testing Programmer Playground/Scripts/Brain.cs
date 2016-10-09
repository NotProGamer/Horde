using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Brain : MonoBehaviour {

    // Get Nearby Objects
    public List<GameObject> m_nearbyObjects;
    private Dictionary<GameObject, Description> m_memory;

    public float m_thoughDelay = 0.5f;
    private float m_thoughtTicker = 0f;

    public enum ObjectCategories
    {
        Uncategorised = 0,
        HealthyEnemy,
        DeadEnemy,
        HealthAlly,
        DeadAlly,
        HealthyEnemyBoss,
        DeadEnemyBoss,
        HealthAllyBoss,
        DeadAllyBoss,
        //HealthyHazzard,
        //DeadHazzard,
        //HealthyObstacle,
        //DeadObstacle,
        //UserTap = 128,
    }

    public class Description
    {
        public ObjectCategories m_catergory = ObjectCategories.Uncategorised;
        public float m_evaluation = 0f;

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

            

        }
	
	}

    void IdentifyObjects()
    {
        // Healthy  Alive   Enemy
        // Healthy  Dead    Enemy
        // Healthy  Alive   Ally
        // Healthy  Dead    Ally
        // 
    }


    void EvaluateNearbyObjects()
    {
        for (int i = 0; i < m_nearbyObjects.Count; i++)
        {

        }
    }
}
