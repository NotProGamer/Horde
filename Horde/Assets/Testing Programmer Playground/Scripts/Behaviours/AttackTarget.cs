using UnityEngine;
using System.Collections;

public class AttackTarget : BaseBehaviour {

    private ZombieBrain m_zombieBrainScript = null;
    private Attack m_attackScript = null;
    private string m_memoryLocation = "";

    public AttackTarget(GameObject pParent, string pMemoryLabel):base(pParent)
    {
        if (m_parent == null)
        {
            Debug.Log("Parent game Object not included.");
            return; // early exit
        }
        m_zombieBrainScript = m_parent.GetComponent<ZombieBrain>();
        if (m_zombieBrainScript == null)
        {
            Debug.Log("ZombieBrain Script not included");
        }

        m_attackScript = m_parent.GetComponent<Attack>();
        if (m_attackScript == null)
        {
            Debug.Log("Attack Script not included");
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

        if (m_attackScript)
        {
            object currentTarget = null;
            if (!m_zombieBrainScript.GetObjectFromMemory(m_memoryLocation, out currentTarget))
            {
                Debug.Log("Fail attack");
                return Status.FAILURE; // early exit

            }
            
            GameObject test = (GameObject)currentTarget;
            if (m_attackScript.InsideAttackRange(test.transform))
            {
                m_attackScript.AttackTarget(test);
                
            }
        }

        //Debug.Log("Idle");
        return Status.SUCCESS;
    }


}
