using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SafeZoneHumanCounter : MonoBehaviour
{

    public List<GameObject> m_humans;
    private ObjectiveUpdater m_updater = null;
    public int m_counter = 0;
    public float survivingPercentage;
    public bool displaySurvivors;
    public bool survivorsDisplayed;


    void Awake()
    {
        m_updater = GetComponent<ObjectiveUpdater>();
    }
	// Use this for initialization
	void Start ()
    {
        foreach (GameObject item in m_humans)
        {
            m_counter++;
        }
    }

    // Update is called once per frame
    void Update ()
    {

        int humansKilled = 0;

        foreach (GameObject item in m_humans)
        {
            Health health = item.GetComponent<Health>();
            if (health.IsDead())
            {
                humansKilled++;
            }
        }

        if (humansKilled >= (m_counter*survivingPercentage))
        {
            displaySurvivors = true;
            //Display icon over remaining humans
            
        }


        if (humansKilled >= m_counter)
        {
            // complete
            if (m_updater)
            {
                m_updater.SetStatus(ObjectiveStatus.Complete);
            }
        }

        if (displaySurvivors && !survivorsDisplayed)
        {
            DisplayRemainingHumans();
            survivorsDisplayed = true;
        }




    }

    void DisplayRemainingHumans()
    {
        foreach (GameObject item in m_humans)
        {
            if (gameObject.activeSelf)
            {
                Transform display = item.transform.FindChild("RemainingHumans");
                display.gameObject.SetActive(true);
            }
        }
    }
}
