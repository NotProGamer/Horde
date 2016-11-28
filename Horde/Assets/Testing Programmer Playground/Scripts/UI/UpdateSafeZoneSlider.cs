using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateSafeZoneSlider : MonoBehaviour {

    private Slider m_slider = null;

    void Awake()
    {
        m_slider = GetComponent<Slider>();
        if (m_slider == null)
        {
            Debug.Log("Slider not included!");
        }
    }

	// Use this for initialization
	void Start ()
    {
        m_slider.maxValue = GetMaxCount();
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_slider.value = GetCurrentCount();
	}

    private float GetCurrentCount()
    {
        return 2;
    }

    private float GetMaxCount()
    {
        return 4;
    }
}
