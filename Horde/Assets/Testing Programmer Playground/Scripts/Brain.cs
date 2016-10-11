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

        CoverSafe = 32,
        CoverComprimised = 33,
        CoverUnsafe = 34,

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


            /// Sense Look
            // Look for nearby GameObjects
            // Add New GameObjects To Memory
            // Evaluate GameObjects In Memory
            // Store Current Decision

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

    void IdentifyObjects()
    {
        foreach (GameObject obj in m_nearbyObjects)
        {
            // if memory contains object
            // // Evaluate it
            // else
            // // Evaluate it
            // // Add it to memory

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

            //ObjectCategories category = CategoriesObject(obj);
            //float evaluation = 0f;
        }
    }

    protected virtual ObjectCategories CategoriesObject(GameObject obj)
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
        else if (IsCover(obj))
        {
            categorised = true;

            if (IsSafe(obj))
            {
                // Safe
                test = (int)ObjectCategories.CoverSafe;
            }
            else if (IsComprimised(obj))
            {
                // Comprimised
                test = (int)ObjectCategories.CoverComprimised;
            }
            else
            {
                // Unsafe
                test = (int)ObjectCategories.CoverUnsafe;
            }
        }


        if (categorised)
        {
            category = (ObjectCategories)test;
        }

        return category;
    }

    //protected virtual ObjectCategories CategoriesObject(GameObject obj)
    //{
    //    ObjectCategories category = ObjectCategories.Uncategorised;
    //    bool categorised = false;
    //    int test = 0;
    //    Health objHealth = obj.GetComponent<Health>();

    //    if (objHealth != null)
    //    {
    //        categorised = true;

    //        if (!objHealth.IsDead())
    //        {
    //            test += 1; // Alive
    //        }

    //        if (IsEnemy(obj))
    //        {
    //            test += 2; // Enemy
    //        }

    //        if (IsBoss(obj))
    //        {
    //            test += 4; // Boss
    //        }

    //        //if (IsTrap(obj))
    //        //{
    //        //    test += 8; // Trap
    //        //}

    //        //if (IsObstacle(obj))
    //        //{
    //        //    test += 16; // Obstacle
    //        //}

    //    }

    //    if (categorised)
    //    {
    //        category = (ObjectCategories)test;
    //    }

    //    return category;
    //}

    protected virtual bool IdentifyObject(GameObject obj, out Description description)
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
            case ObjectCategories.DeadAllyBoss:
                result = EvaluateDeadAlly(obj);
                break;
            case ObjectCategories.HealthAlly:
            case ObjectCategories.HealthAllyBoss:
                result = EvaluateAlly(obj);
                break;
            case ObjectCategories.DeadEnemy:
            case ObjectCategories.DeadEnemyBoss:
                result = EvaluateDeadEnemy(obj);
                break;
            case ObjectCategories.HealthyEnemy:
            case ObjectCategories.HealthyEnemyBoss:
                result = EvaluateEnemy(obj);
                break;
            case ObjectCategories.CoverSafe:
            case ObjectCategories.CoverComprimised:
            case ObjectCategories.CoverUnsafe:
                result = EvalulateCover(obj);
                break;
            case ObjectCategories.Uncategorised:
            default:
                result = 0f;
                break;
        }

        return result; 
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
    public virtual bool IsCover(GameObject obj)
    {
        return false;
    }
    public virtual bool IsSafe(GameObject obj)
    {
        return false;
    }
    public virtual bool IsComprimised(GameObject obj)
    {
        return false;
    }


    // ************************************************
    // Evaluations
    // ****************************

    protected virtual float EvaluateEnemy(GameObject obj)
    {
        float result = 0f;

        return result;
    }

    protected virtual float EvaluateDeadEnemy(GameObject obj)
    {
        float result = 0f;

        return result;
    }

    protected virtual float EvaluateAlly(GameObject obj)
    {
        float result = 0f;

        return result;
    }

    protected virtual float EvaluateDeadAlly(GameObject obj)
    {
        float result = 0f;

        return result;
    }

    protected virtual float EvalulateCover(GameObject obj)
    {
        float result = 0f;

        return result;
    }

}
