using UnityEngine;
using System.Collections;

public class GoToUserTap : SequenceBehaviour
{
    
    public GoToUserTap(GameObject pParent, string pMemoryLabel) : base(pParent)
    {
        if (m_parent == null)
        {
            Debug.Log("Parent game Object not included.");
            return; // early exit
        }

        // set current target
        // set speed


        BaseBehaviour setCurrentTarget = new SetCurrentTargetFromMemory(pParent, pMemoryLabel);
        m_children.Add(setCurrentTarget);
        BaseBehaviour setZombieSpeed = new SetZombieMovementSpeed(pParent);
        m_children.Add(setZombieSpeed);
        BaseBehaviour moveToCurrentTarget = new MoveToMemoryLocation(pParent, Labels.Memory.CurrentTarget);
        m_children.Add(moveToCurrentTarget);
        BaseBehaviour clearMemoryWhenReached = new ClearMemoryLocationWhenDestinationReached(pParent, pMemoryLabel);
        m_children.Add(clearMemoryWhenReached);
    }



}
