using UnityEngine;
using System.Collections;

public class MoveToMemoryLocation : BaseBehaviour
{

    private string m_memoryLocation = "";
    private Movement m_movementScript = null;
    private ZombieBrain m_zombieBrainScript = null;


    public MoveToMemoryLocation(GameObject pParent, string pMemoryLocation):base(pParent)
    {
        if (m_parent == null)
        {
            Debug.Log("Parent game Object not included.");
            return; // early exit
        }
        m_zombieBrainScript = m_parent.GetComponent<ZombieBrain>();
        if (m_zombieBrainScript == null)
        {
            Debug.Log("ZombieBrain not included.");
            return; // early exit
        }

        m_movementScript = m_parent.GetComponent<Movement>();
        if (m_movementScript == null)
        {
            Debug.Log("Movement Script not included");
        }
        m_memoryLocation = pMemoryLocation;
    }


    public override Status Update()
    {
        if (m_parent == null)
        {
            Debug.Log("Parent GameObject not included.");
            return Status.FAILURE; // early exit
        }



        if (m_movementScript == null || m_zombieBrainScript == null)
        {
            return Status.FAILURE; // early exit
        }
        else
        {
            // do move to memory location
            Vector3 position = new Vector3();
            if (!m_zombieBrainScript.GetCurrentTargetPosition(out position))
            {
                return Status.FAILURE; // early exit
            }
            
            // should validate position is on nav mesh
            m_movementScript.SetDestination(position);
        }

        //Debug.Log("Idle");
        return Status.SUCCESS;
    }

}
