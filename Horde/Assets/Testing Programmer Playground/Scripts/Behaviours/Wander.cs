using UnityEngine;
using System.Collections;

public class Wander : BaseBehaviour {

    public Wander(GameObject pParent):base(pParent)
    {
    }

    // Update is called once per frame
    public override Status Update()
    {
        // if arrived at destination
        // calculate nearby desitination, 
        // find closest destination is on nav mesh
        // head to the destination

        Debug.Log("Wander");
        return Status.SUCCESS;
    }
}
