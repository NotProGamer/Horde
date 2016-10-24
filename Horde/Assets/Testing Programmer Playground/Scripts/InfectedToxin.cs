using UnityEngine;
using System.Collections;

public class InfectedToxin : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider other)
    {
        
        if (Labels.Tags.IsHuman(other.gameObject))
        {
            Health otherHealth = other.GetComponent<Health>();
            if (otherHealth)
            {
                if (!otherHealth.IsDead())
                {
                    Debug.Log("Human being infected");
                    otherHealth.ApplyDamage(otherHealth.m_health);
                    HealthCondition m_targetHealthCondition = other.GetComponent<HealthCondition>();
                    if (m_targetHealthCondition != null)
                    {
                        m_targetHealthCondition.Infect();
                    }
                }
            }
        }
    }
}
