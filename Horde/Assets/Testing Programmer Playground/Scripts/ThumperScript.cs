using UnityEngine;
using System.Collections;
using System;

// this script generates noises periodically


public class ThumperScript : MonoBehaviour {

    private NoiseGenerator m_noiseGeneratorScript = null;
    // Use this for initialization
    void Start()
    {
        m_noiseGeneratorScript = GetComponent<NoiseGenerator>();
        if (m_noiseGeneratorScript == null)
        {
            Debug.Log("NoiseGenerator not Included!");
        }



    }

    public float m_delay = 2.0f;
    public float m_volume = 5.0f;
    public NoiseIdentifier m_identifier = NoiseIdentifier.Silent;
    private float m_nextThumpTime = 0f;


	// Update is called once per frame
	void Update ()
    {
        if (m_nextThumpTime < Time.time)
        {
            Thump();
            m_nextThumpTime = Time.time + m_delay;
        }
	}

    private void Thump()
    {
        m_noiseGeneratorScript.GenerateNoise(m_volume, m_delay * 0.5f, m_identifier);
    }
}
