using UnityEngine;
using System.Collections;

public class Idle : BaseBehaviour {

    private Movement m_movementScript = null;

    public Idle(GameObject pParent):base(pParent)
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
            m_movementScript.SetDestination(m_parent.transform.position);
        }

        //Debug.Log("Idle");
        return Status.SUCCESS;
    }
}
