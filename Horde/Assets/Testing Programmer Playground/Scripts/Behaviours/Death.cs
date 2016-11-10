using UnityEngine;
using System.Collections;

public class Death : BaseBehaviour
{
    private NavMeshAgent m_nav = null;

    public Death(GameObject pParent):base(pParent)
    {
        if (m_parent == null)
        {
            Debug.Log("Parent game Object not included.");
            return; // early exit
        }

        m_nav = m_parent.GetComponent<NavMeshAgent>();
        if (m_nav)
        {
            Debug.Log("NavMeshAgent not included");
        }
    }

    public override Status Update()
    {
        if (m_parent == null)
        {
            Debug.Log("Parent GameObject not included.");
            return Status.FAILURE; // early exit
        }

        if (m_nav)
        {
            m_nav.Stop();
            //Debug.Log("Sleep");
            //m_nav.enabled = false;
        }
        // Do stuff



        //Debug.Log("Death Behaviour");
        return Status.SUCCESS;

    }

}
