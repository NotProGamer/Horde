using UnityEngine;
using System.Collections;
using System;

public class HumanBehaviours : BehaviourModule
{

    public enum BehaviourNames
    {
        Idle,
        Wander,
        Patrol,
        Guard,
        Investigate,
        MoveToEnemy,
        AttackIfInRange,
        Flee,
        SeekCover,
        Death,
    }





    public BehaviourNames m_currentBehaviour = BehaviourNames.Idle;

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

    }

    public void RunBehaviour(BehaviourNames pBehaviourName)
    {
        RunBehaviour((int)pBehaviourName);
    }
}
