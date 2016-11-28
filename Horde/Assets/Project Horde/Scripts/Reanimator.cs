using UnityEngine;
using System.Collections;

public class Reanimator : MonoBehaviour {

    public float m_reanimationDelay = 3.0f;

    private ObjectPoolManager m_objectPoolManagerScript = null;
    private Health m_healthScript = null;
    private HealthCondition m_conditionScript = null;

    private float m_timeUntilReanimation = 0.0f;
    private bool m_turned = false;
    private bool m_reanimating = false;

    private GameObject m_zombieHandTargetObj = null;

    void Awake()
    {
        GameObject gameController = GameObject.FindGameObjectWithTag(Labels.Tags.GameController);

        if (gameController)
        {
            m_objectPoolManagerScript = gameController.GetComponent<ObjectPoolManager>();
        }
        else
        {
            Debug.Log("Unable to Find GameController");
        }

        if (m_objectPoolManagerScript == null)
        {
            Debug.Log("ObjectPoolManager not included on GameController");
        }

        m_healthScript = GetComponent<Health>();
        if (m_healthScript == null)
        {
            Debug.Log("Health not included");
        }

        m_conditionScript = GetComponent<HealthCondition>();
        if (m_conditionScript == null)
        {
            Debug.Log("InfectionStatus not included");
        }
        

        GameObject obj = GameObject.FindGameObjectWithTag(Labels.Tags.UIController);
        if (obj)
        {
            m_zombieHandSpawnerScript = obj.GetComponent<ZombieHandSpawner>();
        }
        if (m_zombieHandSpawnerScript == null)
        {
            Debug.Log("ZombieHandSpawner not included");
        }

        //if (m_HUDIndicatorPrefab == null)
        //{
        //    Debug.Log("HUDIndicator not included");
        //}
        //GameObject m_zombieHandTargetObj = GameObject.FindGameObjectWithTag(Labels.Tags.ZombieHandTarget);

        //if (m_zombieHandTargetObj == null)
        //{
        //    Debug.Log("ZombieHandTarget not included");
        //}
    }

    // Use this for initialization
    void Start()
    {
        InitialiseReanimator();
    }

    void OnDisable()
    {
        InitialiseReanimator();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_turned)
        {
            if (m_healthScript && m_conditionScript)
            {
                if (m_healthScript.IsDead() && m_conditionScript.IsInfected())
                {
                    StartReanimationTimer();
                    m_turned = true;
                }
            }
        }

        if (m_reanimating)
        {
            //===========
            //Added by Rory
            //===========
            //NavMeshAgent m_agent = GetComponent<NavMeshAgent>();
            //m_agent.Stop(); // removed by reece

            if (Time.time > m_timeUntilReanimation)
            {
                Reanimate();
            }
        }

    }

    private void InitialiseReanimator()
    {
        m_timeUntilReanimation = 0.0f;
        m_turned = false;
        m_reanimating = false;
    }

    public void StartReanimationTimer()
    {
        m_reanimating = true;
        m_timeUntilReanimation = Time.time + m_reanimationDelay;
    }

    
    private void Reanimate()
    {
        if (m_objectPoolManagerScript)
        {
            if (m_healthScript)
            {
                if (!m_healthScript.IsDevoured())
                {
                    if (m_objectPoolManagerScript.RequestObjectAtPosition(Labels.Tags.Zombie, transform.position) != null)
                    {
                        RequestZombieHand();
                    }
                }
            }
            gameObject.SetActive(false);
        }


    }

    public bool IsTurned()
    {
        return m_turned;
    }


    private ZombieHandSpawner m_zombieHandSpawnerScript = null;

    //private ObjectPoolManager m_UIObjectPoolManagerScript = null;
    //private Vector3 m_zombieHandTargetPosition;
    //public GameObject m_HUDIndicatorPrefab = null;

    //public void RequestZombieHand()
    //{
    //    if (m_UIObjectPoolManagerScript)
    //    {
    //        GameObject uiObj = m_UIObjectPoolManagerScript.RequestObjectAtPosition(Labels.Tags.PlusZombieIndicator, gameObject.transform.position);
    //        if (uiObj)
    //        {
    //            if (m_zombieHandTargetObj)
    //            {
    //                m_zombieHandTargetPosition = m_zombieHandTargetObj.transform.position;
    //            }
    //            //else
    //            //{
    //            //    Debug.Log("ZombieHandTarget not included");
    //            //}
    //            MoveUISprite spriteMoveScript = uiObj.GetComponent<MoveUISprite>();

    //            if (spriteMoveScript)
    //            {
    //                spriteMoveScript.SetTarget(m_zombieHandTargetPosition);
    //            }
    //        }
    //        else
    //        {
    //            //Debug.Log("error blah");
    //        }

    //    }
    //}

    public void RequestZombieHand()
    {
        if (m_zombieHandSpawnerScript)
        {
            m_zombieHandSpawnerScript.SpawnUIAtWorldLocation(gameObject.transform.position);
            //GameObject uiObj = m_UIObjectPoolManagerScript.RequestObjectAtPosition(Labels.Tags.PlusZombieIndicator, gameObject.transform.position);
            //if (uiObj)
            //{
            //    if (m_zombieHandTargetObj)
            //    {
            //        m_zombieHandTargetPosition = m_zombieHandTargetObj.transform.position;
            //    }
            //    //else
            //    //{
            //    //    Debug.Log("ZombieHandTarget not included");
            //    //}
            //    MoveUISprite spriteMoveScript = uiObj.GetComponent<MoveUISprite>();

            //    if (spriteMoveScript)
            //    {
            //        spriteMoveScript.SetTarget(m_zombieHandTargetPosition);
            //    }
            //}
            //else
            //{
            //    //Debug.Log("error blah");
            //}

        }
    }
}
