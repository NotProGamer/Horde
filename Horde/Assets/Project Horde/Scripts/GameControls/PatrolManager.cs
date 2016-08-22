using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatrolManager : MonoBehaviour {

    [System.Serializable]
    public class PatrolRoute
    {
        public string m_identifier = "";
        public List<Transform> m_patrolPoints;
    }
    public List<PatrolRoute> m_patrolRoutes;

    // Use this for initialization
    //void Start ()    {	}

    // Update is called once per frame
    //void Update ()    {	}

    public PatrolRoute GetPatrolRoute(string identifer)
    {
        PatrolRoute patrolRoute = null;

        patrolRoute = m_patrolRoutes.Find(item => item.m_identifier == identifer);

        if (patrolRoute == null)
        {
            Debug.Log("Unable to locate '" + identifer + "' Patrol Route.");
        }

        return patrolRoute;
    }

}
