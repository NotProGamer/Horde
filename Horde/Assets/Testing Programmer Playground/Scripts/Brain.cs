using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Brain : MonoBehaviour {

    // Get Nearby Objects
    public List<GameObject> m_nearbyObjects;
    private Dictionary<GameObject, Description> m_memory;

    public float m_thoughDelay = 0.5f;
    private float m_thoughtTicker = 0f;

    //[System.Flags]
    //public enum ObjectCategories
    //{
    //    Uncategorised = 0,
    //    Unrecognised = 0x01,
    //    Alive = 0x02, // Dead
    //    Enemy = 0x04, // Ally
    //    Boss = 0x08, // Standard
    //}

    public enum ObjectCategories
    {
        DeadAlly = 0,
        HealthAlly = 1,
        DeadEnemy = 2,
        HealthyEnemy = 3,
        DeadAllyBoss = 4,
        HealthAllyBoss = 5,
        DeadEnemyBoss = 6,
        HealthyEnemyBoss = 7,
        //DeadHazzard = 8,
        //HealthyHazzard = 9,
        //DeadObstacle = 16,
        //HealthyObstacle = 17,
        Uncategorised = 128,
    }

    public class Description
    {
        public ObjectCategories m_category = ObjectCategories.Uncategorised;
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
        foreach (GameObject obj in m_nearbyObjects)
        {
            // if memory contains object
            // // Evaluate it
            // else
            // // Add it to memory
            // // Evaluate it

            if (m_memory.ContainsKey(obj))
            {
                Description test;
                if (m_memory.TryGetValue(obj, out test))
                {
                    IdentifyObject(obj, out test);
                }
            }
            else
            {
                Description test = new Description();

                if (IdentifyObject(obj, out test))
                {
                    m_memory.Add(obj, test);
                }
                
            }

            ObjectCategories category = CategoriesObject(obj);
            float evaluation = 0f;
        }
    }

    ObjectCategories CategoriesObject(GameObject obj)
    {
        ObjectCategories category = ObjectCategories.Uncategorised;
        bool categorised = false;
        int test = 0;
        Health objHealth = obj.GetComponent<Health>();

        if (objHealth != null)
        {
            categorised = true;

            if (!objHealth.IsDead())
            {
                test += 1; // Alive
            }

            if (IsEnemy(obj))
            {
                test += 2; // Enemy
            }

            if (IsBoss(obj))
            {
                test += 4; // Boss
            }

            //if (IsTrap(obj))
            //{
            //    test += 8; // Trap
            //}

            //if (IsObstacle(obj))
            //{
            //    test += 16; // Obstacle
            //}
            
        }

        if (categorised)
        {
            category = (ObjectCategories)test;
        }

        return category;
    }

    bool IdentifyObject(GameObject obj, out Description description)
    {
        bool result = false;
        if (m_memory.TryGetValue(obj, out description))
        {
            description.m_category = CategoriesObject(obj);
            if (description.m_category != ObjectCategories.Uncategorised)
            {
                description.m_evaluation = EvaluateObject(obj, description.m_category);
                return true;
            }
        }
        return result;
    }

    private float EvaluateObject(GameObject obj, ObjectCategories category)
    {
        float result = 0f;

        switch (category)
        {
            case ObjectCategories.DeadAlly:
                break;
            case ObjectCategories.HealthAlly:
                break;
            case ObjectCategories.DeadEnemy:
                break;
            case ObjectCategories.HealthyEnemy:
                break;
            case ObjectCategories.DeadAllyBoss:
                break;
            case ObjectCategories.HealthAllyBoss:
                break;
            case ObjectCategories.DeadEnemyBoss:
                break;
            case ObjectCategories.HealthyEnemyBoss:
                break;
            case ObjectCategories.Uncategorised:
                break;
            default:
                break;
        }

        return result; ;
    }


    void EvaluateNearbyObjects()
    {
        for (int i = 0; i < m_nearbyObjects.Count; i++)
        {

        }
    }

    public virtual bool IsEnemy(GameObject obj)
    {
        if (Labels.Tags.IsHuman(gameObject))
        {
            return Labels.Tags.IsZombie(obj);
        }
        if (Labels.Tags.IsZombie(gameObject))
        {
            return Labels.Tags.IsHuman(obj);
        }
        return false;
    }

    public virtual bool IsBoss(GameObject obj)
    {
        return false;
    }
    public virtual bool IsTrap(GameObject obj)
    {
        return false;
    }
    public virtual bool IsObstacle(GameObject obj)
    {
        return false;
    }
}
