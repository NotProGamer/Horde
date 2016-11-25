using UnityEngine;
using System.Collections;

public class NoiseCircleCleanUp : MonoBehaviour {

    public NoiseVisualization m_visualiser = null;

    void Awake()
    {
        m_visualiser = GetComponent<NoiseVisualization>();
        if (m_visualiser == null)
        {
            Debug.Log("NoiseVisualisation not included");
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (m_visualiser)
        {
            if (m_visualiser.m_noise != null)
            {
                if (m_visualiser.m_noise.IsExpired())
                {
                    CleanUp();
                }
            }
            else
            {
                CleanUp();
            }
        }
	}

    void CleanUp()
    {
        gameObject.SetActive(false);
    }
}
