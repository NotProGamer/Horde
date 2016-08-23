using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    private NavMeshAgent m_nav = null;
    public Vector3 m_currentDestination = new Vector3();
    private float m_touchRange = 1f; // may need to get this from attackRange or moveRange

    public float m_minSpeed = 3f;
    public float m_maxSpeed = 10f;
    private float m_currentSpeed = 3f;


    public enum State
    {
        Idle,
        Moving,
    }
    public State m_state = State.Idle;

    void Awake()
    {
        m_nav = GetComponent<NavMeshAgent>();
        if (m_nav == null)
        {
            Debug.Log("NavMeshAgent not included");
        }
    }

    // Use this for initialization
    void Start()
    {

        m_currentSpeed = m_minSpeed;
        m_currentDestination = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (m_nav)
        {
            if (ReachedDestination())
            {
                m_state = State.Idle;
            }
        }
    }

    public void SetDestination(Vector3 position)
    {
        if (m_nav)
        {
            m_state = State.Moving;
            m_currentDestination = position;
            m_nav.speed = m_currentSpeed;
            m_nav.SetDestination(m_currentDestination);
        }
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
    public void SetSpeed(float speed)
    {
        m_currentSpeed = Mathf.Clamp(speed, m_minSpeed, m_maxSpeed);
    }
}
