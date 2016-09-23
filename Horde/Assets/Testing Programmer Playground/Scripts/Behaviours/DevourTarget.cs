using UnityEngine;
using System.Collections;

public class DevourTarget : BaseBehaviour
{

    private ZombieBrain m_zombieBrainScript = null;
    private ZombieAttack m_attackScript = null;
    private string m_memoryLocation = "";


    public DevourTarget(GameObject pParent, string pMemoryLabel) : base(pParent)
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

        m_attackScript = m_parent.GetComponent<ZombieAttack>();
        if (m_attackScript == null)
        {
            Debug.Log("ZombieAttack Script not included");
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
            object memoryObject = null;
            //Debug.Log(m_memoryLocation);
            if (!m_zombieBrainScript.GetObjectFromMemory(m_memoryLocation, out memoryObject))
            {
                Debug.Log("Fail attack");
                return Status.FAILURE; // early exit

            }
            GameObject test = null;

            if (memoryObject != null)
            {
                System.Type t = memoryObject.GetType();
                if (t == typeof(GameObject))
                {
                    test = ((GameObject)memoryObject);
                    
                    if (m_attackScript.InsideAttackRange(test.transform))
                    {
                        m_attackScript.DevourTarget(test);
                        
                    }
                }
                else
                {
                    Debug.Log("Null memory 1 at '" + m_memoryLocation + "'");
                }
            }
            else
            {
                Debug.Log("Null memory 2 at '" + m_memoryLocation + "'");
            }
        }

        //Debug.Log("Idle");
        return Status.SUCCESS;
    }
}
