using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Listen : MonoBehaviour {

    public float m_hearingRange = 10f;


    void Awake()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(Labels.Tags.GameController);
        if (obj)
        {
            m_noiseManager = obj.GetComponent<NoiseManager>();
            if (m_noiseManager == null)
            {
                Debug.Log("NoiseManager not included!");
            }

        }
        else
        {
            Debug.Log("GameController not included!");
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private Noise m_mostAudibleNoise = null;
    private NoiseManager m_noiseManager = null;
    private List<Noise> m_audibleNoises = null;
    public Noise m_currentTargetNoise = null;

}
