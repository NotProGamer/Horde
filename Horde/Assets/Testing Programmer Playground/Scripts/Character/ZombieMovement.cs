using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieMovement : Movement
{


    // references .. parenting on top of monobehaviour, http://answers.unity3d.com/questions/362575/inheritance-hides-start.html
    public Animator m_anim = null;
    protected ZombieUtilityAI m_zombieUtilityAIScript = null;

    protected ZombieUtilityBehaviours.BehaviourNames m_currentBehaviour = ZombieUtilityBehaviours.BehaviourNames.Idle;
    public bool m_animateReanimation = true; // change this to true

    public void EndReanimationBehaviour()
    {
        m_animateReanimation = false;
        m_nav.Resume();
    }

    public void OnEnable()
    {
        m_animateReanimation = true; // change this to true;
    }


    new void Awake()
    {
        base.Awake();
        //GameObject obj = GameObject.FindGameObjectWithTag(Labels.Tags.GameController);
        //if (obj)
        //{
        //    //
        //}
        //else
        //{
        //    Debug.Log("GameController not included!");
        //}

        m_zombieUtilityAIScript = GetComponent<ZombieUtilityAI>();
        if (m_zombieUtilityAIScript == null)
        {
            Debug.Log("ZombieUtilityAI script not included!");
        }
    }


    // Use this for initialization
    new void Start ()
    {
        base.Start();
        if (m_anim != null)
        {
            m_anim.SetBool("Idle", true);
        }
    }

    // Update is called once per frame
    new void Update ()
    {
        //UpdateAnimation();
        base.Update();
        UpdateAnimation();
    }

    
    
    protected virtual void UpdateAnimation()
    {
        if (m_anim != null && m_nav != null)
        {
            m_anim.SetFloat("Movement", m_nav.velocity.magnitude / m_maxSpeed);

            m_anim.SetBool("Moving", m_nav.velocity.sqrMagnitude > 0);
        }
        if (m_anim != null && m_zombieUtilityAIScript != null)
        {
            AnimateBehaviour(m_zombieUtilityAIScript.m_currentBehaviour);

            
            
            //// animate movement
            //switch (m_state)
            //{
            //    case State.Idle:
            //        m_anim.SetBool("Moving", false);
            //        break;
            //    case State.Moving:
            //        m_anim.SetBool("Moving", true);
            //        break;
            //    default:
            //        break;
            //}
        }
    }


    
    protected virtual void AnimateBehaviour(ZombieUtilityBehaviours.BehaviourNames currentBehaviour)
    {
        if (m_currentBehaviour != currentBehaviour)
        {
            //exit
            switch (m_currentBehaviour)
            {
                //case ZombieUtilityBehaviours.BehaviourNames.Wander:
                //    break;
                //case ZombieUtilityBehaviours.BehaviourNames.Investigate:
                //    break;
                //case ZombieUtilityBehaviours.BehaviourNames.Chase:
                //    break;
                //case ZombieUtilityBehaviours.BehaviourNames.GoToUserTap:
                //    break;
                //case ZombieUtilityBehaviours.BehaviourNames.Devour:
                //    m_anim.SetBool("Devouring", false);
                //    break;
                case ZombieUtilityBehaviours.BehaviourNames.Idle:
                    m_anim.SetBool("Idle", false);
                    break;
                case ZombieUtilityBehaviours.BehaviourNames.Death:
                    m_anim.SetBool("Dead", false);
                    break;
                default:
                    break;
            }

            // update
            m_currentBehaviour = currentBehaviour;

            // enter
            switch (m_currentBehaviour)
            {
                //case ZombieUtilityBehaviours.BehaviourNames.Wander:
                //    break;
                //case ZombieUtilityBehaviours.BehaviourNames.Investigate:
                //    break;
                //case ZombieUtilityBehaviours.BehaviourNames.Chase:
                //    break;
                //case ZombieUtilityBehaviours.BehaviourNames.GoToUserTap:
                //    break;
                //case ZombieUtilityBehaviours.BehaviourNames.Devour:
                //    m_anim.SetBool("Devouring", true);
                //    break;
                case ZombieUtilityBehaviours.BehaviourNames.Idle:
                    m_anim.SetBool("Idle", true);
                    break;
                case ZombieUtilityBehaviours.BehaviourNames.Death:
                    m_anim.SetBool("Dead", true);
                    break;
                default:
                    break;
            }
        }
    }


}

