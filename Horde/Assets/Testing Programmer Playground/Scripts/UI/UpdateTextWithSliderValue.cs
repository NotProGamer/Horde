using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateTextWithSliderValue : MonoBehaviour {

    public GameObject m_slider = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (m_slider)
        {
            GetComponent<Text>().text = m_slider.GetComponent<Slider>().value.ToString();
        }
	    
	}
}
