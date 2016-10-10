using UnityEngine;
using System.Collections;

public class Devour : SequenceBehaviour
{

    public Devour(GameObject pParent, string pMemoryLabel):base(pParent)
    {
        if (m_parent == null)
        {
            Debug.Log("Parent game Object not included.");
            return; // early exit
        }

        //BaseBehaviour goToMemoryLocation = new GoToMemoryLocation(pParent, pMemoryLabel);
        //m_children.Add(goToMemoryLocation);
        BaseBehaviour setCurrentTarget = new SetCurrentTargetFromMemory(pParent, pMemoryLabel);
        m_children.Add(setCurrentTarget);
        BaseBehaviour setZombieSpeed = new SetZombieMovementSpeed(pParent);
        m_children.Add(setZombieSpeed);
        BaseBehaviour moveToCurrentTarget = new MoveToMemoryLocation(pParent, Labels.Memory.CurrentTarget);
        m_children.Add(moveToCurrentTarget);
        //BaseBehaviour devourZombie = new DevourTarget(pParent, pMemoryLabel);
        //m_children.Add(devourZombie);

    }

}
