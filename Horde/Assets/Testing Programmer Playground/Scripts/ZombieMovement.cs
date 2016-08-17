﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieMovement : MonoBehaviour {

    private NavMeshAgent m_nav = null;
    private Noise m_mostAudibleNoise = null;
    private NoiseManager m_noiseManager = null;
    private float m_touchRange = 1f; // may need to get this from attackRange or moveRange

    public float m_minSpeed = 1f;
    public float m_maxSpeed = 10f;
    private float m_currentSpeed = 1f;

    public Vector3 m_currentDestination = new Vector3();


    public enum State
    {
        Idle,
        Moving,
    }
    public State m_state = State.Idle;

    void Awake()
    {
        m_nav = GetComponent<NavMeshAgent>();
        if (m_nav == null)
        {
            Debug.Log("NavMeshAgent not included");
        }

        GameObject obj = GameObject.FindGameObjectWithTag(Tags.GameController);
        if (obj)
        {
            m_noiseManager = obj.GetComponent<NoiseManager>();
        }
    }


	// Use this for initialization
	void Start ()
    {
        m_currentSpeed = m_minSpeed;
        m_currentDestination = transform.position;
        m_audibleNoises = new List<Noise>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //SetDestination(GetCurrentTargetPosition());
        Think();
        if (m_nav)
        {
            if (ReachedDestination())
            {
                m_state = State.Idle;
                //m_nav.Stop();
            }
        }
    }


    void SetDestination(Vector3 position)
    {
        if (m_nav)
        {
            m_state = State.Moving;
            m_currentDestination = position;
            m_nav.speed = m_currentSpeed;
            m_nav.SetDestination(m_currentDestination);
            //m_nav.Resume();
        }
    }
    private bool ReachedDestination()
    {
        bool result = false;
        if (m_nav)
        {
            result = m_nav.remainingDistance <= m_touchRange;
        }

        return result;
    }


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
