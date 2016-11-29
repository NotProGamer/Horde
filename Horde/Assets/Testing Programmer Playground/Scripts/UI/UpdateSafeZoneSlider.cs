using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateSafeZoneSlider : MonoBehaviour {

    private Slider m_slider = null;
    public DisplaySafeProgressBar m_evaluator = null;
    
    void Awake()
    {
        m_slider = GetComponent<Slider>();
        if (m_slider == null)
        {
            Debug.Log("Slider not included!");
        }

        if (m_evaluator == null)
        {
            Debug.Log("Evaluator not included!");
        }
        
    }

	// Use this for initialization
	void Start ()
    {
        //m_slider.maxValue = GetMaxCount();
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_slider.value = GetCurrentCount();
        m_slider.maxValue = GetMaxCount();
    }

    private float GetCurrentCount()
    {
        if (m_evaluator)
        {
            return m_evaluator.m_count;
        }
        return 2;
    }

    private float GetMaxCount()
    {
        if (m_evaluator)
        {
            return m_evaluator.m_max;
        }
        return 4;
    }
}
