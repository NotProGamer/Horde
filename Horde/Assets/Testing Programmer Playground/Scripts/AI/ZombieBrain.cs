using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieBrain : MonoBehaviour {


    //public enum ZombieType
    //{
    //    Zombie,
    //    Screamer,
    //    LittleGirl,
    //    Dictator,
    //    Glutton,
    //}
    //public ZombieType m_zombieType = ZombieType.Zombie;


    private Dictionary<string, object> m_memory = new Dictionary<string, object>();
    // CurrentTarget [Done]
    // EnemiesInSight
    // ClosestEnemy 
    // NoisesInHearing
    // LoudestNoise



    // Look 
    public float m_sightRange = 10f;
    private const int m_maxSightResults = 100;
    private Collider[] m_sightResults = new Collider[m_maxSightResults];

    public LayerMask m_sightMask;

    // Listen
    public float m_hearingRange = 5f; // not yet implemented

    public float m_boredomIncrement = 10f;
    public float m_maxBoredom = 100f;
    public float m_currentBoredom = 0f;

    void Awake()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(Labels.Tags.GameController);
        if (obj)
        {
            m_noiseManager = obj.GetComponent<NoiseManager>();
            if (m_noiseManager == null)
            {
                Debug.Log("NoiseManager not included!");
            }
        }
        else
        {
            Debug.Log("GameController not included!");
        }

        m_zombieUtilityAIScript = GetComponent<ZombieUtilityAI>();
        if (m_zombieUtilityAIScript == null)
        {
            Debug.Log("ZombieUtilityAI not included.");
        }

        m_movementScript = GetComponent<Movement>();
        if (m_movementScript == null)
        {
            Debug.Log("Movement not included.");
        }

    }


    // Use this for initialization
    void Start ()
    {
        m_memory.Add(Labels.Memory.CurrentTarget, transform.position);
        m_memory.Add(Labels.Memory.LastUserTap, null);
        m_memory.Add(Labels.Memory.LastPriorityNoise, null);
        m_memory.Add(Labels.Memory.ClosestEnemy, null);
        m_memory.Add(Labels.Memory.ClosestCorpse, null);
        m_memory.Add(Labels.Memory.ClosestDestructible, null);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //object currentTarget = null;
        //if (m_memory.TryGetValue("CurrentTartget", out currentTarget))
        //{
        //    System.Type t = currentTarget.GetType();
        //    if (t == typeof(Vector3))
        //    {
        //        Debug.Log("Position " + ((Vector3)currentTarget).ToString());
        //        m_memory["CurrentTartget"] = transform.gameObject;
        //    }
        //    if (t == typeof(GameObject))
        //    {
        //        Debug.Log("GameObject Position " + ((GameObject)currentTarget).transform.position.ToString());
        //    }
        //}
        //else
        //{
        //    Debug.Log("Unable to find 'CurrentTarget'");
        //}

        //Sense
        Look();
        Listen();
        if (gameObject.CompareTag(Labels.Tags.ZombieScreamer))
        {
            FilterOutScreamerNoise();
        }
        // Think
        SortNoises();


        // modify memory
        if (m_userTaps.Count > 0)
        {
            m_memory[Labels.Memory.LastUserTap] = m_userTaps[0];
        }
        else
        {
            //m_memory[Labels.Memory.LastUserTap] = null; /// testing 
        }

        if (m_audibleNoises.Count > 0)
        {
            m_memory[Labels.Memory.LastPriorityNoise] = m_audibleNoises[0];
        }

        if (m_enemies.Count > 1)
        {
            m_memory[Labels.Memory.ClosestEnemy] = GetClosest(m_enemies);
        }
        else if (m_enemies.Count > 0)
        {
            m_memory[Labels.Memory.ClosestEnemy] = m_enemies[0].gameObject;
        }
        else
        {
            m_memory[Labels.Memory.ClosestEnemy] = null;
        }


        if (m_enemyCorpses.Count > 1)
        {
            m_memory[Labels.Memory.ClosestCorpse] = GetClosest(m_enemyCorpses);
        }
        else if (m_enemyCorpses.Count > 0)
        {
            m_memory[Labels.Memory.ClosestCorpse] = m_enemyCorpses[0].gameObject;
        }
        else
        {
            m_memory[Labels.Memory.ClosestCorpse] = null;
        }

        if (m_destructibles.Count > 1)
        {
            m_memory[Labels.Memory.ClosestDestructible] = GetClosest(m_destructibles);
        }
        else if (m_destructibles.Count > 0)
        {
            m_memory[Labels.Memory.ClosestDestructible] = m_destructibles[0].gameObject;
        }
        else
        {
            m_memory[Labels.Memory.ClosestDestructible] = null;
        }


        UpdateBoredom();

        if (gameObject.CompareTag(Labels.Tags.ZombieLittleGirl))
        {
            LittleGirlZombieUpdate();
        }

        if (gameObject.CompareTag(Labels.Tags.ZombieDictator))
        {
            if (m_giveSpeedBoost)
            {
                GiveAlliesASpeedBoost();
                
            }
        }

        if (gameObject.CompareTag(Labels.Tags.ZombieGlutton))
        {
            GluttonUpdate();
            
        }

        m_speedBoostEffect.Update(); // disable speed boost effect when it times out
        UpdateSpeed();
    }

    public bool m_vomitToxin = false;
    public int m_vomitLevel = 1;
    public GameObject vomitPrefab;
    public void GluttonUpdate()
    {
        if (vomitPrefab == null)
        {
            return;
        }
        if (m_vomitToxin)
        {
            if (m_vomitLevel > 0)
            {
                m_vomitLevel--;
                GameObject test = Instantiate(vomitPrefab) as GameObject;
                test.transform.position = transform.position;
            }
            m_vomitToxin = false;
        }
    }

    public bool m_giveSpeedBoost = false;
    public void GiveAlliesASpeedBoost()
    {
        foreach (GameObject item in m_allies)
        {
            ZombieBrain allyBrain = item.GetComponent<ZombieBrain>();
            if (allyBrain != null)
            {
                allyBrain.m_speedBoostEffect.Enable();
            }
        }
    }


    public Transform m_victimIndicator;
    public enum DemeanorTypes
    {
        Passive,
        Aggressive,
        Neutral,
    }
    public DemeanorTypes m_demeanor = DemeanorTypes.Passive;
    public bool m_becomeAggressive = false;
    //public bool m_aggressive = false; // false is passive
    public void LittleGirlZombieUpdate()
    {
        // temporary switch for testing
        if (m_becomeAggressive)
        {
            m_demeanor = DemeanorTypes.Aggressive;
        }
        else
        {
            m_demeanor = DemeanorTypes.Passive;
        }

        if (m_victimIndicator != null)
        {
            // get closest enemy
            if (GetEnemiesInSightCount() > 0)
            {
                object item;
                if (m_memory.TryGetValue(Labels.Memory.ClosestEnemy, out item))
                {
                    GameObject closestEnemy = item as GameObject;
                    if (closestEnemy)
                    {
                        // move victim Indicator on to target
                        m_victimIndicator.SetParent(closestEnemy.transform);
                        m_victimIndicator.position = closestEnemy.transform.position;
                    }
                }
            }
            else
            {
                m_victimIndicator.SetParent(gameObject.transform);
                m_victimIndicator.position = gameObject.transform.position;

                // should really only trigger this when little girl is out of sight of humans but this will do for now
                m_demeanor = DemeanorTypes.Passive;
                m_becomeAggressive = false;
            }
        }

    }

    public bool IsAggressive()
    {
        bool result = true;
        switch (m_demeanor)
        {
            case DemeanorTypes.Passive:
            case DemeanorTypes.Neutral:
                result = false;
                break;
            case DemeanorTypes.Aggressive:
                break;
            default:
                break;
        }
        return result;
    }


    public bool GetCurrentTargetPosition(out Vector3 pTargetPosition)
    {
        bool result = false;
        Vector3 position = Vector3.zero;
        object currentTarget = null;
        if (m_memory.TryGetValue(Labels.Memory.CurrentTarget, out currentTarget))
        {
            if (currentTarget != null)
            {
                System.Type t = currentTarget.GetType();
                if (t == typeof(Vector3))
                {
                    position = (Vector3)currentTarget;
                    //m_memory["CurrentTartget"] = transform.gameObject;
                    result = true;
                }
                else if (t == typeof(GameObject))
                {
                    position = ((GameObject)currentTarget).transform.position;
                    result = true;
                }
                else if (t == typeof(Noise))
                {
                    position = ((Noise)currentTarget).m_position;
                    result = true;
                }
            }
            else
            {
                //Debug.Log("memory location '" + Labels.Memory.CurrentTarget + "' set to null");
            }
        }
        else
        {
            Debug.Log("Unable to find '" + Labels.Memory.CurrentTarget + "'");
        }
        pTargetPosition = position;
        return result;
    }

    public bool GetObjectPosition(string memoryLabel, out Vector3 pPosition)
    {
        bool result = false;
        Vector3 position = Vector3.zero;
        object memoryObject = null;
        if (m_memory.TryGetValue(memoryLabel, out memoryObject))
        {
            if (memoryObject != null)
            {
                System.Type t = memoryObject.GetType();
                if (t == typeof(Vector3))
                {
                    position = (Vector3)memoryObject;
                    //m_memory["CurrentTartget"] = transform.gameObject;
                    result = true;
                }
                else if (t == typeof(GameObject))
                {
                    position = ((GameObject)memoryObject).transform.position;
                    result = true;
                }
                else if (t == typeof(Noise))
                {
                    position = ((Noise)memoryObject).m_position;
                    result = true;
                }
                else
                {
                    Debug.Log("Unable to find position for memory '" + memoryLabel + "'");
                }
            }
            else
            {
                //Debug.Log("memoryObject is null");
            }
        }
        else
        {
            Debug.Log("Unable to find memory related to'" + memoryLabel + "'");
        }
        pPosition = position;
        return result;
    }


    private List<GameObject> m_enemies = new List<GameObject>();
    private List<GameObject> m_enemyCorpses = new List<GameObject>();
    private List<GameObject> m_allies = new List<GameObject>();
    private List<GameObject> m_destructibles = new List<GameObject>();


    // this will look at the gameobjects in sight and sorts them accordingly

    void Look()
    {
        int count = 0;
        count = Physics.OverlapSphereNonAlloc(transform.position, m_sightRange, m_sightResults, m_sightMask);

        // get all nearby objects

        if (count > 0)
        {
            //Debug.Log(count);
            m_enemies.Clear();
            m_enemyCorpses.Clear();
            m_allies.Clear();
            for (int i = 0; i < count; i++)
            {
                GameObject currentObject = null;

                currentObject = m_sightResults[i].gameObject;

                if (Labels.Tags.IsHuman(currentObject))
                {
                    Health currentObjectHealth = null;
                    currentObjectHealth = currentObject.GetComponent<Health>();

                    if (currentObjectHealth != null && !currentObjectHealth.IsDevoured())
                    {
                        if (currentObjectHealth.IsDead())
                        {
                            m_enemyCorpses.Add(currentObject);
                        }
                        else if (!currentObjectHealth.IsDead())
                        {
                            m_enemies.Add(currentObject);
                        }

                    }
                }
                else if (Labels.Tags.IsZombie(currentObject))
                {
                    Health currentObjectHealth = null;
                    currentObjectHealth = currentObject.GetComponent<Health>();
                    if (currentObjectHealth != null && !currentObjectHealth.IsDead())
                    {
                        m_allies.Add(currentObject);
                    }
                    
                    
                }
                else if (Labels.Tags.IsDestructible(currentObject))
                {
                    Health currentObjectHealth = null;
                    currentObjectHealth = currentObject.GetComponent<Health>();
                    if (currentObjectHealth != null && !currentObjectHealth.IsDead())
                    {
                        m_destructibles.Add(currentObject);
                    }


                }

            }
        }
    }
    public bool EnemyInSight()
    {
        return m_enemies.Count > 0;
    }

    private GameObject GetClosest(List<GameObject> pGameObjects)
    {
        GameObject closestGameObject = null;
        float distanceToClosestGameObject = float.MaxValue;

        for (int i = 0; i < pGameObjects.Count; i++)
        {
            float distanceToTarget = (pGameObjects[i].transform.position - transform.position).sqrMagnitude;
            if (distanceToTarget < distanceToClosestGameObject)
            {
                closestGameObject = pGameObjects[i];
                distanceToClosestGameObject = distanceToTarget;
            }
        }
        return closestGameObject;
    }

    //public void SetCurrentTarget(object pCurrentTarget)
    //{
    //    m_memory[Labels.Memory.CurrentTarget] = pCurrentTarget;
    //}

    public bool SetCurrentTarget(string pMemoryLabel)
    {
        object newTarget = null;
        if (m_memory.TryGetValue(pMemoryLabel, out newTarget))
        {
            m_memory[Labels.Memory.CurrentTarget] = newTarget;
            return true;
        }
        else
        {
            m_memory[Labels.Memory.CurrentTarget] = transform.position;
            Debug.Log("Unable to locate '" + pMemoryLabel + "' in memory");
            return false;
        }

    }
    public bool ClearMemoryLocation(string pMemoryLabel)
    {
        object memory = null;
        if (m_memory.TryGetValue(pMemoryLabel, out memory))
        {
            m_memory[pMemoryLabel] = null;
            return true;
        }
        else
        {
            //m_memory[Labels.Memory.CurrentTarget] = transform.position;
            Debug.Log("Unable to locate '" + pMemoryLabel + "' in memory");
            return false;
        }

    }
    public bool RemembersMemoryLocation(string pMemoryLabel)
    {
        object memory = null;
        if (!m_memory.TryGetValue(pMemoryLabel, out memory))
        {
            memory = null;
        }
        return memory != null;
    }

    private NoiseManager m_noiseManager = null;
    private List<Noise> m_audibleNoises = new List<Noise>();
    private List<Noise> m_userTaps = new List<Noise>();
    private List<Noise> m_investigated = new List<Noise>();
    //private float m_lastListenTime = 0f;

    public float m_maxTapInterest = 2.0f;

    public float GetTapInterest()
    {
        float interest = 0f;
        Noise lastTap = GetLastUserTap();
        if (lastTap != null)
        {
            if (Time.time <= lastTap.m_timeCreated + m_maxTapInterest)
            {
                interest = m_maxTapInterest - (Time.time - lastTap.m_timeCreated);
            }
        }
        //Debug.Log(interest);
        return interest;
    }

    void Listen()
    {
        // Get Noises created since last listen
        m_audibleNoises.Clear();
        m_userTaps.Clear();
        //ClearExpiredNoises();
        if (m_noiseManager)
        {
            //float currentListen = Time.time;
            m_noiseManager.GetAudibleNoisesAtLocation(m_audibleNoises, transform.position, m_hearingRange);
            m_noiseManager.GetUserTapsAtLocation(m_userTaps, transform.position, m_hearingRange);
            //m_lastListenTime = currentListen;
        }
    }

    void FilterOutScreamerNoise()
    {
        List<Noise> filterOut = new List<Noise>();
        for (int i = 0; i < m_audibleNoises.Count; i++)
        {
            Noise test = m_audibleNoises[i];
            if (test != null && test.m_identifier == NoiseIdentifier.Screamer)
            {
                filterOut.Add(test);
            } 
        }
        foreach (Noise item in filterOut)
        {
            m_audibleNoises.Remove(item);
        }
    }
    
    private void SortNoises()
    {
        if (m_audibleNoises.Count > 1)
        {
            //m_audibleNoises.Sort((a, b) => -1 * a.CompareTo(b)); // sort descending
            m_audibleNoises.Sort((a, b) => -1 * Noise.ZombieSort(a,b)); // sort descending
        }
        if (m_userTaps.Count > 1)
        {
            m_userTaps.Sort((a, b) => -1 * Noise.TapSort(a, b)); // sort descending
        }
    }

    public float m_attackRange = 1f;
    public float m_attackDelay = 0.5f;

    public float m_devourRange = 1f;
    public float m_devourDelay = 0.5f;

    // Getters

    public int GetEnemiesInSightCount()
    {
        return m_enemies.Count;
    }
    public int GetCorpsesInSightCount()
    {
        return m_enemyCorpses.Count;
    }
    public int GetDestructiblesInSightCount()
    {
        return m_destructibles.Count;
    }
    public int GetUserTapCount()
    {
        return m_userTaps.Count;
    }
    public int GetAudibleNoiseCount()
    {
        return m_audibleNoises.Count;
    }

    public Noise GetLastUserTap()
    {
        object result = null;
        if (m_memory.TryGetValue(Labels.Memory.LastUserTap, out result))
        {
            return (Noise)result;
        }
        else
        {
            return null;
        }
    }

    [System.Serializable]
    public class MovementSpeeds
    {
        public float m_userTap = 10f;
        public float m_chase = 10f;
        public float m_devour = 7f;
        public float m_investigate = 5f;
        public float m_wander = 1f;
        public float m_idle = 0f;
        private Dictionary<ZombieUtilityBehaviours.BehaviourNames, float> m_speeds = new Dictionary<ZombieUtilityBehaviours.BehaviourNames, float>();

        public bool m_updateFromInspector = true;

        public MovementSpeeds()
        {
            m_speeds.Add(ZombieUtilityBehaviours.BehaviourNames.Idle, m_idle);
            m_speeds.Add(ZombieUtilityBehaviours.BehaviourNames.Wander, m_wander);
            m_speeds.Add(ZombieUtilityBehaviours.BehaviourNames.Investigate, m_investigate);
            m_speeds.Add(ZombieUtilityBehaviours.BehaviourNames.Devour, m_devour);
            m_speeds.Add(ZombieUtilityBehaviours.BehaviourNames.Chase, m_chase);
            m_speeds.Add(ZombieUtilityBehaviours.BehaviourNames.GoToUserTap, m_userTap);
        }
        public float GetMovementSpeed(ZombieUtilityBehaviours.BehaviourNames pBehaviour)
        {
            float speed = 0f;

            // if you can't find the correct speed , set speed to idle
            if (!m_speeds.TryGetValue(pBehaviour, out speed))
            {
                speed = m_idle;
                Debug.Log("Unknown behaviour. Setting Idle Speed as Default.");
            }
            
            return speed;
        }
        public void InspectorUpdate()
        {
            if (m_updateFromInspector)
            {
                m_speeds[ZombieUtilityBehaviours.BehaviourNames.Idle] = m_idle;
                m_speeds[ZombieUtilityBehaviours.BehaviourNames.Wander] = m_wander;
                m_speeds[ZombieUtilityBehaviours.BehaviourNames.Investigate] = m_investigate;
                m_speeds[ZombieUtilityBehaviours.BehaviourNames.Devour] = m_devour;
                m_speeds[ZombieUtilityBehaviours.BehaviourNames.Chase] = m_chase;
                m_speeds[ZombieUtilityBehaviours.BehaviourNames.GoToUserTap] = m_userTap;
            }
        }
    }

    [System.Serializable]
    public class SpeedBoostEffect
    {
        public bool m_enabled = false;
        public float m_speed = 20f;
        public float m_timeOutDelay = 2f;
        private float m_timeOut = 0f;
        public void Update()
        {
            if (m_timeOut <= Time.time)
            {
                m_enabled = false;
            }

        }
        public void Enable()
        {
            m_enabled = true;
            m_timeOut = Time.time + m_timeOutDelay;
        }
    }

    public SpeedBoostEffect m_speedBoostEffect;
    private ZombieUtilityAI m_zombieUtilityAIScript = null;
    private Movement m_movementScript = null;
    public MovementSpeeds m_movementSpeeds;
    public bool UpdateSpeed()
    {
        m_movementSpeeds.InspectorUpdate();
        bool result = true;
        float speed = 0f;

        // this section of code is used to increase the speed of the zombies
        // based on whether they are under the dictators speed boost effect
        if (m_speedBoostEffect.m_enabled)
        {
            if (m_movementScript != null)
            {
                m_movementScript.SetSpeed(m_speedBoostEffect.m_speed);
                return true; /// early exit
            }
        }

        //float speed = m_movementSpeeds.GetMovementSpeed(ZombieUtilityBehaviours.BehaviourNames.Idle);
        if (m_zombieUtilityAIScript != null)
        {
            
            speed = m_movementSpeeds.GetMovementSpeed(m_zombieUtilityAIScript.GetCurrentBehaviour());
        }
        else
        {
            result = false;
        }

        if (m_movementScript != null && result)
        {
            m_movementScript.SetSpeed(speed);
        }
        else
        {
            result = false;
        }
        return result;
    }

    private void UpdateBoredom()
    {
        
        ZombieUtilityBehaviours.BehaviourNames currentBehaviour = m_zombieUtilityAIScript.GetCurrentBehaviour();
        switch (currentBehaviour)
        {
            case ZombieUtilityBehaviours.BehaviourNames.Idle:
                IncrementBoredom(currentBehaviour);
                break;
            case ZombieUtilityBehaviours.BehaviourNames.Investigate:
            case ZombieUtilityBehaviours.BehaviourNames.Wander:
            case ZombieUtilityBehaviours.BehaviourNames.Devour:
                break;
            case ZombieUtilityBehaviours.BehaviourNames.Chase:
            case ZombieUtilityBehaviours.BehaviourNames.GoToUserTap:
            case ZombieUtilityBehaviours.BehaviourNames.Death:
                ResetBoredom();
                break;
            default:
                break;
        }
    }

    private void IncrementBoredom(ZombieUtilityBehaviours.BehaviourNames behaviour, float modifer = 1.0f)
    {
        float boredomIncrement = m_boredomIncrement;
        if (m_movementScript != null)
        {
            boredomIncrement -= m_movementSpeeds.GetMovementSpeed(behaviour);
            boredomIncrement = Mathf.Clamp(boredomIncrement, 0, m_boredomIncrement);
        }
        m_currentBoredom += boredomIncrement * modifer;
        m_currentBoredom = Mathf.Clamp(m_currentBoredom, 0, m_maxBoredom);
    }
    public void ResetBoredom()
    {
        m_currentBoredom = 0;
    }

    public bool GetObjectFromMemory(string pMemoryLocation, out object pObject)
    {
        return m_memory.TryGetValue(pMemoryLocation, out pObject);
    }


    //private void ClearExpiredNoises()
    //{
    //    List<Noise> expiredNoises = new List<Noise>();
    //    foreach (Noise noise in m_audibleNoises)
    //    {
    //        //noise.Update(deltaTime);
    //        if (noise.IsExpired())
    //        {
    //            expiredNoises.Add(noise);
    //        }
    //    }
    //    foreach (Noise noise in m_tapsNoises)
    //    {
    //        //noise.Update(deltaTime);
    //        if (noise.IsExpired())
    //        {
    //            expiredNoises.Add(noise);
    //        }
    //    }
    //    // clean up
    //    foreach (Noise noise in expiredNoises)
    //    {
    //        if (noise.m_identifier == NoiseIdentifier.UserTap)
    //        {
    //            m_tapsNoises.Remove(noise);
    //        }
    //        else
    //        {
    //            m_audibleNoises.Remove(noise);
    //        }
    //    }
    //}


}
