using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DisplaySafeProgressBar : MonoBehaviour {

    public GameObject m_progressBar = null;
    public GameObject m_infectedBar = null;
    public int m_count = 0;
    public int m_max = 0;

    [System.Serializable]
    public class SafeZone
    {
        public Transform m_location;
        public SafeZoneHumanCounter m_counter;
    }
    public List<SafeZone> m_safeZones = new List<SafeZone>();

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        bool test = false;
        bool test2 = false;
        for (int i = 0; i < m_safeZones.Count; i++)
        {
            SafeZone zone = m_safeZones[i];
            if (CheckSafeZoneOnScreen(zone.m_location))
            {
                test = true;
                UpdateSafeZoneSlider(zone.m_counter);
                test = m_count != 0; // disable if safe complete

                test2 = m_count == 0;
            }
        }

        EnableSafeProgressBar(test);
        EnableInfectedBar(test2);

    }

    private void EnableSafeProgressBar(bool value)
    {
        if (m_progressBar)
        {
            m_progressBar.SetActive(value);
        }
    }
    private void EnableInfectedBar(bool value)
    {
        if (m_infectedBar)
        {
            m_infectedBar.SetActive(value);
        }
    }
    private void UpdateSafeZoneSlider(SafeZoneHumanCounter counter)
    {
        if (counter)
        {
            m_count = counter.m_counter - counter.humansKilled;
            m_max = counter.m_counter;
        }
    }

    private bool CheckSafeZoneOnScreen(Transform location)
    {
        bool result = false;

        Vector3 screenPoint = Camera.main.WorldToViewportPoint(location.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        result = onScreen;
        return result;
    }


}
