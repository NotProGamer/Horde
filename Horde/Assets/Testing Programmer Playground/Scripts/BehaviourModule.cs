using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BehaviourModule : MonoBehaviour {

    private Dictionary<int, BaseBehaviour> m_behaviours = new Dictionary<int, BaseBehaviour>();

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected void RunBehaviour(int behaviourIndex)
    {
        BaseBehaviour behaviourToRun = null;

        if (m_behaviours.TryGetValue(behaviourIndex, out behaviourToRun))
        {
            behaviourToRun.Tick();
        }
        else
        {
            Debug.Log("Unknown Behaviour at index " + behaviourIndex+ ".");
        }
    }

    public bool AddBehaviour(int behaviourIndex, BaseBehaviour behaviour, bool overwrite = false)
    {
        bool result = false;


        if (m_behaviours.ContainsKey(behaviourIndex))
        {
            if (overwrite)
            {
                m_behaviours.Add(behaviourIndex, behaviour);
            }
            else
            {
                Debug.Log("Behaviour Index already in use. Use overwrite switch or choose a different index");
            }
        }
        else
        {
            m_behaviours.Add(behaviourIndex, behaviour);
        }

        return result;
    }

}
