using UnityEngine;
using System.Collections;

public class ReanimationComplete : StateMachineBehaviour {

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        ZombieMovement m_zombieMovementScript = animator.transform.parent.GetComponent<ZombieMovement>();
        if (m_zombieMovementScript)
        {
            m_zombieMovementScript.EndReanimationBehaviour();
            Debug.Log("reanimating zombie ");
        }
    }
}
