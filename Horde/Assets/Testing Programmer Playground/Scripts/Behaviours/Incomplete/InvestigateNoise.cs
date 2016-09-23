using UnityEngine;
using System.Collections;

public class InvestigateNoise : BaseBehaviour
{

    public InvestigateNoise(GameObject pParent):base(pParent)
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



        Debug.Log("InvestigateNoise Behaviour");
        return Status.SUCCESS;

    }

}
