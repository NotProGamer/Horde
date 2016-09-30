using UnityEngine;
using System.Collections;

public class MoveToMemoryLocation : BaseBehaviour
{

    private string m_memoryLocation = "";
    private Movement m_movementScript = null;
    private ZombieBrain m_zombieBrainScript = null;

    private NavMeshAgent m_nav = null;

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
        m_nav = m_parent.GetComponent<NavMeshAgent>();
        
    }

    private Vector3 m_lastPostion;
    private object m_lastTarget = null;
    private float delay = 0.5f;
    private float m_nextDestinationChange = 0f;

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

            bool targetChanged = false;
            object test = null;
            if (m_zombieBrainScript.GetObjectFromMemory(Labels.Memory.CurrentTarget, out test))
            {
                // if target changed 
                if (m_lastTarget != test)
                {
                    m_lastTarget = test;
                    targetChanged = true;
                }
            }

            
            Vector3 position = new Vector3();
            if (!m_zombieBrainScript.GetCurrentTargetPosition(out position))
            {
                return Status.FAILURE; // early exit
            }

            // should validate position is on nav mesh


            

            if (targetChanged)
            {
                m_movementScript.SetDestination(position);
            }
            else
            {
                // if current target is the same but the position has changed then mobile target
                if (m_lastPostion != position)
                {
                    m_lastPostion = position;

                    // if mobile target delay destination change
                    if (m_nextDestinationChange < Time.time)
                    {
                        m_movementScript.SetDestination(position);
                        m_nextDestinationChange = Time.time + delay;
                    }
                    //Debug.Log("Move TO memory location");
                }
            }




        }

        //Debug.Log("Idle");
        return Status.SUCCESS;
    }





}


