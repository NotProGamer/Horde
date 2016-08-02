using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectiveChecker : MonoBehaviour {

    public ObjectiveManager.ObjectiveUpdater m_objectiveUpdater; //= new ObjectiveManager.ObjectiveUpdater();

    public List<GameObject> m_defenders;

    void Awake()
    {
        m_objectiveUpdater.Awake();
    }
    // Use this for initialization
    //void Start (){	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!CheckForActiveDefenders())
        {
            m_objectiveUpdater.m_objectiveComplete = true;
            m_objectiveUpdater.Update();
        }
    }

    bool CheckForActiveDefenders()
    {
        bool test = false;
        for (int i = 0; i < m_defenders.Count; i++)
        {
            if (m_defenders[i].activeInHierarchy)
            {
                test = true;
                break; // early exit
            }
        }
        return test;
    }
}
