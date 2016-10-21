using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SafeZoneHumanCounter : MonoBehaviour {

    public List<GameObject> m_humans;
    private ObjectiveUpdater m_updater = null;
    public int m_counter = 0;


    void Awake()
    {
        m_updater = GetComponent<ObjectiveUpdater>();
    }
	// Use this for initialization
	void Start () {

        foreach (GameObject item in m_humans)
        {
            m_counter++;
        }

    }

    // Update is called once per frame
    void Update () {
        int stillAlive = 0;

        foreach (GameObject item in m_humans)
        {
            Health health = item.GetComponent<Health>();
            if (health.IsDead())
            {
                stillAlive++;
            }
        }

        if (stillAlive >= m_counter)
        {
            // complete
            if (m_updater)
            {
                m_updater.SetStatus(ObjectiveStatus.Complete);
            }
        }

    }
}
