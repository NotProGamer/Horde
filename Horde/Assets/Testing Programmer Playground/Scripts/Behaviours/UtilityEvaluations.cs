using UnityEngine;
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

    // Use this for initialization
    void Start ()
    {
        m_test.Add(Evaluations.Health, m_healthFormula);
    }
	
	// Update is called once per frame
	void Update () {
	
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

    private float Evaluation1()
    {
        return 0;
    }
}

