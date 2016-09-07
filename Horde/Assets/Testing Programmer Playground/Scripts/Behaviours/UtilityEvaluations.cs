using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UtilityEvaluations : MonoBehaviour {

    ExampleUtilityAI.Evaluations test;

    public Dictionary<Evaluations, UtilityMath.UtilityValue> m_test = new Dictionary<Evaluations, UtilityMath.UtilityValue>();

    public enum Evaluations
    {
        Health,
    }



    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public float Evaluation(ExampleUtilityAI.Evaluations evaluation)
    {
        float result = 0f;

        switch (evaluation)
        {
            case ExampleUtilityAI.Evaluations.eval1:
                // Code Value
                //result = Evaluation1();
                break;
            case ExampleUtilityAI.Evaluations.eval2:
                break;
            case ExampleUtilityAI.Evaluations.eval3:
                break;
            default:
                break;
        }


        return result;
    }

    //private float Evaluation1()
    //{
    //    return 0;
    //}
}

