using UnityEngine;
using System.Collections;

public class Screamer : NoiseGenerator {

    private NoiseIdentifier m_noisIdentifier = NoiseIdentifier.Screamer;
    public float m_volume = 10f;
    public float m_delay = 2.0f;
    
    public int m_screams = 5;
    public int m_screamCounter = 0;
    public bool m_screaming = false;


    //   // Use this for initialization
    void Start()
    {
        m_screamCounter = m_screams;
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


    public void Scream()
    {
        // generate a noise half as long as the delay between noises
        GenerateNoise(m_volume, m_delay * 0.5f, m_noisIdentifier);
    }

    public void StartScreaming()
    {
        m_screaming = true;
    }

    public void StopScreaming()
    {
        m_screaming = false;
    }
}
