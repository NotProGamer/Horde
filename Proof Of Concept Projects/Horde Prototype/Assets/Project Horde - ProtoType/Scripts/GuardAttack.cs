using UnityEngine;
using System.Collections;

public class GuardAttack : MonoBehaviour {


    public float m_attackDelay = 0.5f;
    public int m_attackDamage = 10;
    private float m_nextAttack = 0.0f;

    private Health m_health = null;


    public Transform m_shotOrigin = null;
    private float m_effectsTimer;
    public float m_range = 100.0f;
    private Ray m_shotRay;
    private RaycastHit m_shotHit;
    LineRenderer m_shotLine;
    float effectsDisplayTime = 0.2f;



    void Awake()
    {
        m_health = GetComponent<Health>();
        if (m_health == null)
        {
            Debug.Log("Health not included");
        }

        if (m_shotOrigin != null)
        {
            m_shotLine = m_shotOrigin.GetComponent<LineRenderer>();
            if (m_shotLine == null)
            {
                Debug.Log("LineRenderer not included");
            }
        }
        else
        {
            Debug.Log("Shot Origin not included");
        }

    }


    void OnTriggerStay(Collider other)
    {
        if (m_health)
        {
            if (m_health.IsDead())
            {
                return; // if dead you can't attack;
            }
        }

        if (other.isTrigger)
        {
            return; // early exit if is trigger
        }

        if (Tags.IsZombie(other.gameObject))
        {
            Attack(other);
            //// if can attack
            //if (Time.time > m_nextAttack)
            //{
            //    Health otherHealthScript = other.gameObject.GetComponent<Health>();
            //    // if has health 
            //    if (otherHealthScript)
            //    {
            //        // if not dead
            //        if (!otherHealthScript.IsDead())
            //        {
            //            //then attack
            //            otherHealthScript.ApplyDamage(m_attackDamage);
            //            m_nextAttack = Time.time + m_attackDelay;
            //            Debug.Log(name + " attacked " + other.gameObject.name + " for " + m_attackDamage + " damage.");
            //        }
            //    }
            //}
        }

    }



    // Update is called once per frame
    void Update()
    {
        m_effectsTimer += Time.deltaTime;

        if (m_effectsTimer >= m_attackDelay * effectsDisplayTime)
        {
            DisableEffects();
        }
    }

    private void Attack(Collider other)
    {
        // if can attack
        if (Time.time > m_nextAttack)
        {
            Health otherHealthScript = other.gameObject.GetComponent<Health>();
            // if has health 
            if (otherHealthScript)
            {
                // if not dead
                if (!otherHealthScript.IsDead())
                {
                    //then attack
                    m_effectsTimer = 0;


                    if (m_shotLine)
                    {
                        m_shotLine.enabled = true;
                        m_shotLine.SetPosition(0, m_shotOrigin.position);
                    }

                    m_shotRay.origin = m_shotOrigin.position;
                    m_shotRay.direction = other.gameObject.transform.position - m_shotOrigin.position;

                    if (Physics.Raycast(m_shotRay, out m_shotHit, m_range/*, shootableMask*/))
                    {
                        otherHealthScript.ApplyDamage(m_attackDamage);
                        m_nextAttack = Time.time + m_attackDelay;
                        Debug.Log(name + " attacked " + other.gameObject.name + " for " + m_attackDamage + " damage.");
                        if (m_shotLine) { m_shotLine.SetPosition(1, m_shotHit.point); }
                    }
                    else
                    {
                        if (m_shotLine) { m_shotLine.SetPosition(1, m_shotRay.origin + m_shotRay.direction * m_range); }
                    }

                }
            }
        }
    }
    public void DisableEffects()
    {
        if (m_shotLine) { m_shotLine.enabled = false; }
    }

}
