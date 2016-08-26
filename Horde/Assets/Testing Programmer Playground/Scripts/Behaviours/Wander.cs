using UnityEngine;
using System.Collections;

public class Wander : Behaviour {


    public override float Evaluate()
    {
        return m_weight;
    }

    // Update is called once per frame
    public override void Update()
    {
        // calculate nearby desitination, 
        // find closest destination is on nav mesh
    }
}
