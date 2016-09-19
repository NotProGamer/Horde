using UnityEngine;
using System.Collections;

public class Chase : SequenceBehaviour
{
    public Chase(GameObject pParent, string pMemoryLabel) : base(pParent)
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


        // Attack
        // if in range call attack
        // attack script should check if attack available,
        // if attack is available, trigger attack animation
        // attack animation should trigger a damage effect at the appropriate time in the animation

    }


}
