using UnityEngine;
using System.Collections;

public class UtilityEvaluations : MonoBehaviour {

    UtilityAI.Evaluations test;





	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public float Evaluation(UtilityAI.Evaluations evaluation)
    {
        float result = 0f;

        switch (evaluation)
        {
            case UtilityAI.Evaluations.eval1:
                // Code Value
                //result = Evaluation1();
                break;
            case UtilityAI.Evaluations.eval2:
                break;
            case UtilityAI.Evaluations.eval3:
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

