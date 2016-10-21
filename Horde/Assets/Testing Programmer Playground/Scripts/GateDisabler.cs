using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GateDisabler : MonoBehaviour
{

    private Health m_health = null;

    public List<GameObject> m_gateParts;


    void Awake()
    {
        m_health = GetComponent<Health>();
        if (m_health == null)
        {
            Debug.Log("Health not included.");
        }
    }

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (m_health)
        {
            if (m_health.IsDead())
            {
                // Do stuff
                DisableGate();
            }
        }
	}

    private void DisableGate()
    {
        for (int i = 0; i < m_gateParts.Count; i++)
        {
            GameObject test = m_gateParts[i];
            if (test != null)
            {
                test.SetActive(false);
            }
        }
    }
}
