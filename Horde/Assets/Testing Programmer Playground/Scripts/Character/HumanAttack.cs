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
        if (m_soundsEnabled)
        {
            SoundLibrary.PlaySound(gameObject, m_sounds.Attack);
        }

    }


    [System.Serializable]
    public class SoundsStrings
    {
        //public string Idle ="";
        public string Attack = "HumanAttackMelee";
        //public string Investigate = "";
        //public string Devour = "ZombieDevour";
        //public string Chase = "";
        //public string GoToUserTap = "ZombieHearsUserTap";
        //public string Death = "ZombieDeath";
        //public string Reanimating = "ZombieReanimating";

    }
    public SoundsStrings m_sounds;
    public bool m_soundsEnabled = true;

}
