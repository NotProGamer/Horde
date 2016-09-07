using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

// dictionary reference : https://unity3d.com/learn/tutorials/modules/intermediate/scripting/lists-and-dictionaries

    // if have written this script as an example of how the Behaviour Controller will work.
    // The Behaviour Example Class will need to be replaced by the BaseBehaviour class.

public class UtilityBehaviours : MonoBehaviour {

    public enum BehaviourNames
    {
        testa,
        testb,
    }
    public class BehaviourExample
    {
        public virtual void Tick() { }
    }
    public class BehaviourExampleA : BehaviourExample
    {
        public override void Tick() { Debug.Log("boot"); }
    }
    public class BehaviourExampleB : BehaviourExample
    {
        private GameObject m_parent = null;
        public BehaviourExampleB(GameObject pParent)
        {
            m_parent = pParent;
        }
        public override void Tick()
        {
            PrintParentTag();
            Debug.Log("shoe");
        }

        public void PrintParentTag()
        {
            if (m_parent != null)
            {
                Debug.Log(m_parent.tag);
            }
        }
    }

    Dictionary<BehaviourNames, BehaviourExample> behavioursExample = new Dictionary<BehaviourNames, BehaviourExample>();

    // Use this for initialization
    void Start () {
        BehaviourExample exampleA = new BehaviourExampleA();
        BehaviourExample exampleB = new BehaviourExampleB(this.gameObject);
        behavioursExample.Add(BehaviourNames.testa, exampleA);
        behavioursExample.Add(BehaviourNames.testb, exampleB);
    }

    void RunBehaviour(BehaviourNames pName)
    {
        BehaviourExample behaviourExample = null;
        if (behavioursExample.TryGetValue(pName, out behaviourExample))
        {
            // success
            behaviourExample.Tick();
        }
        else
        {
            // fail
            Debug.Log("Unable to Locate Behaviour: " + pName.ToString());
        }
    }


    // Update is called once per frame
    //void Update ()    {	}


}
