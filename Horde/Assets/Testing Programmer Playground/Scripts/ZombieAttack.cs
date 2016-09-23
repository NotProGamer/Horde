using UnityEngine;
using System.Collections;
using System;

public class ZombieAttack : Attack {

    private HealthCondition m_healthCondition = null;
    private HealthCondition m_targetHealthCondition = null;

    // Use this for initialization
    void Start ()
    {
        m_healthCondition = GetComponent<HealthCondition>();
        if (m_healthCondition == null)
        {
            Debug.Log("HealthCondition not included");
        }

    }

    public override void DamageTarget()
    {
        // this is to be triggerred by the attack animation
        if (m_health.IsDead())
        {
            return; // early exit
        }

        if (m_currentTarget != null)
        {
            // get current target health
            m_currentTargetHealth = m_currentTarget.GetComponent<Health>();
            
            // if target has health and is not dead
            if (m_currentTargetHealth != null && !m_currentTargetHealth.IsDead())
            {
                //if target still in range
                if (InsideAttackRange(m_currentTarget.transform))
                {
                    // apply damage
                    m_currentTargetHealth.ApplyDamage(m_damage);
                    // infect target
                    m_targetHealthCondition = m_currentTarget.GetComponent<HealthCondition>();
                    if (m_targetHealthCondition != null)
                    {
                        m_targetHealthCondition.Infect();
                    }
                    Debug.Log("Hit");
                }
                else
                {
                    Debug.Log("Miss");
                }
            }
        }
    }

    public bool DevourTarget(GameObject pTargetGameObject)
    {
        bool result = false;
        if (!m_health.IsDead())
        {
            return result; // early exit
        }

        if (pTargetGameObject == null)
        {
            return result; // early exit
        }

        if (m_nextAttackTime < Time.time)
        {

            // if target changed 
            if (m_currentTarget == null || m_currentTarget != pTargetGameObject)
            {
                // Set Current Target
                m_currentTarget = pTargetGameObject;
                //clear current target details
                m_currentTargetHealth = null;
            }

            // ========================
            // Trigger Attack Animation HERE
            // ========================
            TriggerDevourAnimation();

            m_nextAttackTime = Time.time + m_delay;
            result = true;
        }

        return result;
    }

    private void TriggerDevourAnimation()
    {
        Debug.Log("Devour Target");
    }

    public void ConsumeTarget()
    {
        // this is to be triggerred by the attack animation
        if (!m_health.IsDead())
        {
            return; // early exit
        }

        if (m_currentTarget != null)
        {
            // get current target health
            m_currentTargetHealth = m_currentTarget.GetComponent<Health>();

            // if target has health and is not dead
            if (m_currentTargetHealth != null && !m_currentTargetHealth.IsDead())
            {
                //if target still in range
                if (InsideAttackRange(m_currentTarget.transform))
                {
                    // apply damage
                    m_health.ApplyHeal(m_currentTargetHealth.DevourCorpse(m_damage));
                    // infect target
                    Debug.Log("Eat");
                }
                else
                {
                    Debug.Log("Go Hungry");
                }
            }
        }

    }

}
