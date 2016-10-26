using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SafeZoneUICount : MonoBehaviour
{
    ObjectiveManager m_manager = null;
    Text safeZoneCount = null;
    int completedSafeZones = 0;
    int totalSafeZones = 0;

    void Awake()
    {
        safeZoneCount = GetComponent<Text>();
        GameObject obj = GameObject.FindGameObjectWithTag(Labels.Tags.GameController);
        if (obj)
        {
            m_manager = obj.GetComponent<ObjectiveManager>();
        }
            
    }

	// Use this for initialization
	void Start ()
    {
        totalSafeZones = m_manager.m_objectives.Count;
        safeZoneCount.text = completedSafeZones + " of " + totalSafeZones;	
	}
	
	void Update ()
    {
        if (completedSafeZones != m_manager.completedObjectives)
        {
            completedSafeZones = m_manager.completedObjectives;
            safeZoneCount.text = completedSafeZones + " of " + totalSafeZones;
        }
	}
}
