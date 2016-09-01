using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Brain : MonoBehaviour {

    public float m_thinkDelay = 1f;
    private float m_nextThink = 0f;
    public List<Behaviour> m_behaviours;
    public Behaviour m_currentBehaviour = null;
    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Time.time > m_nextThink)
        {
            Behaviour nextBehaviour = null;
            float nextEvaluation = 0f;
            // evaluate
            foreach (Behaviour behaviour in m_behaviours)
            {
                //float evaluation = behaviour.Evaluate();
                //if (nextBehaviour == null)
                //{
                //    nextBehaviour = behaviour;
                //    nextEvaluation = evaluation;
                //}
                //else if (evaluation > nextEvaluation)
                //{
                //    nextBehaviour = behaviour;
                //    nextEvaluation = evaluation;
                //}
            }
            // find best bahaviour


            m_nextThink = Time.time + m_thinkDelay;
        }
	}

}
