using UnityEngine;
using System.Collections;

public class SetCurrentTargetFromMemory : BaseBehaviour {

    private ZombieBrain m_zombieBrainScript = null;
    private string m_memoryLabel = "";

    public SetCurrentTargetFromMemory(GameObject pParent, string pMemoryLabel)
        :base(pParent)
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
        m_memoryLabel = pMemoryLabel;
    }

    public override Status Update()
    {
        if (m_parent == null)
        {
            Debug.Log("Parent GameObject not included.");
            return Status.FAILURE; // early exit
        }

        if (m_zombieBrainScript == null || !m_zombieBrainScript.SetCurrentTarget(m_memoryLabel))
        {
            return Status.FAILURE;
            // Labels.Memory.LastUserTap
        }


        //Debug.Log("Wander");
        return Status.SUCCESS;
    }

}
