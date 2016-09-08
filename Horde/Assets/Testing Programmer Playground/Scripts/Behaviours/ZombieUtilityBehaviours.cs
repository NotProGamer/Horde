using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieUtilityBehaviours : MonoBehaviour {

    public enum BehaviourNames
    {
        Idle,
        Wander,
    }

    private Dictionary<BehaviourNames, BaseBehaviour> m_behaviours = new Dictionary<BehaviourNames, BaseBehaviour>();
	// Use this for initialization
	void Start ()
    {
        BaseBehaviour wander = new Wander(this.gameObject);
        m_behaviours.Add(BehaviourNames.Wander, wander);
	
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
