using UnityEngine;
using System.Collections;

public class MovePartialPathMarker : MonoBehaviour {

    private NavMeshAgent m_nav = null;

    public Transform m_partialPathMarker;

    void Awake()
    {
        m_nav = GetComponent<NavMeshAgent>();

    }


	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_nav.pathStatus == NavMeshPathStatus.PathPartial)
        {
            int lastCornerIndex = m_nav.path.corners.Length;
            if (lastCornerIndex > 0)
            {
                Vector3 test = m_nav.path.corners[lastCornerIndex -1];
                if (m_partialPathMarker != null)
                {
                    m_partialPathMarker.position = test;
                }
            }
            
        }
	}



}
