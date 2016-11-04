using UnityEngine;
using System.Collections;

public class HumanAttack : Attack {

    //Added by Rory
    //public GameObject gunObject;
    //GunTest gun;

    public Animator m_anim = null;

    ////Added by Rory
    //void Start()
    //{
    //    gun = gunObject.GetComponent<GunTest>();
    //}

    //void Update()
    //{
    //    gun.Fire();
    //}


    protected override void TriggerAttackAnimation()
    {
        //if (m_anim)
        //{
        //    m_anim.SetTrigger("Attack");
        //}
        base.TriggerAttackAnimation();
        
    }

    
    
}
