using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieUtilityEvaluations : MonoBehaviour {

    public Dictionary<Evaluations, UtilityMath.UtilityValue> m_evaluations = new Dictionary<Evaluations, UtilityMath.UtilityValue>();

    public enum Evaluations
    {
        Health,
        Damage,
        isAlive,
        isDead,
        EnemyInSight,
        NoEnemyInSight,
        CorpseInSight,
        NoCorpseInSight,
        Boredom,
        InverseBoredom,
        CanHearUserTap,
        CanNotHearUserTap,
        CanHearNoise,
        InterestInUserTap,
        DistanceToEnemy,
        DistanceToCorpse,
        DistanceToPriorityNoise,
        DistanceToLastUserTap,
    }

    private Health m_healthScript = null;
    public UtilityMath.UtilityValue m_healthFormula; // Done
    public UtilityMath.UtilityValue m_damageFormula; // Done
    public UtilityMath.UtilityValue m_isAliveFormula; // Done
    public UtilityMath.UtilityValue m_isDeadFormula; // Done


    private ZombieBrain m_zombieBrainScript = null;
    private GameObject m_gameController = null;
    private NoiseManager m_noiseManagerScript = null;
    public UtilityMath.UtilityValue m_enemyInSightFormula; // Done
    public UtilityMath.UtilityValue m_noEnemyInSightFormula; // 
    public UtilityMath.UtilityValue m_corspeInSightFormula; // Done
    public UtilityMath.UtilityValue m_noCorspeInSightFormula; // 
    public UtilityMath.UtilityValue m_boredomFormula; // Done
    public UtilityMath.UtilityValue m_inverseBoredomFormula; // Done
    public UtilityMath.UtilityValue m_canHearUserTapFormula; // Done
    public UtilityMath.UtilityValue m_canNotHearUserTapFormula; // Done
    public UtilityMath.UtilityValue m_canHearNoiseFormula; // Done
    public UtilityMath.UtilityValue m_interestInUsertapFormula; // Done
    public UtilityMath.UtilityValue m_distanceToEnemyFormula; // Done
    public UtilityMath.UtilityValue m_distanceToCorpseFormula; // Done
    public UtilityMath.UtilityValue m_distanceToPriorityNoiseFormula; // Done
    public UtilityMath.UtilityValue m_distanceToLastUserTapFormula; // Done .. probably not relevant

    //public UtilityMath.UtilityValue m_distanceToUserTapFormula;

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

        m_gameController = GameObject.FindGameObjectWithTag(Labels.Tags.GameController);

        if (m_gameController != null)
        {
            m_noiseManagerScript = m_gameController.GetComponent<NoiseManager>();
            if (m_noiseManagerScript == null)
            {
                Debug.Log("NoiseManager not included");
            }
        }
        else
        {
            Debug.Log("GameController no included");
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

            m_isDeadFormula.SetMinMaxValues(System.Convert.ToInt32(false), System.Convert.ToInt32(true)); // 0, 1
            m_isAliveFormula.SetMinMaxValues(System.Convert.ToInt32(false), System.Convert.ToInt32(true)); // 0, 1
            m_isAliveFormula.SetNormalisationType(UtilityMath.UtilityValue.NormalisationFormula.InverseLinear);
        }

        m_evaluations.Add(Evaluations.Health, m_healthFormula);
        m_evaluations.Add(Evaluations.Damage, m_damageFormula);
        m_evaluations.Add(Evaluations.isDead, m_isDeadFormula);
        m_evaluations.Add(Evaluations.isAlive, m_isAliveFormula);

        if (m_zombieBrainScript)
        {
            m_enemyInSightFormula.SetMinMaxValues(0, 1);
            m_noEnemyInSightFormula.SetMinMaxValues(0, 1);
            m_noEnemyInSightFormula.SetNormalisationType(UtilityMath.UtilityValue.NormalisationFormula.InverseLinear);
            m_corspeInSightFormula.SetMinMaxValues(0, 1);
            m_noCorspeInSightFormula.SetMinMaxValues(0, 1);
            m_noCorspeInSightFormula.SetNormalisationType(UtilityMath.UtilityValue.NormalisationFormula.InverseLinear);

            m_boredomFormula.SetMinMaxValues(0, m_zombieBrainScript.m_maxBoredom);
            m_inverseBoredomFormula.SetMinMaxValues(0, m_zombieBrainScript.m_maxBoredom);
            m_inverseBoredomFormula.SetNormalisationType(UtilityMath.UtilityValue.NormalisationFormula.InverseLinear);
            m_canHearUserTapFormula.SetMinMaxValues(0, 1);
            m_canNotHearUserTapFormula.SetMinMaxValues(0, 1);
            m_canNotHearUserTapFormula.SetNormalisationType(UtilityMath.UtilityValue.NormalisationFormula.InverseLinear);
            m_canHearNoiseFormula.SetMinMaxValues(0, 1);
            m_interestInUsertapFormula.SetMinMaxValues(0, m_zombieBrainScript.m_tapInterest);
            // distances
            m_distanceToEnemyFormula.SetMinMaxValues(0, m_zombieBrainScript.m_sightRange * m_zombieBrainScript.m_sightRange);
            m_distanceToCorpseFormula.SetMinMaxValues(0, m_zombieBrainScript.m_sightRange * m_zombieBrainScript.m_sightRange);

            float maxHearingRange = float.MaxValue;
            if (m_noiseManagerScript != null)
            {
                maxHearingRange = (m_noiseManagerScript.m_maxVolume * m_noiseManagerScript.m_maxVolume) + (m_zombieBrainScript.m_hearingRange * m_zombieBrainScript.m_hearingRange);
            }
            m_distanceToPriorityNoiseFormula.SetMinMaxValues(0, maxHearingRange);
            m_distanceToLastUserTapFormula.SetMinMaxValues(0, maxHearingRange);
        }

        m_evaluations.Add(Evaluations.EnemyInSight, m_enemyInSightFormula);
        m_evaluations.Add(Evaluations.NoEnemyInSight, m_noEnemyInSightFormula);
        m_evaluations.Add(Evaluations.CorpseInSight, m_corspeInSightFormula);
        m_evaluations.Add(Evaluations.NoCorpseInSight, m_noCorspeInSightFormula);
        m_evaluations.Add(Evaluations.Boredom, m_boredomFormula);
        m_evaluations.Add(Evaluations.InverseBoredom, m_inverseBoredomFormula);
        m_evaluations.Add(Evaluations.CanHearUserTap, m_canHearUserTapFormula);
        m_evaluations.Add(Evaluations.CanNotHearUserTap, m_canNotHearUserTapFormula);
        m_evaluations.Add(Evaluations.CanHearNoise, m_canHearNoiseFormula);
        m_evaluations.Add(Evaluations.InterestInUserTap, m_interestInUsertapFormula);
        // distances
        m_evaluations.Add(Evaluations.DistanceToEnemy, m_distanceToEnemyFormula);
        m_evaluations.Add(Evaluations.DistanceToCorpse, m_distanceToCorpseFormula);
        m_evaluations.Add(Evaluations.DistanceToPriorityNoise, m_distanceToPriorityNoiseFormula);
        m_evaluations.Add(Evaluations.DistanceToLastUserTap, m_distanceToLastUserTapFormula);
    }


    void Update()
    {
        // might want to put these checks on a delay as the do not need to be checked every frame
        CheckHealth();
        if (m_zombieBrainScript)
        {
            m_enemyInSightFormula.SetValue(m_zombieBrainScript.GetEnemiesInSightCount());
            m_noEnemyInSightFormula.SetValue(m_zombieBrainScript.GetEnemiesInSightCount());
            m_corspeInSightFormula.SetValue(m_zombieBrainScript.GetCorpsesInSightCount());
            m_noCorspeInSightFormula.SetValue(m_zombieBrainScript.GetCorpsesInSightCount());
            m_boredomFormula.SetValue(m_zombieBrainScript.m_currentBoredom);
            m_inverseBoredomFormula.SetValue(m_zombieBrainScript.m_currentBoredom);
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

            // get distance to Enemy;
            m_distanceToEnemyFormula.SetValue(DistanceToMemoryLocation(Labels.Memory.ClosestEnemy, m_zombieBrainScript.m_sightRange * m_zombieBrainScript.m_sightRange));

            // get distance to Corpse
            m_distanceToCorpseFormula.SetValue(DistanceToMemoryLocation(Labels.Memory.ClosestCorpse, m_zombieBrainScript.m_sightRange * m_zombieBrainScript.m_sightRange));

            // get distance to Noise
            m_distanceToPriorityNoiseFormula.SetValue(DistanceToMemoryLocation(Labels.Memory.LastPriorityNoise)); 

            // get distance to UserTap
            m_distanceToLastUserTapFormula.SetValue(DistanceToMemoryLocation(Labels.Memory.LastUserTap)); 
        }
    }

    private float DistanceToMemoryLocation(string MemoryLabel, float maximumRangeSqrd = float.MaxValue)
    {
        float distance = maximumRangeSqrd;

        if (m_zombieBrainScript)
        {
            Vector3 position;

            if (m_zombieBrainScript.GetObjectPosition(MemoryLabel, out position))
            {
                distance = (position - transform.position).sqrMagnitude;
            }
        }
        else
        {
            Debug.Log("ZombieBrain not included");
        }

        return distance;
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
            Debug.Log("Unable to Locate Evaluation: " + pEvaluation.ToString());
        }

        return result;
    }

    private void CheckHealth()
    {
        if (m_healthScript)
        {
            m_healthFormula.SetValue(m_healthScript.m_health);
            m_damageFormula.SetValue(m_healthScript.m_health);
            m_isDeadFormula.SetValue(System.Convert.ToInt32(m_healthScript.IsDead()));
            m_isAliveFormula.SetValue(System.Convert.ToInt32(m_healthScript.IsDead()));
        }
    }


}
