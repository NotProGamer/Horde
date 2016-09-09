using UnityEngine;
using System.Collections;

public class MoveToCurrentDestination : BaseBehaviour {
    private Movement m_movementScript = null;

    public MoveToCurrentDestination(GameObject pParent):base(pParent)
    {
        if (m_parent == null)
        {
            Debug.Log("Parent game Object not included.");
            return; // early exit
        }
        m_movementScript = m_parent.GetComponent<Movement>();
        if (m_movementScript == null)
        {
            Debug.Log("Movement Script not included");
        }
    }


    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    public override Status Update()
    {
        if (m_parent == null)
        {
            Debug.Log("Parent GameObject not included.");
            return Status.FAILURE; // early exit
        }

        if (m_movementScript)
        {
            // do move to current target
        }

        //Debug.Log("Idle");
        return Status.SUCCESS;
    }

}
