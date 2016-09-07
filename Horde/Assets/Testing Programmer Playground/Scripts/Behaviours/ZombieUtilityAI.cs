using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieUtilityAI : MonoBehaviour {

    public enum BehaviourEvalutationOperator
    {
        None,
        Addition,
        Multiplication,
    }

    [System.Serializable]
    public class WeightedEvaluation
    {
        public UtilityEvaluations.Evaluations m_eval;
        public float m_weight = 1f;
        public BehaviourEvalutationOperator m_evaluationCombination = BehaviourEvalutationOperator.None;
    }


    [System.Serializable]
    public class BehaviourEvaluation
    {
        public List<WeightedEvaluation> m_evaluations;
        public UtilityBehaviours.BehaviourNames m_behaviour;
    }
    public List<BehaviourEvaluation> m_behaviourEvaluations;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
