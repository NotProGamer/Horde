using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

// dictionary reference : https://unity3d.com/learn/tutorials/modules/intermediate/scripting/lists-and-dictionaries


public class UtilityBehaviours : MonoBehaviour {

    public enum BehaviourNames
    {
        testa,
        testb,
    }

    //public class Boot : IComparable<Boot>
    //{
    //    public int value = 0;
    //    public int CompareTo(Boot other)
    //    {
    //        if (other == null)
    //        {
    //            return 1;
    //        }
    //        return value - other.value;
    //    }
    //    public void Tick() { }
    //}

    public class FootWear 
    {
        public virtual void Tick() { }
    }

    public class Boot : FootWear
    {
        public override void Tick() { Debug.Log("boot"); }
    }
    public class Shoe : FootWear
    {
        private GameObject m_parent = null;
        public Shoe(GameObject pParent)
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


    Dictionary<BehaviourNames, FootWear> footwears = new Dictionary<BehaviourNames, FootWear>();

    // Use this for initialization
    void Start () {
        FootWear leftfoot = new Boot();
        FootWear rightfoot = new Shoe(this.gameObject);
        footwears.Add(BehaviourNames.testa, leftfoot);
        footwears.Add(BehaviourNames.testb, rightfoot);



    }

    void RunBehaviour(BehaviourNames name)
    {
        FootWear footwear = null;

        if (footwears.TryGetValue(name, out footwear))
        {
            // success
            footwear.Tick();
        }
        else
        {
            // fail
        }

    }


    // Update is called once per frame
    void Update () {
	
	}


}
