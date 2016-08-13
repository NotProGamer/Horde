using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Keeps track of all noises created by gameObject, also sends generated noises to the noise manager
// OnDestroy and OnDisable removes noises from the noise manager to avoid noises being audible afte the owner gameobject is destroyed


public class NoiseGenerator : MonoBehaviour {

    /// <summary>
    /// if true calls CleanExpiredNoises() during update
    /// </summary>
    public bool m_cleanUpMyExpiredNoises = false;
    private List<Noise> m_myNoises = null;
    private NoiseManager m_noiseManager = null;

    void Awake()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(Tags.GameController);
        if (obj)
        {
            m_noiseManager = obj.GetComponent<NoiseManager>();
        }
        //else
        //{
        //    Debug.Log("GameController not included.");
        //}
        if (m_noiseManager == null)
        {
            Debug.Log("Noise Manager not included.");
        }
    }

	// Use this for initialization
	void Start ()
    {
        m_myNoises = new List<Noise>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_cleanUpMyExpiredNoises)
        {
            CleanExpiredNoises();
        }
    }


    void OnDestroy()
    {
        if (m_noiseManager)
        {
            foreach (Noise noise in m_myNoises)
            {
                m_noiseManager.Remove(noise);
            }
        }
    }


    void OnDisable()
    {
        //if (m_noiseManager)
        //{
        //    foreach (Noise noise in m_myNoises)
        //    {
        //        m_noiseManager.Remove(noise);
        //    }
        //}
    }

    /// <summary>
    /// Creates a noise and sends it to the noise manager
    /// </summary>
    /// <param name="volume"> initial volume of the noise</param>
    /// <param name="reduction"> reduction of the noise over time </param>
    public void GenerateNoise(float volume, float reduction, NoisePriority priority = NoisePriority.NoPriority)
    {
        if (m_noiseManager)
        {
            m_myNoises.Add(m_noiseManager.Add(transform.position, volume, reduction, priority));
        }
    }


    /// <summary>
    /// removes any expired noises from the local noise list. Used during update
    /// </summary>
    private void CleanExpiredNoises()
    {
        List<Noise> deathRow = new List<Noise>();
        foreach (Noise noise in m_myNoises)
        {
            noise.Update(Time.deltaTime);
            if (noise.IsExpired())
            {
                deathRow.Add(noise);
            }
        }

        foreach (Noise noise in deathRow)
        {
            m_myNoises.Remove(noise);
        }
    }

}
