using UnityEngine;
using System.Collections;

public class TimedCleanUp : MonoBehaviour {

    public bool m_startTimerOnInstantiation = true;
    public bool m_enabled = false;
    public float m_delay = 2.0f;
    private float m_timer = 0f;
	// Use this for initialization
	void Start ()
    {
        if (m_startTimerOnInstantiation)
        {
            Enable();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_enabled)
        {
            if (Time.time > m_timer)
            {
                Destroy(gameObject);
            }
        }
	}

    void Enable()
    {
        m_enabled = true;
        m_timer = Time.time + m_delay;
    }



}
