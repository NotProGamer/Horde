using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Think 

[System.Serializable]
public class Assignment
{
    // this class should be defined elsewhere
    public float value = 0f;
}

public class HumanEvaluations : EvaluationModule {

    // *****************************************************************
    // *****************************************************************
    // *****************************************************************
    //  Self Evaluation
    // **********************

    public enum EvaluationNames
    {
        Health,
        Damage,
        SelfIsDead,
        Bored,
        Enganged,
        StandardEvaluationCount, // Should Always be the Last Enumerator Value
    }



    [System.Serializable]
    public class HealthEvaluations
    {
        public UtilityMath.UtilityValue m_linear; // Health
        public UtilityMath.UtilityValue m_inverseLinear; // Damage
        public HealthEvaluations(float min, float max)
        {
            m_linear = new UtilityMath.UtilityValue(UtilityMath.UtilityValue.NormalisationFormula.Linear, min, max);
            m_inverseLinear = new UtilityMath.UtilityValue(UtilityMath.UtilityValue.NormalisationFormula.InverseLinear, min, max);
        }
        public void SetValues(float value)
        {
            m_linear.SetValue(value);
            m_inverseLinear.SetValue(value);
        }
    }

    [System.Serializable]
    public class Boredom
    {
        public UtilityMath.UtilityValue m_linear; // Boredom
        public UtilityMath.UtilityValue m_inverseLinear; // Enganged

        public Boredom(float min, float max)
        {
            m_linear = new UtilityMath.UtilityValue(UtilityMath.UtilityValue.NormalisationFormula.Linear, min, max);
            m_inverseLinear = new UtilityMath.UtilityValue(UtilityMath.UtilityValue.NormalisationFormula.InverseLinear, min, max);
        }

        public void SetValues(float value)
        {
            m_linear.SetValue(value);
            m_inverseLinear.SetValue(value);
        }

    }

    [System.Serializable]
    public class StandardEvaluations
    {
        public UtilityMath.UtilityValue m_count; // Standard Evaluation Count (returns the number of standard evaluations)
        public HealthEvaluations m_healthEvaluations;
        public UtilityMath.UtilityValue m_death; // IsDead
        public Boredom m_boredom;
    }

    public StandardEvaluations m_standardEvaluations;

    [System.Serializable]
    public class ObjectEvaluations
    {
        [System.Serializable]
        public class Enemy
        {
            [System.Serializable]
            public class Weights
            {
                public float m_base = 50f;
                public float m_damage = 10.0f;
                public float m_distance = 10.0f;
                public float m_death = -125.0f; // early exit
            }
            public Weights m_weights;
            private float m_healthiestEnemy = 0f;
            public UtilityMath.UtilityValue m_health;
            public void SetMinMaxHealth(float min, float max)
            {
                if (max > m_healthiestEnemy)
                {
                    m_health.SetMinMaxValues(0, max); // this should possibly be the highest health for any enemy
                    m_healthiestEnemy = max;
                }
            }
        }
        public Enemy m_enemy;
        public UtilityMath.UtilityValue objectDistance; // using sqrMagnitude
    }

    public ObjectEvaluations m_objectEvaluations;


    public float m_thoughDelay = 0.5f;
    private float m_thoughtTicker = 0f;

    private Brain m_brain = null;
    private Health m_healthScript = null;

    void Awake()
    {
        m_brain = GetComponent<Brain>();
        if (m_brain == null)
        {
            Debug.Log("Brain not included!");
        }
        m_healthScript = GetComponent<Health>();
        if (m_brain == null)
        {
            Debug.Log("Health not included!");
        }
    }

    // Use this for initialization
    void Start ()
    {
        //CreateEvaluations();
        m_thoughtTicker = Random.Range(0f, m_thoughDelay);
        CreateSelfEvaluations();
        CreateEnvironmentalEvaluations();
    }

    void CreateSelfEvaluations()
    {
        if (m_healthScript != null)
        {
            m_standardEvaluations.m_healthEvaluations = new HealthEvaluations(0, m_healthScript.m_maxHealth);
            m_standardEvaluations.m_death = new UtilityMath.UtilityValue(UtilityMath.UtilityValue.NormalisationFormula.Linear, System.Convert.ToInt32(false), System.Convert.ToInt32(true)); // 0, 1

            AddEvaluation((int)EvaluationNames.Health, m_standardEvaluations.m_healthEvaluations.m_linear);
            AddEvaluation((int)EvaluationNames.Damage, m_standardEvaluations.m_healthEvaluations.m_inverseLinear);
            AddEvaluation((int)EvaluationNames.SelfIsDead, m_standardEvaluations.m_death);
        }

        if (m_brain != null)
        {
            m_standardEvaluations.m_boredom = new Boredom(0, m_brain.m_boredomMaximum);
            AddEvaluation((int)EvaluationNames.Bored, m_standardEvaluations.m_boredom.m_linear);
            AddEvaluation((int)EvaluationNames.Enganged, m_standardEvaluations.m_boredom.m_inverseLinear);
        }

    }
    void CreateEnvironmentalEvaluations()
    {
        if (m_brain != null)
        {
            float maxDistanceToObject = m_brain.m_sight.m_range * m_brain.m_sight.m_range;
            m_objectEvaluations.objectDistance = new UtilityMath.UtilityValue(UtilityMath.UtilityValue.NormalisationFormula.InverseLinear, 0, maxDistanceToObject);
        }
        m_objectEvaluations.m_enemy.m_health = new UtilityMath.UtilityValue(UtilityMath.UtilityValue.NormalisationFormula.Linear, 0, 1);
    }


    // Update is called once per frame
    void Update ()
    {
        //UpdateEvaluations();
        if (Time.time > m_thoughtTicker)
        {
            m_thoughtTicker = Time.time + m_thoughDelay;



            /// Add new input to memory
            // Get New Observations
            // Get New Noises
            // Get New Assignments

            /// Evaluate Items in Memory
            //  Remove Inactive Item From Memory
            //  Evaluate Item In Memory
            //  Update Memory Counts

            /// Select Priority Items
            // Closest Item
            // Highest Priority Item
            // Direction of Threat

            if (m_brain)
            {
                List<GameObject> nearbyObjects;
                if (m_brain.GetNearbyObjects(out nearbyObjects))
                {
                    for (int i = 0; i > nearbyObjects.Count; i++)
                    {
                        // if memory contains object
                        // // Categorize it 
                        // else
                        // // Categorize it 
                        // // Add it to memory
                        GameObject obj = nearbyObjects[i];

                        Description description;
                        if (m_visualMemory.TryGetValue(obj, out description))
                        {
                            description.m_category = Categorize(obj);
                        }
                        else
                        {
                            description = new Description();
                            description.m_category = Categorize(obj);
                            m_visualMemory.Add(obj, description);
                        }

                    }
                }
                List<Noise> audibleNoises;
                if (m_brain.GetAudibleNoises(out audibleNoises))
                {
                    for (int i = 0; i > audibleNoises.Count; i++)
                    {
                        // if memory contains object
                        // // Categorize it 
                        // else
                        // // Categorize it 
                        // // Add it to memory
                        Noise obj = audibleNoises[i];

                        Description description;
                        if (m_audioMemory.TryGetValue(obj, out description))
                        {
                            description.m_category = Categorize(obj);
                        }
                        else
                        {
                            description = new Description();
                            description.m_category = Categorize(obj);
                            m_audioMemory.Add(obj, description);
                        }

                    }
                }
                List<Assignment> nearbyAssignments;
                if (m_brain.GetNearbyAssignments(out nearbyAssignments))
                {
                    for (int i = 0; i > nearbyAssignments.Count; i++)
                    {
                        // if memory contains object
                        // // Categorize it 
                        // else
                        // // Categorize it 
                        // // Add it to memory
                        Assignment obj = nearbyAssignments[i];

                        Description description;
                        if (m_assignmentMemory.TryGetValue(obj, out description))
                        {
                            description.m_category = Categorize(obj);
                        }
                        else
                        {
                            description = new Description();
                            description.m_category = Categorize(obj);
                            m_assignmentMemory.Add(obj, description);
                        }

                    }
                }
            }




            UpdateEvaluations();
        }
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

    /// Evaluate priorities
    // Evaluate Death
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

    void UpdateEvaluations()
    {
        m_standardEvaluations.m_healthEvaluations.SetValues(m_healthScript.m_health);
        m_standardEvaluations.m_death.SetValue(System.Convert.ToInt32(m_healthScript.IsDead()));
        m_standardEvaluations.m_boredom.SetValues(m_brain.m_boredom);
    }
    

    public float RunEvaluation(EvaluationNames evaluation)
    {
        return RunEvaluation((int)evaluation);
    }



    // *****************************************************************
    // *****************************************************************
    // *****************************************************************
    //  Target Identification and Evaluation
    // **************************************

    private Dictionary<GameObject, Description> m_visualMemory;
    private Dictionary<Noise, Description> m_audioMemory;
    private Dictionary<Assignment, Description> m_assignmentMemory;

    public enum Categories
    {
        EnemyObject,
        AllyObject,
        CorpseObject,
        Noise,
        Assignment,
        //CoverObject,
        //AmmoObject,
        //HealthPackObject,
        Uncategorised,
    }


    [System.Serializable]
    public class Description
    {
        //public ObjectCategories m_objectCategory = ObjectCategories.Uncategorised;
        public Categories m_category = Categories.Uncategorised;
        public float m_evaluation = 0f;
    }

    protected virtual Categories Categorize(GameObject obj)
    {
        Categories category = Categories.Uncategorised;

        Health objHealth = obj.GetComponent<Health>();
        
        // if it has health 
        if (objHealth)
        {
            // if it is dead    
            if (objHealth.IsDead())
            {
                category = Categories.CorpseObject;
            }
            else
            {
                
                if (IsEnemy(obj))
                {
                    category = Categories.EnemyObject;
                }
                else if (IsAlly(obj))
                {
                    category = Categories.AllyObject;
                }
                else
                {
                    // might be something else with health
                }
            }
        }


        return category;
    }

    protected virtual Categories Categorize(Noise obj)
    {
        Categories category = Categories.Uncategorised;

        if (obj != null)
        {
            category = Categories.Noise;
        }

        return category;
    }

    protected virtual Categories Categorize(Assignment obj)
    {
        Categories category = Categories.Uncategorised;

        if (obj != null)
        {
            category = Categories.Assignment;
        }

        return category;
    }




    /// Decisions
    //public class Decision
    //{
    //    public object m_target = null;
    //    public string m_identifier = ""; // BehaviorName
    //    public Decision(string identifier, object target = null)
    //    {
    //        m_target = target;
    //        m_identifier = identifier;
    //    }
    //    public void SetDecision(string identifier, object target = null)
    //    {
    //        m_target = target;
    //        m_identifier = identifier;
    //    }
    //}

    //public Decision m_currentDecision;

    /// Identify Objects
    //void IdentifyObjects()
    //{
    //    foreach (GameObject obj in m_brain.m_nearbyObjects)
    //    {
    //        // if memory contains object
    //        // // Evaluate it
    //        // else
    //        // // Evaluate it
    //        // // Add it to memory

    //        if (m_visualMemory.ContainsKey(obj))
    //        {
    //            Description test;
    //            if (m_visualMemory.TryGetValue(obj, out test))
    //            {
    //                IdentifyObject(obj, out test);
    //            }
    //        }
    //        else
    //        {
    //            Description test = new Description();

    //            if (IdentifyObject(obj, out test))
    //            {
    //                m_visualMemory.Add(obj, test);
    //            }

    //        }

    //        //ObjectCategories category = CategoriesObject(obj);
    //        //float evaluation = 0f;
    //    }
    //}
    //protected virtual bool IdentifyObject(GameObject obj, out Description description)
    //{
    //    bool result = false;
    //    if (m_visualMemory.TryGetValue(obj, out description))
    //    {
    //        description.m_objectCategory = CategoriesObject(obj);
    //        if (description.m_objectCategory != ObjectCategories.Uncategorised)
    //        {
    //            description.m_evaluation = EvaluateObject(obj, description.m_objectCategory);
    //            return true;
    //        }
    //    }
    //    return result;
    //}

    /// Object Categories Old
    //public enum ObjectCategories
    //{
    //    DeadAlly = 0,
    //    HealthAlly = 1,
    //    DeadEnemy = 2,
    //    HealthyEnemy = 3,
    //    DeadAllyBoss = 4,
    //    HealthAllyBoss = 5,
    //    DeadEnemyBoss = 6,
    //    HealthyEnemyBoss = 7,
    //    //DeadHazzard = 8,
    //    //HealthyHazzard = 9,
    //    //DeadObstacle = 16,
    //    //HealthyObstacle = 17,

    //    CoverSafe = 32,
    //    CoverComprimised = 33,
    //    CoverUnsafe = 34,

    //    Uncategorised = 128,

    //}
    //[System.Flags]
    //public enum ObjectCategories
    //{
    //    Uncategorised = 0,
    //    Unrecognised = 0x01,
    //    Alive = 0x02, // Dead
    //    Enemy = 0x04, // Ally
    //    Boss = 0x08, // Standard
    //}

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
    //    else if (IsCover(obj))
    //    {
    //        categorised = true;

    //        if (IsSafe(obj))
    //        {
    //            // Safe
    //            test = (int)ObjectCategories.CoverSafe;
    //        }
    //        else if (IsComprimised(obj))
    //        {
    //            // Comprimised
    //            test = (int)ObjectCategories.CoverComprimised;
    //        }
    //        else
    //        {
    //            // Unsafe
    //            test = (int)ObjectCategories.CoverUnsafe;
    //        }
    //    }


    //    if (categorised)
    //    {
    //        category = (ObjectCategories)test;
    //    }

    //    return category;
    //}

    /// Evaluate Object
    //protected virtual float EvaluateObject(GameObject obj, ObjectCategories category)
    //{
    //    float result = 0f;

    //    switch (category)
    //    {
    //        case ObjectCategories.DeadAlly:
    //        case ObjectCategories.DeadAllyBoss:
    //            result = EvaluateDeadAlly(obj);
    //            break;
    //        case ObjectCategories.HealthAlly:
    //        case ObjectCategories.HealthAllyBoss:
    //            result = EvaluateAlly(obj);
    //            break;
    //        case ObjectCategories.DeadEnemy:
    //        case ObjectCategories.DeadEnemyBoss:
    //            result = EvaluateDeadEnemy(obj);
    //            break;
    //        case ObjectCategories.HealthyEnemy:
    //        case ObjectCategories.HealthyEnemyBoss:
    //            result = EvaluateEnemy(obj);
    //            break;
    //        case ObjectCategories.CoverSafe:
    //        case ObjectCategories.CoverComprimised:
    //        case ObjectCategories.CoverUnsafe:
    //            result = EvalulateCover(obj);
    //            break;
    //        case ObjectCategories.Uncategorised:
    //        default:
    //            result = 0f;
    //            break;
    //    }

    //    return result;
    //}

    void EvaluateNearbyObjects()
    {
        //for (int i = 0; i < m_nearbyObjects.Count; i++)
        //{

        //}
    }


    // ************************************************
    // Qualifiers
    // ****************************

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
    public virtual bool IsAlly(GameObject obj)
    {
        if (Labels.Tags.IsHuman(gameObject))
        {
            return Labels.Tags.IsHuman(obj);
        }
        if (Labels.Tags.IsZombie(gameObject))
        {
            return Labels.Tags.IsZombie(obj);
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
    //public virtual bool IsAmmo(GameObject obj)
    //{
    //    return false;
    //}
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

    protected virtual float EvaluateObject(GameObject obj, Categories category)
    {
        float value = 0f;

        switch (category)
        {
            case Categories.EnemyObject:
                value = EvaluateEnemy(obj);
                break;
            case Categories.AllyObject:
                break;
            case Categories.CorpseObject:
                break;
            case Categories.Uncategorised:
                Debug.Log("Uncategorised objects can not be evaluated");
                break;
            default:
                break;
        }

        return value;
    }

    protected virtual float EvaluateNoise(Noise noise)
    {
        float result = 0f;

        return result;
    }
    protected virtual float EvaluateAssignment(Assignment assignment)
    {
        float result = 0f;

        return result;
    }
    //protected virtual float EvaluateDeath(bool dead)
    //{
    //    float result = 0f;
    //    return result;
    //}
    //protected virtual float EvaluateWander(float Boredom)
    //{
    //    float result = 0f;
    //    return result;
    //}
    //protected virtual float EvaluateIdle(float Boredom)
    //{
    //    float result = 0f;
    //    return result;
    //}
    //void EvaluateVitals()
    //{
    //}

    // *************************

    protected virtual float EvaluateEnemy(GameObject obj)
    {
        float value = 0f;
        if (obj == null)
        {
            return value; // early exit
        }

        // if enemy dead return death weight (-125)
        // if enemy alive add base enemy value (50)
        // add enemy damage (0-10) 
        // add enemy distance covered (0-10)

        Health enemyHealthScript = obj.GetComponent<Health>();
        if (enemyHealthScript)
        {
            if (enemyHealthScript.IsDead())
            {
                return m_objectEvaluations.m_enemy.m_weights.m_death; // early exit
            }
            else
            {
                value += m_objectEvaluations.m_enemy.m_weights.m_base; // add value if living enemy nearby
            }
            // Damage Evaluation = Damage Percent * Damage Weight
            // the more damaged they are the higher the priority they become
            m_objectEvaluations.m_enemy.SetMinMaxHealth(0, enemyHealthScript.m_maxHealth);
            m_objectEvaluations.m_enemy.m_health.SetExponentialProperties(2.0f);
            m_objectEvaluations.m_enemy.m_health.SetNormalisationType(UtilityMath.UtilityValue.NormalisationFormula.InverseExponential);
            m_objectEvaluations.m_enemy.m_health.SetValue(enemyHealthScript.m_health);
            value += m_objectEvaluations.m_enemy.m_health.Evaluate() * m_objectEvaluations.m_enemy.m_weights.m_damage;
        }
        if (m_brain)
        {
            // Distance to Enemy Evaluation = Distance Covered Percentage * Distance Weight
            // the Closer the get the higher the priority they become
            m_objectEvaluations.objectDistance.SetExponentialProperties(2.0f);
            m_objectEvaluations.objectDistance.SetNormalisationType(UtilityMath.UtilityValue.NormalisationFormula.Exponential);
            float distanceToObject = (obj.transform.position - transform.position).sqrMagnitude;
            m_objectEvaluations.objectDistance.SetValue(distanceToObject);
            value += m_objectEvaluations.objectDistance.Evaluate() * m_objectEvaluations.m_enemy.m_weights.m_distance; 
        }

        return value;
    }

    protected virtual float EvaluateAlly(GameObject obj)
    {
        float result = 0f;

        return result;
    }

    protected virtual float EvaluateCorpse(GameObject obj)
    {
        float result = 0f;

        return result;
    }

    protected virtual float EvalulateCover(GameObject obj)
    {
        float result = 0f;

        return result;
    }

    // *************************

    // ************************************************
    // Getters
    // ****************************

    public GameObject GetClosestEnemy()
    {
        GameObject test = null;
        // Not yet implemented

        // get closest enemy from visual memory

        return test;
    }

    public GameObject GetHighestPriorityEnemy()
    {
        return null;
    }

    public Noise GetHighestPriorityNoise()
    {
        return null;
    }

    public Assignment GetHighestPriorityAssignment()
    {
        return null;
    }




    //// Custom Evaluations
    //[System.Serializable]
    //public class CustomEvaluation
    //{
    //    public string m_identifier = "";
    //    public List<EvaluationNames> m_evaluations;
    //    // Consider including Custom Evaluations as children aswell
    //    // public List<string> m_customEvaluations;
    //    // float weight = 0f;
    //}
    //public List<CustomEvaluation> m_customEvaluations;
    //public float RunCustomEvaluation(string identifier)
    //{
    //    float evaluation = 0f;
    //    int customEvalualtionIndex = m_customEvaluations.FindIndex(item => item.m_identifier == identifier);
    //    if (customEvalualtionIndex >= 0)
    //    {
    //        CustomEvaluation customEvaluation = m_customEvaluations[customEvalualtionIndex];
    //        for (int i = 0; i < customEvaluation.m_evaluations.Count; i++)
    //        {
    //            if (i == 0)
    //            {
    //                evaluation = RunEvaluation(customEvaluation.m_evaluations[i]);
    //            }
    //            else
    //            {
    //                evaluation += RunEvaluation(customEvaluation.m_evaluations[i]);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("Evaluation not found!");
    //    }
    //    return evaluation;
    //}



}
