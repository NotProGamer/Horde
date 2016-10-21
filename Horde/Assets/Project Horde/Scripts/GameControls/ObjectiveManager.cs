using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ObjectiveStatus
{
    Disabled,
    Incomplete,
    InProgress,
    Complete,
}

public class ObjectiveManager : MonoBehaviour {

    public List<ObjectiveUpdater> m_objectives = new List<ObjectiveUpdater>();

    public bool m_enableDebugStatusUpdates = true;
    public float m_statusUpdateDelay = 5f;
    public float m_statusUpdateTimer = 0f;
    public int completedObjectives = 0;
    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_enableDebugStatusUpdates)
        {
            DisplayStatusUpdate();
        }
    }

    private void DisplayStatusUpdate()
    {
        if (Time.time > m_statusUpdateTimer)
        {
            foreach (ObjectiveUpdater objective in m_objectives)
            {
                if (objective.Changed())
                {
                    Debug.Log("Objective '" + objective.m_identifer + "' " + objective.m_status.ToString() + ".");
                    Debug.Log(objective.m_objectiveCompletedText);
                    //UpdateUI();
                    objective.ClearChange();
                    completedObjectives++;
                }
            }
            m_statusUpdateTimer = Time.time + m_statusUpdateTimer;
        }
    }

    //protected virtual void UpdateUI()
    //{
    //    // your code here

    //}
}
