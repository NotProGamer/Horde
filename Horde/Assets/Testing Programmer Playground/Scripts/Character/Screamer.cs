using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Screamer : NoiseGenerator {

    private NoiseIdentifier m_noisIdentifier = NoiseIdentifier.Screamer;
    public float m_volume = 10f;
    public float m_delay = 2.0f;
    
    public int m_screams = 5;
    public int m_screamCounter = 0;
    public bool m_screaming = false;


    void Awake()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(Labels.Tags.GameController);
        if (obj)
        {
            m_noiseManager = obj.GetComponent<NoiseManager>();
            m_objectPoolManagerScript = obj.GetComponent<ObjectPoolManager>();
        }
        //else
        //{
        //    Debug.Log("GameController not included.");
        //}
        if (m_noiseManager == null)
        {
            Debug.Log("Noise Manager not included.");
        }
        if (m_objectPoolManagerScript == null)
        {
            Debug.Log("ObjectPoolManager not included");
        }


    }

    //   // Use this for initialization
    void Start()
    {
        m_screamCounter = m_screams;
        m_myNoises = new List<Noise>();
        m_cleanUpMyExpiredNoises = true;
    }

    // Update is called once per frame
    void Update ()
    {
        // if want to scream
        if (m_screaming)
        {
            // if not screaming
            if (m_myNoises.Count == 0)
            {
                Scream();
                m_screamCounter--;
                PlayScreamSound();
            }

            // if out of screams
            if (m_screamCounter <= 0)
            {
                m_screamCounter = m_screams;
                m_screaming = false;
            }
        }

        if (m_cleanUpMyExpiredNoises)
        {
            CleanExpiredNoises();
        }

    }

    private ObjectPoolManager m_objectPoolManagerScript = null;

    public void Scream()
    {
        // generate a noise half as long as the delay between noises
        Noise noise = GenerateNoise(m_volume, m_delay * 0.5f, m_noisIdentifier);

        if (noise != null)
        {
            //GameObject obj = Instantiate(m_noiseTemplate) as GameObject;
            GameObject obj = m_objectPoolManagerScript.RequestObjectAtPosition(Labels.Tags.NoiseVisualisation, noise.m_position);

            NoiseVisualization noiseVis = obj.GetComponent<NoiseVisualization>();
            if (noiseVis != null)
            {
                noiseVis.SetNoise(noise);
            }
        }


    }

    public void StartScreaming()
    {
        m_screaming = true;
    }

    public void StopScreaming()
    {
        m_screaming = false;
    }

    public string m_screamSoundTag = "ScreamerScream";

    void PlayScreamSound()
    {
        SoundLibrary.PlaySound(gameObject, m_screamSoundTag);
    }
}
