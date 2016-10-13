using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Act

public class HumanBehaviours : BehaviourModule
{

    public enum BehaviourNames
    {
        Idle, //1
        Wander, //1
        Patrol, //5
        Guard, //5
        Investigate, //3
        MoveToEnemy, //2
        AttackIfInRange, //2
        Flee, //4
        SeekCover, //1
        Death, //1
    }

    [System.Serializable]
    public class EvaluationWeight
    {
        public HumanEvaluations.EvaluationNames m_name;
        public float m_weight = 0f;
    }

    [System.Serializable]
    public class BehaviourEvaluation
    {
        public float m_currentEvaluation = 0f; // For Debugging
        public BehaviourNames m_behaviourName;
        public List<EvaluationWeight> m_evaluations;
    }

    public BehaviourNames m_currentBehaviour = BehaviourNames.Idle;
    public List<BehaviourEvaluation> m_behaviorEvaluations;

    private HumanEvaluations m_humanEvaluationsScript = null;

    void Awake()
    {
        m_humanEvaluationsScript = GetComponent<HumanEvaluations>();
        if (m_humanEvaluationsScript == null)
        {
            Debug.Log("HumanEvaluations no included");
        }
    }

    // Use this for initialization
    void Start ()
    {
        CreateBehaviours();
	}

    private void CreateBehaviours()
    {
        BaseBehaviour idle = new Idle(this.gameObject);

        AddBehaviour((int)BehaviourNames.Idle, idle);
    }

    // Update is called once per frame
    void Update()
    {
        EvaluateBehaviours();
    }

    public void EvaluateBehaviours()
    {
        if (m_humanEvaluationsScript)
        {
            // Initialise Highest Behaviour
            float highestEvaluation = 0f;
            BehaviourNames highestEvaluatedBehaviour = BehaviourNames.Idle;

            // For Each Behaviour
                // Evaluate Behaviour
                // If Behaviour if the Highest Rated make it the current Behavior

            foreach (BehaviourEvaluation behaviourEvaluation in m_behaviorEvaluations)
            {

                float evaluationOfBehaviour = 0f;
                for (int i = 0; i < behaviourEvaluation.m_evaluations.Count; i++)
                {
                    float evaluation = 0f;
                    evaluation = m_humanEvaluationsScript.RunEvaluation(behaviourEvaluation.m_evaluations[i].m_name);
                    evaluation *= behaviourEvaluation.m_evaluations[i].m_weight;
                    if (i == 0)
                    {
                        evaluationOfBehaviour = evaluation;
                    }
                    else
                    {
                        evaluationOfBehaviour += evaluation;
                    }
                }

                behaviourEvaluation.m_currentEvaluation = evaluationOfBehaviour; // For debugging

                if (evaluationOfBehaviour > highestEvaluation)
                {
                    highestEvaluation = evaluationOfBehaviour;
                    highestEvaluatedBehaviour = behaviourEvaluation.m_behaviourName;
                }
            }

            // Run Highest Evaluated Behaviour
            m_currentBehaviour = highestEvaluatedBehaviour;
            RunBehaviour(m_currentBehaviour);
        }
    }

    public void RunBehaviour(BehaviourNames pBehaviourName)
    {
        RunBehaviour((int)pBehaviourName);
    }

    public BehaviourNames GetCurrentBehaviour()
    {
        return m_currentBehaviour;
    }
}
