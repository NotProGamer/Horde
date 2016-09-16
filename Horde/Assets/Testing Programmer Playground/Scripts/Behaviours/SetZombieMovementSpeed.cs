using UnityEngine;
using System.Collections;

public class SetZombieMovementSpeed : BaseBehaviour
{

    private ZombieBrain m_zombieBrainScript = null;

    public SetZombieMovementSpeed(GameObject pParent) : base(pParent)
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

    }

    public override Status Update()
    {
        if (m_parent == null)
        {
            Debug.Log("Parent GameObject not included.");
            return Status.FAILURE; // early exit
        }

        if (m_zombieBrainScript == null)
        {
            return Status.FAILURE;
            // Labels.Memory.LastUserTap
        }
        else
        {
            if (!m_zombieBrainScript.UpdateSpeed())
            {
                return Status.FAILURE;
            }
        }

        //Debug.Log("Wander");
        return Status.SUCCESS;
    }

}
