using UnityEngine;
using System.Collections;

public class HumanAttack : Attack {

    public Animator m_anim = null;
    protected override void TriggerAttackAnimation()
    {
        //if (m_anim)
        //{
        //    m_anim.SetTrigger("Attack");
        //}
        base.TriggerAttackAnimation();
        
    }
}
