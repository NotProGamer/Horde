using UnityEngine;
using System.Collections;

public class HumanEvaluations : EvaluationModule {

    public enum EvaluationNames
    {
        Test,
    }

	// Use this for initialization
	void Start () {
	
	}

    void CreateEvaluations()
    {

    }

	// Update is called once per frame
	void Update () {
	
	}

    void UpdateEvaluations()
    {

    }

    public float RunEvaluation(EvaluationNames evaluation)
    {
        return RunEvaluation((int)evaluation);
    }
}
