using UnityEngine;
using System.Collections;

public class Devour : BaseBehaviour
{

    public Devour(GameObject pParent):base(pParent)
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



        Debug.Log("Devour Behaviour");
        return Status.SUCCESS;

    }
}
