using UnityEngine;
using System.Collections;

public class Death : BaseBehaviour
{
    public Death(GameObject pParent):base(pParent)
    {
        if (m_parent == null)
        {
            Debug.Log("Parent game Object not included.");
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


        // Do stuff



        Debug.Log("Death Behaviour");
        return Status.SUCCESS;

    }

}
