using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class HumanMovement : Movement {

    public Animator m_anim = null;
    private HumanBehaviours m_humanBehavioursScript = null;

    //private Health m_health = null;
    new void Awake()
    {
        base.Awake();
        //GameObject obj = GameObject.FindGameObjectWithTag(Labels.Tags.GameController);
        //if (obj)
        //{
        //    m_patrolManagerScript = obj.GetComponent<PatrolManager>();
        //    if (m_patrolManagerScript == null)
        //    {
        //        Debug.Log("PatrolManager not included!");
        //    }
        //}
        //else
        //{
        //    Debug.Log("GameController not included!");
        //}

        //m_health = GetComponent<Health>();
        //if (m_health == null)
        //{
        //    Debug.Log("Health not included!");
        //}
        m_humanBehavioursScript = GetComponent<HumanBehaviours>();
        if (m_humanBehavioursScript == null)
        {
            Debug.Log("Brain script not included!");
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
        //RequestPatrolRoute();
    }

    // Update is called once per frame
    new void Update ()
    {
        //if (m_health.IsDead())
        //{
        //    Stop();
        //}
        //else
        //{
        //    Patrol();
        //}
        
        base.Update();

        if (m_anim != null && m_nav != null)
        {
            m_anim.SetFloat("Movement", m_nav.velocity.magnitude / m_maxSpeed);

            m_anim.SetBool("Moving", m_nav.velocity.sqrMagnitude > 0);
        }
        if (m_anim != null && m_humanBehavioursScript != null)
        {
            AnimateBehaviour(m_humanBehavioursScript.m_currentBehaviour);
        }
    }

    private HumanBehaviours.BehaviourNames m_currentBehaviour = HumanBehaviours.BehaviourNames.Idle;
    void AnimateBehaviour(HumanBehaviours.BehaviourNames currentBehaviour)
    {
        if (m_currentBehaviour != currentBehaviour)
        {
            //exit

            switch (m_currentBehaviour)
            {
                //case HumanBehaviours.BehaviourNames.Wander:
                //    break;
                //case HumanBehaviours.BehaviourNames.Patrol:
                //    break;
                //case HumanBehaviours.BehaviourNames.Guard:
                //    break;
                //case HumanBehaviours.BehaviourNames.Investigate:
                //    break;
                //case HumanBehaviours.BehaviourNames.MoveToEnemy:
                //    break;
                //case HumanBehaviours.BehaviourNames.Flee:
                //    break;
                //case HumanBehaviours.BehaviourNames.SeekCover:
                //    break;
                case HumanBehaviours.BehaviourNames.Idle:
                    m_anim.SetBool("Idle", false);
                    break;
                case HumanBehaviours.BehaviourNames.AttackIfInRange:
                    m_anim.SetBool("Attacking", false);
                    break;
                case HumanBehaviours.BehaviourNames.Death:
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
                //case HumanBehaviours.BehaviourNames.Wander:
                //    break;
                //case HumanBehaviours.BehaviourNames.Patrol:
                //    break;
                //case HumanBehaviours.BehaviourNames.Guard:
                //    break;
                //case HumanBehaviours.BehaviourNames.Investigate:
                //    break;
                //case HumanBehaviours.BehaviourNames.MoveToEnemy:
                //    break;
                //case HumanBehaviours.BehaviourNames.Flee:
                //    break;
                //case HumanBehaviours.BehaviourNames.SeekCover:
                //    break;
                case HumanBehaviours.BehaviourNames.Idle:
                    m_anim.SetBool("Idle", true);
                    break;
                case HumanBehaviours.BehaviourNames.AttackIfInRange:
                    m_anim.SetBool("Attacking", true);
                    break;
                case HumanBehaviours.BehaviourNames.Death:
                    m_anim.SetBool("Dead", true);
                    break;
                default:
                    break;
            }
        }
    }

    //private void Stop()
    //{
    //    SetDestination(transform.position);
    //}



    //// The following code is more to do with behaviour and will later be move out of this script

    //public PatrolManager.PatrolRoute m_patrolRoute = null;
    //public string m_patrolIdentifier = "";
    //private PatrolManager m_patrolManagerScript = null;
    //public int m_currentPatrolPointIndex = -1;

    //void RequestPatrolRoute()
    //{
    //    m_patrolRoute = m_patrolManagerScript.GetPatrolRoute(m_patrolIdentifier);
    //}

    //private void Patrol()
    //{

    //    if (m_patrolRoute != null)
    //    {
    //        if (!m_patrolRoute.IsEmpty())
    //        {
    //            if (ReachedDestination())
    //            {
    //                Vector3 nextDestination = transform.position;
    //                // set current patrol index
    //                m_currentPatrolPointIndex++;
    //                if (m_currentPatrolPointIndex >= m_patrolRoute.m_patrolPoints.Count)
    //                {
    //                    m_currentPatrolPointIndex = 0;
    //                }
    //                // get current patrol transform
    //                Transform nextTransform = m_patrolRoute.m_patrolPoints[m_currentPatrolPointIndex];
    //                // set next destination
    //                if (nextTransform != null)
    //                {
    //                    nextDestination = nextTransform.position;
    //                    SetDestination(nextDestination);
    //                }
    //                else
    //                {
    //                    Debug.Log("Invalid PatrolPoint: no patrol point at index: "+ m_currentPatrolPointIndex + "!");
    //                }
    //                // Set Destination
    //                //SetDestination(nextDestination);
    //            }
    //        }
    //        else
    //        {
    //            Debug.Log("Invalid PatrolRoute: no patrol points in " + m_patrolRoute.m_identifier + "!");
    //        }
    //    }
    //}

}
