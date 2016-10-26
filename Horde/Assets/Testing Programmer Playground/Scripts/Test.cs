using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {


    private NavMeshAgent m_navAgent = null;

    public Transform m_followTarget;

    void Awake()
    {
        m_navAgent = GetComponent<NavMeshAgent>();
    }

    // Use this for initialization
    void Start ()
    {
        //m_navAgent.SetDestination(m_followTarget.position);
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_navAgent.SetDestination(m_followTarget.position);
    }
}
