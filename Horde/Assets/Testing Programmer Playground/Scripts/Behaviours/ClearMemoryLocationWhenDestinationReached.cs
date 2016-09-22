using UnityEngine;
using System.Collections;

public class ClearMemoryLocationWhenDestinationReached : BaseBehaviour {

    private string m_memoryLocation = "";
    private Movement m_movementScript = null;
    private ZombieBrain m_zombieBrainScript = null;

    public ClearMemoryLocationWhenDestinationReached(GameObject pParent, string pMemoryLabel) : base(pParent)
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
        m_zombieBrainScript = m_parent.GetComponent<ZombieBrain>();
        if (m_zombieBrainScript == null)
        {
            Debug.Log("ZombieBrain Script not included");
        }
        m_memoryLocation = pMemoryLabel;
    }

    public override Status Update()
    {
        if (m_parent == null)
        {
            Debug.Log("Parent GameObject not included.");
            return Status.FAILURE; // early exit
        }

        if (m_movementScript)
        {
            if (m_movementScript.ReachedDestination())
            {
                m_zombieBrainScript.ClearMemoryLocation(m_memoryLocation);
                //Debug.Log("Reached Destination Forgetting LastTap");
            }
        }
        else
        {
            return Status.FAILURE; // early exit
        }


        //Debug.Log("Idle");
        return Status.SUCCESS;
    }
}
