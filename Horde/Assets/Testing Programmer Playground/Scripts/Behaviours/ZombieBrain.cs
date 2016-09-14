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
    public float m_hearingRange = 50f;


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
        Look();


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
    private List<Noise> m_audibleNoises = null;
    private List<Noise> m_tapsNoises = null;

    void Listen()
    {
        m_audibleNoises.Clear();
        if (m_noiseManager)
        {
            m_noiseManager.GetAudibleNoisesAtLocation(m_audibleNoises, transform.position);
        }
    }

}
