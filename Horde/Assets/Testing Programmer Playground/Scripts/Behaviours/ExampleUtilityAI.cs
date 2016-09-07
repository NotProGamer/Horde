using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExampleUtilityAI : MonoBehaviour {

    public enum Evaluations
    {
        eval1,
        eval2,
        eval3
    }

    public enum Behaviours
    {
        behaviour1,
        behaviour2
    }

    public enum ROperator
    {
        None,
        Addition,
        Multiplication,
    }

    [System.Serializable]
    public class Eval
    {
        public Evaluations m_eval;
        public ROperator m_operator = ROperator.None;
    }

    [System.Serializable]
    public class BehaveEval
    {
        public List<Eval> m_evaluations;
        public Behaviours m_behaviour;
    }

    public List<BehaveEval> m_behaviours;


    public float Evaluation(Evaluations evaluation)
    {
        float result = 0f;

        switch (evaluation)
        {
            case Evaluations.eval1:
                // Code Value
                result = Evaluation1();
                break;
            case Evaluations.eval2:
                break;
            case Evaluations.eval3:
                break;
            default:
                break;
        }


        return result;
    }

    private float Evaluation1()
    {
        return 0;
    }


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
