using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieMovement : Movement
{
    // references .. parenting on top of monobehaviour, http://answers.unity3d.com/questions/362575/inheritance-hides-start.html

    new void Awake()
    {
        base.Awake();
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
        m_rigidbody = GetComponent<Rigidbody>();
        m_nav = GetComponent<NavMeshAgent>();
    }


    // Use this for initialization
    new void Start ()
    {
        base.Start();
        m_audibleNoises = new List<Noise>();
    }

    // Update is called once per frame
    new void Update ()
    {
        //SetDestination(GetCurrentTargetPosition());
        //Think();
        UpdateAnimation();
        base.Update();
    }

    public Animator m_anim = null;
    private Rigidbody m_rigidbody = null;
    private NavMeshAgent m_nav = null;

    void UpdateAnimation()
    {
        if (m_anim != null && m_rigidbody != null && m_nav != null)
        {
            m_anim.SetFloat("Movement", m_nav.velocity.magnitude / m_maxSpeed);
            //m_anim.SetFloat("Movement", m_rigidbody.velocity.magnitude / m_maxSpeed);
        }
        
    }

    // The following code is more to do with behaviour and will later be move out of this script
    private Noise m_mostAudibleNoise = null;
    private NoiseManager m_noiseManager = null;
    private List<Noise> m_audibleNoises = null;
    public Noise m_currentTargetNoise = null;


    void Think()
    {
        List<Noise> audibleNoises = new List<Noise>();
        if (m_noiseManager)
        {
            m_noiseManager.GetAudibleNoisesAtLocation(audibleNoises, transform.position);
        }
        m_audibleNoises = audibleNoises;
        m_currentTargetNoise = SelectTargetNoise();
        if (m_currentTargetNoise != null)
        {
            m_currentSpeed = DetermineCurrentSpeed(); // this should be based on target/ behaviour
            SetDestination(m_currentTargetNoise.m_position);
        }
    }

    Noise SelectTargetNoise()
    {
        Noise targetNoise = null;

        foreach (Noise noise in m_audibleNoises)
        {
            if (targetNoise == null)
            {
                targetNoise = noise;
            }
            else
            {
                // if noise if of higher priority then chase it
                if ((noise.m_identifier > targetNoise.m_identifier) ||
                    (noise.m_identifier == targetNoise.m_identifier && noise.m_volume > targetNoise.m_volume))
                {
                    targetNoise = noise;
                }
            }
        }

        return targetNoise;
    }

    public void SetCurrentNoiseTarget(Noise noise)
    {
        // This should be a much smarter check system.
        if (noise.m_identifier == NoiseIdentifier.UserTap)
        {
            m_currentTargetNoise = noise;
        }
    }

    private float DetermineCurrentSpeed()
    {
        float result = m_minSpeed;
        if (m_currentTargetNoise != null)
        {
            result = m_maxSpeed;
        }
        return result;
    }


    //float m_test = 0f;
    //float m_delay = 5f;


    //private Vector3 GetCurrentTargetPosition()
    //{

    //    Vector3 currentTarget = transform.position;
    //    if (m_mostAudibleNoise != null)
    //    {
    //        // hunt
    //        currentTarget = m_mostAudibleNoise.m_position;
    //        //Debug.Log("hunt to: " + currentTarget);
    //    }
    //    else
    //    {
    //        //// wander
    //        //if (Time.time > m_test)
    //        //{
    //        //    m_test = Time.time + m_delay;

    //        //    Vector2 test2 = Random.insideUnitCircle * 10f;
    //        //    Vector3 randomTarget = transform.position + new Vector3(test2.x, 0, test2.y);
    //        //    //Debug.Log(randomTarget);
    //        //    NavMeshHit hit;
    //        //    if (NavMesh.SamplePosition(randomTarget, out hit, 100f, NavMesh.AllAreas))
    //        //    {
    //        //        currentTarget = hit.position;
    //        //        currentTarget.y = 1.0f;
    //        //    }
    //        //    else
    //        //    {
    //        //        currentTarget = transform.position;
    //        //    }
    //        //    Debug.Log("wander to: " + currentTarget);
    //        //}
    //    }
    //    return currentTarget;
    //}



    //private float CalculateSpeed(float volumeMultiplier)
    //{
    //    float result = m_minSpeed;
    //    result += volumeMultiplier;
    //    result = Mathf.Clamp(result, m_minSpeed, m_maxSpeed);
    //    return result;
    //}

}

