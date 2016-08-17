using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
//public enum NoisePriority
//{
//    NoPriority, // Ignored by Zombie
//    LowestPriority, // Amibent
//    LowPriority, // Other , car alarm etc.
//    MediumPrioritty, // Human
//    HighPriority, // User Taps
//    MaxPriority, // GameOverride
//}
public enum NoiseIdentifier
{
    UserTap,
    Human,
    Expolosion,
    Alarm,
    WeaponFire,
    Zombie,
    Silent
}

public class Noise
{
    public Vector3 m_position = new Vector3();
    public float m_volume = 0f; // is like a range or radius of the sound
    public NoiseIdentifier m_identifier = NoiseIdentifier.Silent;
    public float m_expiry = 0f;
    //public float m_reductionOverTime;
    //private NoisePriority m_priority = NoisePriority.NoPriority;
    //private static int m_id = 0;
    //public int GetID()
    //{
    //    return m_id;
    //}

    public Noise(Vector3 position, float volume, float expirationDelay, NoiseIdentifier identifier = NoiseIdentifier.Silent)
    {
        m_position = position;
        m_volume = volume;
        m_identifier = identifier;
        m_expiry = Time.time + expirationDelay;
        //m_id += 1; 
    }

    public bool IsExpired()
    {
        return Time.time > m_expiry;
    }

    public NoiseIdentifier Identifier()
    {
        return m_identifier;
    }

    public float CalculateAudabilityFromPosition(Vector3 audiencePosition)
    {
        float audible = 0f;
        float sqrDistance = (m_position - audiencePosition).sqrMagnitude;
        if ()

        return audible;
    }

    //public float CalculateAudabilityFromPosition(Vector3 audiencePosition)
    //{
    //    float audible = 0f;
    //    float distance = (m_position - audiencePosition).magnitude;
    //    if (m_volume > distance)
    //    {
    //        // can be heard
    //        audible = m_volume - distance;
    //    }
    //    //float sqrDistance = (m_position - audiencePosition).sqrMagnitude;
    //    //float sqrVolume = m_volume * m_volume;
    //    //if (sqrVolume > sqrDistance)
    //    //{
    //    //    // can be heard
    //    //    audible = sqrVolume - sqrDistance;
    //    //}
    //    if (audible < 0)
    //    {
    //        audible = 0;
    //    }
    //    //audible = m_volume / (0.1f + distance);
    //    //Debug.Log(sqrVolume + " - " + sqrDistance + " = " + audible);
    //    return audible;
    //}
    //public void Update(float deltaTime)
    //{
    //    if (m_volume > 0)
    //    {
    //        m_volume -= m_reductionOverTime * deltaTime;
    //    }
    //    if (m_volume < 0)
    //    {
    //        m_volume = 0;
    //    }
    //}
    //public bool IsExpired()
    //{
    //    return m_volume == 0;
    //}
    //public NoisePriority Priority()
    //{
    //    return m_priority;
    //}
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

        public Noise Add(Vector3 position, float volume, float expirationDelay, NoiseIdentifier identifier = NoiseIdentifier.Silent)
        {
            Noise noise = new Noise(position, volume, expirationDelay, identifier);
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

        public Noise GetMostAudibleNoise(Vector3 audiencePosition, bool usingNoisePriority = false)
        {
            Noise mostAudible = null;
            float mostAudibleNoiseLevel = 0f;

            foreach (Noise noise in m_noises)
            {
                float noiseLevel = noise.CalculateAudabilityFromPosition(audiencePosition);

                //if (mostAudible == null || mostAudibleNoiseLevel < noiseLevel)
                //{
                //    mostAudible = noise;
                //    mostAudibleNoiseLevel = noiseLevel;
                //}
                if (mostAudibleNoiseLevel < noiseLevel)
                {
                    if (mostAudible != null && usingNoisePriority)
                    {
                        // find audible noise of the highest priority
                        if (noise.Priority() >= mostAudible.Priority())
                        {
                            mostAudible = noise;
                            mostAudibleNoiseLevel = noiseLevel;
                        }
                    }
                    else
                    {
                        // find most audible noise
                        mostAudible = noise;
                        mostAudibleNoiseLevel = noiseLevel;
                    }
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

    public Noise GetMostAudibleNoise(Vector3 position, bool usingNoisePriority = false)
    {
        return m_noiseLibrary.GetMostAudibleNoise(position, usingNoisePriority);
    }

    public Noise Add(Vector3 position, float volume, float reduction, NoisePriority priority = NoisePriority.NoPriority)
    {
        return m_noiseLibrary.Add(position, volume, reduction, priority);
    }

    public void Remove(Noise noise)
    {
        if (!m_noiseLibrary.IsEmpty())
        {
            m_noiseLibrary.Remove(noise);
        }
    }

}
