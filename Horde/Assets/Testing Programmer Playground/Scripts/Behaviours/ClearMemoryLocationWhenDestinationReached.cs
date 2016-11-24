using UnityEngine;
using System.Collections;

public class ClearMemoryLocationWhenDestinationReached : BaseBehaviour {

    private string m_memoryLocation = "";
    private Movement m_movementScript = null;
    private ZombieBrain m_zombieBrainScript = null;

    private float timer = 0f;
    private float delay = 1f;
    private Vector3 lastPosition;

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
        lastPosition = m_parent.transform.position;
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

            if (CurrentTargetIsExpiredNoise() || InRangeOfCurrentTarget() || IfStuck())
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

    private bool IfStuck()
    {
        bool result = false;

        // changed is moving function from speed != 0 to speed < 1f 
        if (!m_movementScript.IsMoving())
        {
            if (lastPosition == m_parent.transform.position)
            {
                if (timer < Time.time)
                {
                    result = true;
                }
            }
        }
        else
        {
            lastPosition = m_parent.transform.position;
            timer = Time.time + delay;
        }
        return result;
    }

    private bool InRangeOfCurrentTarget()
    {
        bool result = false;

        float distanceToCurrentTarget = 0f;
        Vector3 currentTargetPosition = Vector3.zero;
        if (m_zombieBrainScript.GetCurrentTargetPosition(out currentTargetPosition))
        {
            distanceToCurrentTarget = (m_parent.transform.position - currentTargetPosition).sqrMagnitude;
            if (distanceToCurrentTarget < m_movementScript.m_touchRange * m_movementScript.m_touchRange)
            {
                result = true;
            }
        }
        return result;
    }

    bool CurrentTargetIsExpiredNoise()
    {
        bool result = false;


        result = m_zombieBrainScript.CurrentTargetIsExpiredNoise() && m_zombieBrainScript.GetTapInterest() <= 0;

        return result;
    }
}
