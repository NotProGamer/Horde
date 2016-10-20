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
        Patrol, //5 requires Decision Target
        Guard, //5 requires Decision Target
        Investigate, //3 requires Decision Target
        MoveToEnemy, //2 requires Decision Target
        AttackIfInRange, //2 requires Decision Target
        Flee, //4 requires Decision Target
        SeekCover, //6 requires Decision Target
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
        BaseBehaviour wander = new Wander(this.gameObject);
        BaseBehaviour death = new Death(this.gameObject);
        BaseBehaviour moveToEnemy = new MoveToEnemy(this.gameObject);
        BaseBehaviour attackEnemy = new AttackEnemy(this.gameObject);

        AddBehaviour((int)BehaviourNames.Idle, idle);
        AddBehaviour((int)BehaviourNames.Wander, wander);
        AddBehaviour((int)BehaviourNames.Death, death);
        AddBehaviour((int)BehaviourNames.MoveToEnemy, moveToEnemy);
        AddBehaviour((int)BehaviourNames.AttackIfInRange, attackEnemy);

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
                for (int eIndex = 0; eIndex < behaviourEvaluation.m_evaluations.Count; eIndex++)
                {
                    float evaluation = 0f;
                    evaluation = m_humanEvaluationsScript.RunEvaluation(behaviourEvaluation.m_evaluations[eIndex].m_name);
                    evaluation *= behaviourEvaluation.m_evaluations[eIndex].m_weight;
                    if (eIndex == 0)
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

            //for (int bIndex = 0; bIndex < m_behaviorEvaluations.Count; bIndex++)
            //{
            //    float evaluationOfBehaviour = 0f;
            //    for (int eIndex = 0; eIndex < m_behaviorEvaluations[bIndex].m_evaluations.Count; eIndex++)
            //    {
            //        float evaluation = 0f;
            //        evaluation = m_humanEvaluationsScript.RunEvaluation(m_behaviorEvaluations[bIndex].m_evaluations[eIndex].m_name);
            //        evaluation *= m_behaviorEvaluations[bIndex].m_evaluations[eIndex].m_weight;
            //        if (eIndex == 0)
            //        {
            //            evaluationOfBehaviour = evaluation;
            //        }
            //        else
            //        {
            //            evaluationOfBehaviour += evaluation;
            //        }
            //    }
            //    m_behaviorEvaluations[bIndex].m_currentEvaluation = evaluationOfBehaviour; // For debugging
            //    if (evaluationOfBehaviour > highestEvaluation)
            //    {
            //        highestEvaluation = evaluationOfBehaviour;
            //        highestEvaluatedBehaviour = m_behaviorEvaluations[bIndex].m_behaviourName;
            //    }
            //}

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
