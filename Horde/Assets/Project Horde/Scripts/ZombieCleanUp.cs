using UnityEngine;
using System.Collections;

public class ZombieCleanUp : MonoBehaviour {

    //private ObjectPoolManager m_objectPoolManagerScript = null;
    private Health m_healthScript = null;
    public float m_cleanUpDelay = 3.0f;
    public float m_cleanUpTime = 0.0f;
    private bool m_timeToCleanUp = false;

    void Awake()
    {
        //GameObject gameController = GameObject.FindGameObjectWithTag(Labels.Tags.GameController);

        //if (gameController)
        //{
        //    m_objectPoolManagerScript = gameController.GetComponent<ObjectPoolManager>();
        //}
        //else
        //{
        //    Debug.Log("Unable to Find GameController");
        //}

        //if (m_objectPoolManagerScript == null)
        //{
        //    Debug.Log("ObjectPoolManager not included on GameController");
        //}

        m_healthScript = GetComponent<Health>();
        if (m_healthScript == null)
        {
            Debug.Log("Health not included");
        }

    }

    // Use this for initialization
    void Start ()
    {
        InitialiseZombieCleanup();
    }

    void OnEnable()
    {
        InitialiseZombieCleanup();
    }

    // Update is called once per frame
    void Update () {
        if (m_healthScript)
        {
            if (m_healthScript.IsDead())
            {
                if (!m_timeToCleanUp)
                {
                    m_timeToCleanUp = true;
                    m_cleanUpTime = Time.time + m_cleanUpDelay;
                }
                else if (Time.time >= m_cleanUpTime)
                {
                    CleanUp();
                }
            }
        }

    }

    void InitialiseZombieCleanup()
    {
        m_timeToCleanUp = false;
    }

    void CleanUp()
    {
        gameObject.SetActive(false);
    }


}
