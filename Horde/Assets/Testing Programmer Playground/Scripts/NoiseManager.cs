using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Noise
{
    public Vector3 m_position;
    public float m_volume;
    public float m_reductionOverTime;

    private static int m_id = 0;
    public Noise(Vector3 position, float volume, float reduction)
    {
        m_position = position;
        m_volume = volume;
        m_reductionOverTime = reduction;
        m_id += 1; 
    }

    public int GetNoiseID()
    {
        return m_id;
    }
    public float CalculateAudabilityFromPosition(Vector3 audiencePosition)
    {
        float audible = 0f;
        float distance = (m_position - audiencePosition).sqrMagnitude;
        audible = m_volume / (0.1f + distance);
        return audible;
    }

    public void Update(float deltaTime)
    {
        if (m_volume > 0)
        {
            m_volume -= m_reductionOverTime * deltaTime;
        }
        if (m_volume < 0)
        {
            m_volume = 0;
        }
    }

    public bool IsExpired()
    {
        return m_volume == 0;
    }
}

public class NoiseManager : MonoBehaviour {

    [System.Serializable]
    public class NoiseLibrary
    {

        public List<Noise> m_noises = new List<Noise>();

        public int Count()
        {
            return m_noises.Count;
        }

        public Noise Add(Vector3 position, float volume, float reduction)
        {
            Noise noise = new Noise(position, volume, reduction);
            m_noises.Add(noise);
            return noise;
        }

        public void Remove(Noise noise)
        {
            m_noises.Remove(noise);
        }

        //public Vector3 GetMostAudibleNoiseLocation(Vector3 audiencePosition)
        //{
        //    Noise mostAudible = null;
        //    float mostAudibleNoiseLevel = 0f;

        //    foreach (Noise noise in m_noises)
        //    {
        //        float noiseLevel = noise.CalculateAudabilityFromPosition(audiencePosition);

        //        if (mostAudible == null || mostAudibleNoiseLevel < noiseLevel)
        //        {
        //            mostAudible = noise;
        //            mostAudibleNoiseLevel = noiseLevel;
        //        }
        //    }

        //    return mostAudible.m_position;
        //}

        public Noise GetMostAudibleNoise(Vector3 audiencePosition)
        {
            Noise mostAudible = null;
            float mostAudibleNoiseLevel = 0f;

            foreach (Noise noise in m_noises)
            {
                float noiseLevel = noise.CalculateAudabilityFromPosition(audiencePosition);

                if (mostAudible == null || mostAudibleNoiseLevel < noiseLevel)
                {
                    mostAudible = noise;
                    mostAudibleNoiseLevel = noiseLevel;
                }
            }

            return mostAudible;
        }


        public void Update(float deltaTime)
        {
            List<Noise> expiredNoises = new List<Noise>();

            foreach (Noise noise in m_noises)
            {
                noise.Update(deltaTime);
                if (noise.IsExpired())
                {
                    expiredNoises.Add(noise);
                }
            }

            // clean up
            foreach (Noise noise in expiredNoises)
            {
                m_noises.Remove(noise);
            }
        }

        public bool IsEmpty()
        {
            return m_noises.Count < 1;
        }
    }

    public NoiseLibrary m_noiseLibrary = new NoiseLibrary();

    //// Use this for initialization
    //void Start ()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        m_noiseLibrary.Update(Time.deltaTime);
    }

    public Noise GetMostAudibleNoise(Vector3 position)
    {
        return m_noiseLibrary.GetMostAudibleNoise(position);
    }

    public Noise Add(Vector3 position, float volume, float reduction)
    {
        return m_noiseLibrary.Add(position, volume, reduction);
    }

    public void Remove(Noise noise)
    {
        if (!m_noiseLibrary.IsEmpty())
        {
            m_noiseLibrary.Remove(noise);
        }
    }
}
