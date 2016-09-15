using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieBrain : MonoBehaviour {

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
    //public float m_hearingRange = 50f; // not yet implemented

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
    }


    // Use this for initialization
    void Start ()
    {
        m_memory.Add("CurrentTarget", transform.position);
        m_memory.Add("LastUserTap", null);
        m_memory.Add("LastPriorityNoise", null);
        m_memory.Add("ClosestEnemy", null);
        m_memory.Add("ClosestCorpse", null);
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

        // Think
        SortNoises();


        // modify memory
        if (m_userTaps.Count > 0)
        {
            m_memory["LastUserTap"] = m_userTaps[0];
        }

        if (m_audibleNoises.Count > 0)
        {
            m_memory["LastPriorityNoise"] = m_audibleNoises[0];
        }

        if (m_enemies.Count > 1)
        {
            m_memory["ClosestEnemy"] = GetClosest(m_enemies);
        }
        else if (m_enemies.Count > 0)
        {
            m_memory["ClosestEnemy"] = m_enemies[0];
        }
        else
        {
            m_memory["ClosestEnemy"] = null;
        }


        if (m_enemyCorpses.Count > 1)
        {
            m_memory["ClosestCorpse"] = GetClosest(m_enemyCorpses);
        }
        else if (m_enemyCorpses.Count > 0)
        {
            m_memory["ClosestCorpse"] = m_enemyCorpses[0];
        }
        else
        {
            m_memory["ClosestCorpse"] = null;
        }




    }

    public bool GetCurrentTargetPosition(out Vector3 pTargetPosition)
    {
        bool result = false;
        Vector3 position = Vector3.zero;
        object currentTarget = null;
        if (m_memory.TryGetValue("CurrentTarget", out currentTarget))
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
            Debug.Log("Unable to find 'CurrentTarget'");
        }
        pTargetPosition = position;
        return result;
    }

    private List<GameObject> m_enemies = new List<GameObject>();
    private List<GameObject> m_enemyCorpses = new List<GameObject>();
    private List<GameObject> m_allies = new List<GameObject>();
    

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
                    if (currentObjectHealth != null && currentObjectHealth.IsDead() && !currentObjectHealth.IsDevoured())
                    {
                        m_enemyCorpses.Add(currentObject);
                    }
                    else
                    {
                        m_enemies.Add(currentObject);
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

    public void SetCurrentTarget(object pCurrentTarget)
    {
        m_memory["CurrentTartget"] = pCurrentTarget;
    }

    private NoiseManager m_noiseManager = null;
    private List<Noise> m_audibleNoises = new List<Noise>();
    private List<Noise> m_userTaps = new List<Noise>();
    private List<Noise> m_investigated = new List<Noise>();
    //private float m_lastListenTime = 0f;

    public float m_tapInterest = 2.0f;


    void Listen()
    {
        
        // Get Noises created since last listen
        m_audibleNoises.Clear();
        m_userTaps.Clear();
        //ClearExpiredNoises();
        if (m_noiseManager)
        {
            //float currentListen = Time.time;
            m_noiseManager.GetAudibleNoisesAtLocation(m_audibleNoises, transform.position);
            m_noiseManager.GetUserTapsAtLocation(m_userTaps, transform.position);
            //m_lastListenTime = currentListen;
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
        return (Noise)m_memory["LastUserTap"];
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
