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
    }

    private Dictionary<BehaviourNames, BaseBehaviour> m_behaviours = new Dictionary<BehaviourNames, BaseBehaviour>();
	// Use this for initialization
	void Start ()
    {
        BaseBehaviour idle = new Idle(this.gameObject);
        BaseBehaviour wander = new ZombieWander(this.gameObject);
        BaseBehaviour goToUserTap = new GoToUserTap(this.gameObject, Labels.Memory.LastUserTap);
        m_behaviours.Add(BehaviourNames.Idle, idle);
        m_behaviours.Add(BehaviourNames.Wander, wander);
        m_behaviours.Add(BehaviourNames.GoToUserTap, goToUserTap);
    }

    // Update is called once per frame
    void Update ()
    {
	
	}

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
