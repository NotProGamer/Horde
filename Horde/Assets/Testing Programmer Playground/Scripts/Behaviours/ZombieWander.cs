using UnityEngine;
using System.Collections;

public class ZombieWander : SequenceBehaviour
{
    public ZombieWander(GameObject pParent) : base(pParent)
    {
        if (m_parent == null)
        {
            Debug.Log("Parent game Object not included.");
            return; // early exit
        }

        // set current target
        // set speed


        BaseBehaviour setZombieSpeed = new SetZombieMovementSpeed(pParent);
        m_children.Add(setZombieSpeed);
        BaseBehaviour wander = new Wander(pParent);
        m_children.Add(wander);

    }

}
