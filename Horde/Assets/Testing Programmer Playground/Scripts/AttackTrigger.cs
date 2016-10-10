using UnityEngine;
using System.Collections;

public class AttackTrigger : MonoBehaviour {

    private ZombieAttack m_zombieAttackScript = null;
    private Health m_health = null;


    void Awake()
    {
        m_zombieAttackScript = GetComponent<ZombieAttack>();
        if (m_zombieAttackScript == null)
        {
            Debug.Log("ZombieAttack script not included");
        }
        m_health = GetComponent<Health>();
        if (m_health == null)
        {
            Debug.Log("Health script not included");
        }

    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void OnTriggerStay(Collider other)
    {
        //Debug.Log("Bump: " + other.name);
        if (m_zombieAttackScript != null)
        {
            if (Labels.Tags.IsHuman(other.gameObject) || Labels.Tags.IsDestructible(other.gameObject))
            {
                Health m_healthOther = other.GetComponent<Health>();

                if (m_health != null && m_healthOther != null)
                {

                    if (m_healthOther.IsDead())
                    {
                        if (m_health.IsDamaged() && Labels.Tags.IsHuman(other.gameObject))
                        {
                            m_zombieAttackScript.DevourTarget(other.gameObject);
                        }
                        else
                        {
                            // ignore
                        }
                    }
                    else
                    {
                        m_zombieAttackScript.AttackTarget(other.gameObject);
                    }
                }
            }
        }
    }
}
