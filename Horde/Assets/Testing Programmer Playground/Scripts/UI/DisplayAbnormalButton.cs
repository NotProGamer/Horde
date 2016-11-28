using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayAbnormalButton : MonoBehaviour {

    public GameObject m_screamerButton = null;
    private Dropdown m_dropDown = null;
    void Awake()
    {
        m_dropDown = GetComponent<Dropdown>();

        if (m_screamerButton == null)
        {
            Debug.Log("Screamer button not included on DropDown");
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (m_dropDown && m_screamerButton)
        {
            m_screamerButton.SetActive(ActivateScreamerButton());

            //if (m_dropDown.value == 1)
            //{
            //    m_screamerButton.SetActive(true);
            //}
            //else
            //{
            //    m_screamerButton.SetActive(false);
            //}
        }
    }

    private bool ActivateScreamerButton()
    {
        // could also check if scream is alive 
        return m_dropDown.value == 1;
    }
}
