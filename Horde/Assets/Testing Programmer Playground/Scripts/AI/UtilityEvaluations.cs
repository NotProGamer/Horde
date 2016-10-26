﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UtilityEvaluations : MonoBehaviour {

    public Dictionary<Evaluations, UtilityMath.UtilityValue> m_test = new Dictionary<Evaluations, UtilityMath.UtilityValue>();

    public enum Evaluations
    {
        Health,
        EnemyCount
    }

    public UtilityMath.UtilityValue m_healthFormula;
    public UtilityMath.UtilityValue m_enemyCountFormula;

    //private float Evaluation1()
    //{
    //    return 0;
    //}

    private Health m_healthScript = null;

    void Awake()
    {
        m_healthScript = GetComponent<Health>();
        if (m_healthScript == null)
        {
            Debug.Log("Health no included");
        }
    }

    // Use this for initialization
    void Start()
    {
        if (m_healthScript)
        {
            //m_healthFormula = new UtilityMath.UtilityValue(UtilityMath.UtilityValue.NormalisationFormula.Exponential, 0, m_healthScript.m_maxHealth);
            m_healthFormula.SetMinMaxValues(0, m_healthScript.m_maxHealth);
        }

        m_test.Add(Evaluations.Health, m_healthFormula);
    }

    // Update is called once per frame
    void Update()
    {
        // might want to put these checks on a delay as the do not need to be checked every frame
        CheckHealth();
    }


    public float Evaluation(Evaluations pEvaluation)
    {
        float result = 0f;

        UtilityMath.UtilityValue evaluator = null;
        if (m_test.TryGetValue(pEvaluation, out evaluator))
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
        }
   }




















}
