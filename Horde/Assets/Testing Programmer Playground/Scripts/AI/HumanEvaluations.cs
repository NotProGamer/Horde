using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
        EnemiesNearby,
        NoEnemiesNearby,
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

    [System.Serializable]
    public class EnvironmentalEvaluations
    {
        [System.Serializable]
        public class EnemiesNearby
        {
            public UtilityMath.UtilityValue m_linear;
            public UtilityMath.UtilityValue m_inverseLinear;
            public void SetValue(float value)
            {
                m_linear.SetValue(value);
                m_inverseLinear.SetValue(value);
            }
            public EnemiesNearby(float min, float max)
            {
                m_linear = new UtilityMath.UtilityValue(UtilityMath.UtilityValue.NormalisationFormula.Linear, 0, 1);
                m_inverseLinear = new UtilityMath.UtilityValue(UtilityMath.UtilityValue.NormalisationFormula.InverseLinear, 0, 1);
            }

        }
        public EnemiesNearby m_enemiesNearby;
        //public UtilityMath.UtilityValue m_threat;
        //public UtilityMath.UtilityValue m_courage;

        public UtilityMath.UtilityValue m_enemyInRange;
    }
    public EnvironmentalEvaluations m_environmentalEvaluations;

    [System.Serializable]
    public class Counter
    {
        public Counter()
        {
            m_counters = new Dictionary<Categories, int>();
            CreateCounter();
        }
        public Dictionary<Categories, int> m_counters;
        public void CreateCounter()
        {
            foreach (Categories category in System.Enum.GetValues(typeof(Categories)))
            {
                m_counters.Add(category, 0);
            }
        }
        public int GetCategoryCount(Categories category)
        {
            int count;
            // if category counter not found return 0
            if (!m_counters.TryGetValue(category, out count))
            {
                count = 0;
            }
            return count;
        }
        public void Reset()
        {
            foreach (Categories category in System.Enum.GetValues(typeof(Categories)))
            {
                ClearCount(category);
            }
        }
        public void IncreaseCount(Categories category)
        {
            m_counters[category] = m_counters[category] + 1;
        }
        public void ClearCount(Categories category)
        {
            m_counters[category] = 0;
        }
    }
    private Counter m_memoryCounter;
    private Counter m_senseCounter;


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
        InitializeMemory();
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
        m_environmentalEvaluations.m_enemiesNearby= new EnvironmentalEvaluations.EnemiesNearby(0, 1);
        AddEvaluation((int)EvaluationNames.EnemiesNearby, m_environmentalEvaluations.m_enemiesNearby.m_linear);
        AddEvaluation((int)EvaluationNames.NoEnemiesNearby, m_environmentalEvaluations.m_enemiesNearby.m_inverseLinear);
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


            MemorizeStimuli();

            EvaluateObjectsInMemory(); // This code needs to be rewritten as it in not currently updating the dictionary with the changes.
            OutputCounter();


            UpdateEvaluations();
            m_environmentalEvaluations.m_enemiesNearby.SetValue((float)m_senseCounter.GetCategoryCount(Categories.EnemyObject));
        }
    }

    private void OutputCounter()
    {
        foreach (var i in m_memoryCounter.m_counters)
        {
            if (i.Value > 0)
            {
                //Debug.Log("Memory: " + i.Key.ToString() + " " + i.Value);
            }
        }

        foreach (var i in m_senseCounter.m_counters)
        {
            if (i.Value > 0)
            {
                //Debug.Log("Sense: " + i.Key.ToString() + " " + i.Value);
            }
        }
    }

    private void MemorizeStimuli()
    {
        m_senseCounter.Reset();
        if (m_brain)
        {
            List<GameObject> nearbyObjects;


            if (m_brain.GetNearbyObjects(out nearbyObjects))
            {
                if (nearbyObjects.Count > 0)
                {
                    //Debug.Log("nearby object found");
                }

                for (int i = 0; i < nearbyObjects.Count; i++)
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
                        //Debug.Log("recategorize");
                    }
                    else
                    {
                        description = new Description();
                        description.m_category = Categorize(obj);
                        m_visualMemory.Add(obj, description);
                        
                        //Debug.Log("add");
                    }
                    m_senseCounter.IncreaseCount(description.m_category);
                }

            }
            List<Noise> audibleNoises;
            if (m_brain.GetAudibleNoises(out audibleNoises))
            {
                for (int i = 0; i < audibleNoises.Count; i++)
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
                    m_senseCounter.IncreaseCount(description.m_category);
                }
            }
            List<Assignment> nearbyAssignments;
            if (m_brain.GetNearbyAssignments(out nearbyAssignments))
            {
                for (int i = 0; i < nearbyAssignments.Count; i++)
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
                    m_senseCounter.IncreaseCount(description.m_category);
                }
            }
        }
    }


    private void EvaluateObjectsInMemory()
    {
        m_memoryCounter.Reset();
        if (m_visualMemory.Count > 0)
        {

            List<GameObject> visualkeys = new List<GameObject>(m_visualMemory.Keys);
            foreach (GameObject key in visualkeys)
            {
                // Might Also want to consider forgeting items when:
                //  out of range
                //  not seen for a certain amount of time

                if (key.gameObject.activeSelf == false)
                {
                    // Remove Inactive Items from memory
                    m_visualMemory.Remove(key);
                }
                else
                {
                    Description description = m_visualMemory[key];
                    description.m_evaluation = EvaluateObject(key, description.m_category);
                    if (description.m_evaluation < 0)
                    {
                        // this check is made incase the object has changed category
                        description.m_category = Categorize(key);
                        description.m_evaluation = EvaluateObject(key, description.m_category);
                    }
                    m_visualMemory[key] = description;
                    m_memoryCounter.IncreaseCount(description.m_category);
                }
            }
        }

        if (m_audioMemory.Count > 0)
        {
            List<Noise> audiokeys = new List<Noise>(m_audioMemory.Keys);
            foreach (Noise key in audiokeys)
            {
                Description description = m_audioMemory[key];
                description.m_evaluation = EvaluateNoise(key);
                //if (description.m_evaluation < 0)
                //{
                //    // this check is made incase the object has changed category
                //    description.m_category = Categorize(key);
                //    description.m_evaluation = EvaluateNoise(key);
                //}
                m_audioMemory[key] = description;
                m_memoryCounter.IncreaseCount(description.m_category);
            }
        }


        if (m_assignmentMemory.Count > 0)
        {
            List<Assignment> assignmentkeys = new List<Assignment>(m_assignmentMemory.Keys);
            foreach (Assignment key in assignmentkeys)
            {
                Description description = m_assignmentMemory[key];
                description.m_evaluation = EvaluateAssignment(key);
                //if (description.m_evaluation < 0)
                //{
                //    // this check is made incase the object has changed category
                //    description.m_category = Categorize(key);
                //    description.m_evaluation = EvaluateAssignment(key);
                //}
                m_assignmentMemory[key] = description;
                m_memoryCounter.IncreaseCount(description.m_category);
            }
        }


        // This code needs to be rewritten as it in not currently updating the dictionary with the changes.

        //foreach (KeyValuePair<GameObject, Description> entry in m_visualMemory)
        //{
        //    GameObject obj = entry.Key;
        //    Description description = entry.Value;
        //    if (obj != null && description != null)
        //    {
        //        description.m_evaluation = EvaluateObject(obj, description.m_category);
        //        if (description.m_evaluation < 0)
        //        {
        //            // this check is made incase the object has changed category
        //            description.m_category = Categorize(obj);
        //            description.m_evaluation = EvaluateObject(obj, description.m_category);
        //        }
        //    }
        //    m_counter.IncreaseCount(description.m_category);
        //}

        //foreach (KeyValuePair<Noise, Description> entry in m_audioMemory)
        //{
        //    Noise obj = entry.Key;
        //    Description description = entry.Value;
        //    if (obj != null && description != null)
        //    {
        //        description.m_evaluation = EvaluateNoise(obj);
        //        //if (description.m_evaluation < 0)
        //        //{
        //        //    // this check is made incase the object has changed category
        //        //    description.m_category = Categorize(obj);
        //        //    description.m_evaluation = EvaluateNoise(obj);
        //        //}
        //    }
        //    m_counter.IncreaseCount(description.m_category);
        //}

        //foreach (KeyValuePair<Assignment, Description> entry in m_assignmentMemory)
        //{
        //    Assignment obj = entry.Key;
        //    Description description = entry.Value;
        //    if (obj != null && description != null)
        //    {
        //        description.m_evaluation = EvaluateAssignment(obj);
        //        //if (description.m_evaluation < 0)
        //        //{
        //        //    // this check is made incase the object has changed category
        //        //    description.m_category = Categorize(obj);
        //        //    description.m_evaluation = EvaluateAssignment(obj);
        //        //}
        //    }
        //    m_counter.IncreaseCount(description.m_category);
        //}
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
        SelfObject,
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

    void InitializeMemory()
    {
        m_visualMemory = new Dictionary<GameObject, Description>();
        m_audioMemory = new Dictionary<Noise, Description>();
        m_assignmentMemory = new Dictionary<Assignment, Description>();
        m_memoryCounter = new Counter();
        m_senseCounter = new Counter();
    }

    [System.Serializable]
    public class Description/*: System.IComparable*/
    {
        //public ObjectCategories m_objectCategory = ObjectCategories.Uncategorised;
        public Categories m_category = Categories.Uncategorised;
        public float m_evaluation = 0f;

        //public int CompareTo(object obj)
        //{
        //    if (obj == null) return 1;
        //    Description otherDescription = obj as Description;
        //    if (otherDescription != null)
        //    {
        //        return this.m_evaluation.CompareTo(otherDescription.m_evaluation);
        //    }
        //    else
        //    {
        //        throw new System.ArgumentException("Object is not a Description");
        //    }
            
        //}
    }

    protected virtual Categories Categorize(GameObject obj)
    {
        Categories category = Categories.Uncategorised;

        if (obj.gameObject == this.gameObject)
        {
            return Categories.SelfObject; // Early exit as obj is Self
        }

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

    public GameObject GetHighestPriorityEnemy()
    {
        // return the enemy with the highest evaluation
        GameObject priorityEnemy = null;
        // generate a filtered list of enemies
        Dictionary<GameObject, Description> enemies = m_visualMemory.Where(x => x.Value.m_category == Categories.EnemyObject).ToDictionary(x => x.Key, x => x.Value);

        // if enemies in memory
        if (enemies.Count() > 0)
        {
            // find enemy of highest priority
            priorityEnemy = enemies.Aggregate((a,b) => a.Value.m_evaluation > b.Value.m_evaluation ? a: b).Key;
        }
        return priorityEnemy;
    }

    public GameObject GetClosestEnemy()
    {
        GameObject test = null;
        // Not yet implemented

        // get closest enemy from visual memory

        return test;
    }


    public Noise GetHighestPriorityNoise()
    {
        return null;
    }

    public Assignment GetHighestPriorityAssignment()
    {
        return null;
    }




    /// Custom Evaluations
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
