using UnityEngine;
using System.Collections;
using System;

public class Attack : MonoBehaviour {

    public float m_delay = 0.5f;
    public int m_damage = 10;
    public float m_attackRange = 10f;
    private float m_nextAttackTime = 0.0f;

    private Health m_health = null;
    private GameObject m_currentTarget = null;
    private Health m_currentTargetHealth = null;

    void Awake()
    {
        m_health = GetComponent<Health>();
        if (m_health == null)
        {
            Debug.Log("Health not included");
        }

    }


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void AttackTarget(GameObject pTargetGameObject)
    {

        if (m_health.IsDead())
        {
            return; // early exit
        }

        if (pTargetGameObject == null)
        {
            return; // early exit
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
            TriggerAttackAnimation();

            m_nextAttackTime = Time.time + m_delay;
        }
    }

    protected virtual void TriggerAttackAnimation()
    {
        // ========================
        // Trigger Attack Animation HERE
        // ========================
        throw new NotImplementedException();
    }

    public virtual void DamageTarget()
    {
        // this is to be triggerred by the attack animation
        if (m_health.IsDead())
        {
            return; // early exit
        }

        if (m_currentTarget != null)
        {
            m_currentTargetHealth = m_currentTarget.GetComponent<Health>();
            // if target has health and is not dead
            if (m_currentTargetHealth != null && !m_currentTargetHealth.IsDead())
            {
                // apply damage
                m_currentTargetHealth.ApplyDamage(m_damage);
            }
        }
        
    }


}
