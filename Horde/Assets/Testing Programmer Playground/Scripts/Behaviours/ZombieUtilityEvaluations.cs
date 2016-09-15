using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieUtilityEvaluations : MonoBehaviour {

    public Dictionary<Evaluations, UtilityMath.UtilityValue> m_evaluations = new Dictionary<Evaluations, UtilityMath.UtilityValue>();

    public enum Evaluations
    {
        Health,
        Damage,
        EnemyInSight,
    }

    private Health m_healthScript = null;
    public UtilityMath.UtilityValue m_healthFormula;
    public UtilityMath.UtilityValue m_damageFormula;


    private ZombieBrain m_zombieBrainScript = null;
    public UtilityMath.UtilityValue m_enemyInSightFormula;
    public UtilityMath.UtilityValue m_corspeInSightFormula;
    public UtilityMath.UtilityValue m_boredomFormula;
    public UtilityMath.UtilityValue m_canHearUserTapFormula;
    public UtilityMath.UtilityValue m_canHearNoiseFormula;
    public UtilityMath.UtilityValue m_interestInUsertapFormula;


    void Awake()
    {
        m_healthScript = GetComponent<Health>();
        if (m_healthScript == null)
        {
            Debug.Log("Health no included");
        }
        m_zombieBrainScript = GetComponent<ZombieBrain>();
        if (m_zombieBrainScript == null)
        {
            Debug.Log("ZombieBrain no included");
        }
    }

    // Use this for initialization
    void Start()
    {
        if (m_healthScript)
        {
            //m_healthFormula = new UtilityMath.UtilityValue(UtilityMath.UtilityValue.NormalisationFormula.Exponential, 0, m_healthScript.m_maxHealth);
            m_healthFormula.SetMinMaxValues(0, m_healthScript.m_maxHealth);
            m_damageFormula.SetMinMaxValues(0, m_healthScript.m_maxHealth);
            m_damageFormula.SetNormalisationType(UtilityMath.UtilityValue.NormalisationFormula.InverseLinear);
        }

        m_evaluations.Add(Evaluations.Health, m_healthFormula);
        m_evaluations.Add(Evaluations.Damage, m_damageFormula);

        if (m_zombieBrainScript)
        {
            m_enemyInSightFormula.SetMinMaxValues(0, 1);
            m_corspeInSightFormula.SetMinMaxValues(0, 1);
            m_boredomFormula.SetMinMaxValues(0, m_zombieBrainScript.m_maxBoredom);
            m_canHearUserTapFormula.SetMinMaxValues(0, 1);
            m_canHearNoiseFormula.SetMinMaxValues(0, 1);
            m_interestInUsertapFormula.SetMinMaxValues(0, m_zombieBrainScript.m_tapInterest);
        }

        
    }


    void Update()
    {
        // might want to put these checks on a delay as the do not need to be checked every frame
        CheckHealth();
        if (m_zombieBrainScript)
        {
            m_enemyInSightFormula.SetValue(m_zombieBrainScript.GetEnemiesInSightCount());
            m_enemyInSightFormula.SetValue(m_zombieBrainScript.GetCorpsesInSightCount());
            m_boredomFormula.SetValue(m_zombieBrainScript.m_currentBoredom);
            m_canHearUserTapFormula.SetValue(m_zombieBrainScript.GetUserTapCount());
            m_canHearNoiseFormula.SetValue(m_zombieBrainScript.GetAudibleNoiseCount());

            Noise lastTap = m_zombieBrainScript.GetLastUserTap();
            float interest = 0f;
            if (lastTap != null)
            {
                if (Time.time < lastTap.m_timeCreated + m_zombieBrainScript.m_tapInterest)
                {
                    interest = m_zombieBrainScript.m_tapInterest - (Time.time - lastTap.m_timeCreated);
                }
            }
            m_interestInUsertapFormula.SetValue(interest);
        }
    }


    public float Evaluation(Evaluations pEvaluation)
    {
        float result = 0f;

        UtilityMath.UtilityValue evaluator = null;
        if (m_evaluations.TryGetValue(pEvaluation, out evaluator))
        {
            // success
            result = evaluator.Evaluate();
        }
        else
        {
            // fail
            Debug.Log("Unable to Locate Evalutaion: " + pEvaluation.ToString());
        }

        return result;
    }

    private void CheckHealth()
    {
        if (m_healthScript)
        {
            m_healthFormula.SetValue(m_healthScript.m_health);
            m_damageFormula.SetValue(m_healthScript.m_health);
        }
    }


}
