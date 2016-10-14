using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EvaluationModule : MonoBehaviour {

    private Dictionary<int, UtilityMath.UtilityValue> m_evaluations = new Dictionary<int, UtilityMath.UtilityValue>();
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected float RunEvaluation(int evaluationIndex)
    {
        float result = 0f;

        UtilityMath.UtilityValue evalutaionToRun = null;

        if (m_evaluations.TryGetValue(evaluationIndex, out evalutaionToRun))
        {
            result = evalutaionToRun.Evaluate();
        }
        else
        {
            Debug.Log("Unknown Evaluation at index " + evaluationIndex + ".");
        }

        return result;
    }

    public bool AddEvaluation(int evaluationIndex, UtilityMath.UtilityValue evaluation, bool overwrite = false)
    {
        bool result = false;


        if (m_evaluations.ContainsKey(evaluationIndex))
        {
            if (overwrite)
            {
                m_evaluations.Add(evaluationIndex, evaluation);
            }
            else
            {
                Debug.Log("Evaluation Index already in use. Use overwrite switch or choose a different index");
            }
        }
        else
        {
            m_evaluations.Add(evaluationIndex, evaluation);
        }

        return result;
    }
}
