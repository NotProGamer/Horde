using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    Silent,
    Zombie,
    WeaponFire,
    Alarm,
    Expolosion,
    Human,
    Screamer,
    UserTap,
}

[System.Serializable]
public class Noise
{
    public Vector3 m_position = new Vector3();
    public float m_volume = 0f; // is like a range or radius of the sound
    public NoiseIdentifier m_identifier = NoiseIdentifier.Silent;
    public float m_timeCreated = 0f;
    public float m_timeExpiry = 0f;
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
        m_timeCreated = Time.time;
        m_timeExpiry = Time.time + expirationDelay;
        //m_id += 1; 
    }

    public bool IsExpired()
    {
        return Time.time > m_timeExpiry;
    }

    public NoiseIdentifier Identifier()
    {
        return m_identifier;
    }

    public float GetVolumeIfAudible(Vector3 audiencePosition, float hearingSensitivity = 0f)
    {
        float audible = 0f;
        float sqrDistance = (m_position - audiencePosition).sqrMagnitude;
        float sqrVolume = (m_volume + hearingSensitivity) * (m_volume + hearingSensitivity);
        if (sqrVolume > sqrDistance)
        {
            audible = m_volume;
        }
        return audible;
    }

    //public int CompareTo(Noise pNoise)
    //{
    //    int result = 0;
    //    if (pNoise == null)
    //    {
    //        result = 1;
    //    }
    //    else
    //    {
    //        // intelligent sort (humans)
    //        //result = this.m_identifier.CompareTo(pNoise.m_identifier);
    //        //if (result == 0)
    //        //{
    //        //    result = this.m_timeCreated.CompareTo(pNoise.m_timeCreated);
    //        //    if (result == 0)
    //        //    {
    //        //        result = this.m_volume.CompareTo(pNoise.m_volume);
    //        //    }
    //        //}

    //        // stupid Sort (zombies)
    //        result = this.m_volume.CompareTo(pNoise.m_volume);
    //        if (result == 0)
    //        {
    //            result = this.m_timeCreated.CompareTo(pNoise.m_timeCreated);
    //            if (result == 0)
    //            {
    //                result = this.m_identifier.CompareTo(pNoise.m_identifier);
    //            }
    //        }

    //    }
    //    return result;
    //}

    public static int ZombieSort(Noise a, Noise b)
    {
        int result = 0;
        if (a == null && b == null)
        {
            result = 0;
        }
        else if (a == null)
        {
            result = 1;
        }
        else if (b == null)
        {
            result = -1;
        }
        else
        {
            // stupid Sort (zombies)
            result = a.m_volume.CompareTo(b.m_volume);
            if (result == 0)
            {
                result = a.m_timeCreated.CompareTo(b.m_timeCreated);
                if (result == 0)
                {
                    result = a.m_identifier.CompareTo(b.m_identifier);
                }
            }
        }
        return result;
    }

    public static int HumanSort(Noise a, Noise b)
    {
        int result = 0;
        if (a == null && b == null)
        {
            result = 0;
        }
        else if (a == null)
        {
            result = 1;
        }
        else if (b == null)
        {
            result = -1;
        }
        else
        {
            // intelligent sort (humans)
            result = a.m_identifier.CompareTo(b.m_identifier);
            if (result == 0)
            {
                result = a.m_timeCreated.CompareTo(b.m_timeCreated);
                if (result == 0)
                {
                    result = a.m_volume.CompareTo(b.m_volume);
                }
            }
        }
        return result;
    }

    public static int TapSort(Noise a, Noise b)
    {
        int result = 0;
        if (a == null && b == null)
        {
            result = 0;
        }
        else if (a == null)
        {
            result = 1;
        }
        else if (b == null)
        {
            result = -1;
        }
        else
        {
            // tap sort
            result = a.m_timeCreated.CompareTo(b.m_timeCreated);
            if (result == 0)
            {
                result = a.m_volume.CompareTo(b.m_volume);
            }
        }
        return result;
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

    public float m_maxVolume = 100f;

    [System.Serializable]
    public class NoiseList
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

        //public Noise GetMostAudibleNoise(Vector3 audiencePosition, bool usingNoisePriority = false)
        //{
        //    Noise mostAudible = null;
        //    float mostAudibleNoiseLevel = 0f;

        //    foreach (Noise noise in m_noises)
        //    {
        //        float noiseLevel = noise.CalculateAudabilityFromPosition(audiencePosition);

        //        //if (mostAudible == null || mostAudibleNoiseLevel < noiseLevel)
        //        //{
        //        //    mostAudible = noise;
        //        //    mostAudibleNoiseLevel = noiseLevel;
        //        //}
        //        if (mostAudibleNoiseLevel < noiseLevel)
        //        {
        //            if (mostAudible != null && usingNoisePriority)
        //            {
        //                // find audible noise of the highest priority
        //                if (noise.Priority() >= mostAudible.Priority())
        //                {
        //                    mostAudible = noise;
        //                    mostAudibleNoiseLevel = noiseLevel;
        //                }
        //            }
        //            else
        //            {
        //                // find most audible noise
        //                mostAudible = noise;
        //                mostAudibleNoiseLevel = noiseLevel;
        //            }
        //        }
        //    }

        //    return mostAudible;
        //}

        public void Update(float deltaTime)
        {
            List<Noise> expiredNoises = new List<Noise>();

            foreach (Noise noise in m_noises)
            {
                //noise.Update(deltaTime);
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

        public void Clear()
        {
            m_noises.Clear();
        }
    }

    public NoiseList m_noiseLibrary = new NoiseList();
    public NoiseList m_userTapLibrary = new NoiseList();

    // Update is called once per frame
    void Update()
    {
        m_noiseLibrary.Update(Time.deltaTime);
        m_userTapLibrary.Update(Time.deltaTime);
    }

    //public Noise GetMostAudibleNoise(Vector3 position, bool usingNoisePriority = false)
    //{
    //    return m_noiseLibrary.GetMostAudibleNoise(position, usingNoisePriority);
    //}
    public void GetAudibleNoisesAtLocation(List<Noise> audibleNoises, Vector3 audiencePosition, float hearingSensitivity = 0f, float createdAfter = 0f)
    {
        foreach (Noise noise in m_noiseLibrary.m_noises)
        {
            if (noise.m_timeCreated >= createdAfter)
            {
                if (noise.GetVolumeIfAudible(audiencePosition, hearingSensitivity) > 0)
                {
                    audibleNoises.Add(noise);
                }
            }
        }
    }

    public void GetUserTapsAtLocation(List<Noise> audibleNoises, Vector3 audiencePosition, float hearingSensitivity = 0f, float createdAfter = 0f)
    {
        foreach (Noise noise in m_userTapLibrary.m_noises)
        {
            if (noise.m_timeCreated >= createdAfter)
            {
                if (noise.GetVolumeIfAudible(audiencePosition, hearingSensitivity) > 0)
                {
                    audibleNoises.Add(noise);
                }
            }
        }
    }

    public Noise Add(Vector3 position, float pVolume, float expirationDelay, NoiseIdentifier identifier = NoiseIdentifier.Silent)
    {
        float volume = Mathf.Min(pVolume, m_maxVolume);

        if (identifier == NoiseIdentifier.UserTap)
        {
            return m_userTapLibrary.Add(position, volume, expirationDelay, identifier);
        }
        else
        {
            return m_noiseLibrary.Add(position, volume, expirationDelay, identifier);
        }
        
    }

    public void Remove(Noise noise)
    {
        if (!m_noiseLibrary.IsEmpty())
        {
            m_noiseLibrary.Remove(noise);
        }
    }

}
