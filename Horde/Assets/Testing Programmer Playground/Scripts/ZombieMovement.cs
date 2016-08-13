using UnityEngine;
using System.Collections;

public class ZombieMovement : MonoBehaviour {

    private NavMeshAgent m_nav = null;
    private Noise m_mostAudibleNoise = null;
    private NoiseManager m_noiseManager = null;
    private float m_touchRange = 1f; // may need to get this from attackRange or moveRange

    public float m_minSpeed = 1f;
    public float m_maxSpeed = 10f;
    private float m_currentSpeed = 1;

    void Awake()
    {
        m_nav = GetComponent<NavMeshAgent>();
        if (m_nav == null)
        {
            Debug.Log("NavMeshAgent not included");
        }

        GameObject obj = GameObject.FindGameObjectWithTag(Tags.GameController);
        if (obj)
        {
            m_noiseManager = obj.GetComponent<NoiseManager>();
        }
        
    }


	// Use this for initialization
	void Start ()
    {
        m_currentSpeed = m_minSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        if (m_nav)
        {
            // This will need to be tested further
            m_nav.speed = m_currentSpeed;
            m_nav.SetDestination(GetCurrentTargetPosition());
            
        }
        //if (m_mostAudibleNoise != null && ReachedDestination())
        //{
        //    m_mostAudibleNoise = null;
        //    Debug.Log("ReachedDestination");
        //}
        if (m_noiseManager)
        {
            // if there is a more audible noise generated it will overrate the current most audible noise
            Noise mostAudibleNoise = m_noiseManager.GetMostAudibleNoise(transform.position, true);
            if (mostAudibleNoise != null)
            {
                //Debug.Log((mostAudibleNoise.m_position - transform.position).sqrMagnitude);
                m_mostAudibleNoise = mostAudibleNoise;
                //m_currentSpeed = CalculateSpeed(m_mostAudibleNoise.m_volume);
                m_currentSpeed = m_maxSpeed;
            }
        }



        //DetermineCurrentSpeed();

    }



    float m_test = 0f;
    float m_delay = 5f;


    private Vector3 GetCurrentTargetPosition()
    {
        
        Vector3 currentTarget = transform.position;
        if (m_mostAudibleNoise != null)
        {
            // hunt
            currentTarget = m_mostAudibleNoise.m_position;
            //Debug.Log("hunt to: " + currentTarget);
        }
        else
        {
            //// wander
            //if (Time.time > m_test)
            //{
            //    m_test = Time.time + m_delay;
                
            //    Vector2 test2 = Random.insideUnitCircle * 10f;
            //    Vector3 randomTarget = transform.position + new Vector3(test2.x, 0, test2.y);
            //    //Debug.Log(randomTarget);
            //    NavMeshHit hit;
            //    if (NavMesh.SamplePosition(randomTarget, out hit, 100f, NavMesh.AllAreas))
            //    {
            //        currentTarget = hit.position;
            //        currentTarget.y = 1.0f;
            //    }
            //    else
            //    {
            //        currentTarget = transform.position;
            //    }
            //    Debug.Log("wander to: " + currentTarget);
            //}
        }
        return currentTarget;
    }

    private bool ReachedDestination()
    {
        bool result = false;
        if (m_nav)
        {
            result = m_nav.remainingDistance <= m_touchRange;
        }

        return result;
    }

    //private float CalculateSpeed(float volumeMultiplier)
    //{
    //    float result = m_minSpeed;
    //    result += volumeMultiplier;
    //    result = Mathf.Clamp(result, m_minSpeed, m_maxSpeed);
    //    return result;
    //}

    private float DetermineCurrentSpeed()
    {
        float result = m_minSpeed;
        if (m_mostAudibleNoise != null)
        {
            result = m_maxSpeed;
        }
        return result;
    }
}

