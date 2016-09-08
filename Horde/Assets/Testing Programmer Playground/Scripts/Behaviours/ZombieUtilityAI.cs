using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieUtilityAI : MonoBehaviour {

    public ZombieUtilityBehaviours.BehaviourNames m_currentBehaviour = ZombieUtilityBehaviours.BehaviourNames.Idle;

    [System.Serializable]
    public class WeightedEvaluation
    {
        public ZombieUtilityEvaluations.Evaluations m_eval;
        public float m_weight = 1f;
        //public float Evaluation()
        //{
        //    float eval = 0f;

        //    return eval;

        //}
    }


    [System.Serializable]
    public class BehaviourEvaluation
    {
        public float m_currentEvaluationForDebugging = 0f;
        public List<WeightedEvaluation> m_evaluations;
        public ZombieUtilityBehaviours.BehaviourNames m_behaviourName;
        //public void Evaluate()
        //{
        //    for(int i = 0; i < m_evaluations.Count; i++)
        //    {
        //        if (i == 0)
        //        {
        //            //m_evaluation = m_evaluations[0].m_eval * m_evaluations[0].m_weight;
        //        }
        //    }
        //}
    }
    public List<BehaviourEvaluation> m_behaviourEvaluations;

    private ZombieUtilityBehaviours m_zombieBehaviourScript = null;
    private ZombieUtilityEvaluations m_zombieEvaluationScript = null;

    void Awake()
    {
        m_zombieBehaviourScript = GetComponent<ZombieUtilityBehaviours>();
        if (m_zombieBehaviourScript == null)
        {
            Debug.Log("ZombieUtilityBehaviours no included");
        }

        m_zombieEvaluationScript = GetComponent<ZombieUtilityEvaluations>();
        if (m_zombieEvaluationScript == null)
        {
            Debug.Log("ZombieUtilityEvaluations no included");
        }

        
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Could delay this if i wanted
        Evaluation();

    }

    private void Evaluation()
    {
        // this evaluates the behaviours and calls the best one


        if (m_zombieBehaviourScript && m_zombieEvaluationScript)
        {

            //initialise starting evaluation;
            float highestEvaluation = 0f;
            ZombieUtilityBehaviours.BehaviourNames behaviourWithHighestEvaluation = ZombieUtilityBehaviours.BehaviourNames.Idle;

            foreach (BehaviourEvaluation behaviourEvaluation in m_behaviourEvaluations)
            {
                float behaviourEvaluationValue = 0f;
                for (int i = 0; i < behaviourEvaluation.m_evaluations.Count; i++)
                {
                    float evaluation = 0f;
                    evaluation = m_zombieEvaluationScript.Evaluation(behaviourEvaluation.m_evaluations[0].m_eval);
                    evaluation *= behaviourEvaluation.m_evaluations[0].m_weight;
                    if (i == 0)
                    {
                        behaviourEvaluationValue = evaluation;
                    }
                    else
                    {
                        behaviourEvaluationValue *= evaluation;
                    }
                }

                if (behaviourEvaluationValue > highestEvaluation)
                {
                    highestEvaluation = behaviourEvaluationValue;
                    behaviourWithHighestEvaluation = behaviourEvaluation.m_behaviourName;
                }
            }

            // update current behaviour
            m_currentBehaviour = behaviourWithHighestEvaluation;
            m_zombieBehaviourScript.RunBehaviour(m_currentBehaviour);
        }

    }
}
