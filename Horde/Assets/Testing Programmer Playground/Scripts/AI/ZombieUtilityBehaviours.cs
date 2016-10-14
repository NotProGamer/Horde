using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieUtilityBehaviours : MonoBehaviour {

    public enum BehaviourNames
    {
        Idle,
        Wander,
        Investigate,
        Devour,
        Chase,
        GoToUserTap,
        Death,
    }

    private Dictionary<BehaviourNames, BaseBehaviour> m_behaviours = new Dictionary<BehaviourNames, BaseBehaviour>();
	// Use this for initialization
	void Start ()
    {
        BaseBehaviour idle = new Idle(this.gameObject);
        BaseBehaviour wander = new ZombieWander(this.gameObject);
        BaseBehaviour goToUserTap = new GoToUserTap(this.gameObject, Labels.Memory.LastUserTap);
        BaseBehaviour devour = new Devour(this.gameObject, Labels.Memory.ClosestCorpse);
        //BaseBehaviour investigate = new InvestigateNoise(this.gameObject);
        BaseBehaviour investigate = new GoToMemoryLocation(this.gameObject, Labels.Memory.LastPriorityNoise);
        BaseBehaviour death = new Death(this.gameObject);
        BaseBehaviour chase = new Chase(this.gameObject, Labels.Memory.ClosestEnemy);

        m_behaviours.Add(BehaviourNames.Idle, idle);
        m_behaviours.Add(BehaviourNames.Wander, wander);
        m_behaviours.Add(BehaviourNames.GoToUserTap, goToUserTap);
        m_behaviours.Add(BehaviourNames.Death, death);
        m_behaviours.Add(BehaviourNames.Devour, devour);
        m_behaviours.Add(BehaviourNames.Chase, chase);
        m_behaviours.Add(BehaviourNames.Investigate, investigate);
    }

    // Update is called once per frame
    //void Update ()    {	}

    public void RunBehaviour(BehaviourNames pName)
    {
        BaseBehaviour behaviourExample = null;
        if (m_behaviours.TryGetValue(pName, out behaviourExample))
        {
            // success
            behaviourExample.Tick();
        }
        else
        {
            // fail
            //Debug.Log("Unable to Locate Behaviour: " + pName.ToString());
        }
    }

}
