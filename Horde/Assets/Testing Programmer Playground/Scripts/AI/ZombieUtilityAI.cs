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
    }


    [System.Serializable]
    public class BehaviourEvaluation
    {
        public float m_currentEvaluationForDebugging = 0f;
        public ZombieUtilityBehaviours.BehaviourNames m_behaviourName;
        public List<WeightedEvaluation> m_evaluations;
    }
    public List<BehaviourEvaluation> m_behaviourEvaluations;

    private ZombieUtilityBehaviours m_zombieBehaviourScript = null;
    private ZombieUtilityEvaluations m_zombieEvaluationScript = null;
    private ZombieBrain m_zombieBrainScript = null;

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

        m_zombieBrainScript = GetComponent<ZombieBrain>();
        if (m_zombieBrainScript == null)
        {
            Debug.Log("ZombieBrain no included");
        }

    }

    // Use this for initialization
    //void Start () {	}

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
                    evaluation = m_zombieEvaluationScript.Evaluation(behaviourEvaluation.m_evaluations[i].m_eval);
                    evaluation *= behaviourEvaluation.m_evaluations[i].m_weight;
                    if (i == 0)
                    {
                        behaviourEvaluationValue = evaluation;
                    }
                    else
                    {
                        behaviourEvaluationValue += evaluation;
                    }
                }

                if (behaviourEvaluationValue > highestEvaluation)
                {
                    highestEvaluation = behaviourEvaluationValue;
                    behaviourWithHighestEvaluation = behaviourEvaluation.m_behaviourName;
                }
                behaviourEvaluation.m_currentEvaluationForDebugging = behaviourEvaluationValue;
            }

            // update current behaviour
            //m_currentBehaviour = behaviourWithHighestEvaluation;
            ChangeBehaviour(behaviourWithHighestEvaluation);
            m_zombieBehaviourScript.RunBehaviour(m_currentBehaviour);
        }

    }

    public ZombieUtilityBehaviours.BehaviourNames GetCurrentBehaviour()
    {
        return m_currentBehaviour;
    }

    //[System.Serializable]
    //public class SoundsStrings
    //{
    //    public string Idle = "";
    //    public string Wander = "";
    //    public string Investigate = "";
    //    public string Devour = "";
    //    public string Chase = "";
    //    public string GoToUserTap = "";
    //    public string Death = "";
    //    public string Reanimating = "";

    //}
    //public SoundsStrings m_sounds;

    [System.Serializable]
    public class SoundsStrings
    {
        //public string Idle ="";
        public string Wander = "ZombieWander";
        //public string Investigate = "";
        public string Devour = "ZombieDevour";
        //public string Chase = "";
        public string GoToUserTap = "ZombieHearsUserTap";
        public string Death = "ZombieDeath";
        public string Reanimating = "ZombieReanimating";

    }
    public SoundsStrings m_sounds;
    public bool m_soundsEnabled = true;
    void ChangeBehaviour(ZombieUtilityBehaviours.BehaviourNames behaviour)
    {
        //AudioSource test = null;

        if(m_currentBehaviour != behaviour)
        {
            // exit
            //switch (m_currentBehaviour)
            //{
            //    case ZombieUtilityBehaviours.BehaviourNames.Idle:
            //        break;
            //    case ZombieUtilityBehaviours.BehaviourNames.Wander:
            //        break;
            //    case ZombieUtilityBehaviours.BehaviourNames.Investigate:
            //        break;
            //    case ZombieUtilityBehaviours.BehaviourNames.Devour:
            //        break;
            //    case ZombieUtilityBehaviours.BehaviourNames.Chase:
            //        break;
            //    case ZombieUtilityBehaviours.BehaviourNames.GoToUserTap:
            //        break;
            //    case ZombieUtilityBehaviours.BehaviourNames.Death:
            //        break;
            //    case ZombieUtilityBehaviours.BehaviourNames.Reanimating:
            //        break;
            //    default:
            //        break;
            //}

            m_currentBehaviour = behaviour;

            //enter
            //switch (m_currentBehaviour)
            //{
            //    case ZombieUtilityBehaviours.BehaviourNames.Idle:
            //        break;
            //    case ZombieUtilityBehaviours.BehaviourNames.Wander:
            //        break;
            //    case ZombieUtilityBehaviours.BehaviourNames.Investigate:
            //        break;
            //    case ZombieUtilityBehaviours.BehaviourNames.Devour:
            //        break;
            //    case ZombieUtilityBehaviours.BehaviourNames.Chase:
            //        break;
            //    case ZombieUtilityBehaviours.BehaviourNames.GoToUserTap:
            //        break;
            //    case ZombieUtilityBehaviours.BehaviourNames.Death:
            //        break;
            //    case ZombieUtilityBehaviours.BehaviourNames.Reanimating:
            //        break;
            //    default:
            //        break;
            //}

            if (m_soundsEnabled)
            {
                switch (m_currentBehaviour)
                {
                    //case ZombieUtilityBehaviours.BehaviourNames.Idle:
                    //    break;
                    case ZombieUtilityBehaviours.BehaviourNames.Wander:
                        SoundLibrary.PlaySound(gameObject, m_sounds.Wander);
                        break;
                    //case ZombieUtilityBehaviours.BehaviourNames.Investigate:
                    //    break;
                    case ZombieUtilityBehaviours.BehaviourNames.Devour:
                        SoundLibrary.PlaySound(gameObject, m_sounds.Devour);
                        break;
                    //case ZombieUtilityBehaviours.BehaviourNames.Chase:
                    //    break;
                    case ZombieUtilityBehaviours.BehaviourNames.GoToUserTap:
                        SoundLibrary.PlaySound(gameObject, m_sounds.GoToUserTap);
                        break;
                    case ZombieUtilityBehaviours.BehaviourNames.Death:
                        SoundLibrary.PlaySound(gameObject, m_sounds.Death);
                        break;
                    case ZombieUtilityBehaviours.BehaviourNames.Reanimating:
                        SoundLibrary.PlaySound(gameObject, m_sounds.Reanimating);
                        break;
                    default:
                        break;
                }
            }
        }


    }

}
