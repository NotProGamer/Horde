using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuardTowerDestruction : MonoBehaviour {

    private Health m_health = null;
    private HumanMovement m_humanMovement = null;

    public List<GameObject> m_guardsInTower = new List<GameObject>();



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
        if (m_health)
        {
            if (m_health.IsDead())
            {
                DestroyTower();
            }
        }
	}

    void DestroyTower()
    {
        // Kill Guards
        for (int i = 0; i < m_guardsInTower.Count; i++)
        {
            Health guardHealth = m_guardsInTower[i].GetComponent<Health>();
            if (guardHealth)
            {
                guardHealth.ApplyDamage(guardHealth.m_health); // 'Falling' damage
                
            }
            m_guardsInTower[i].SetActive(false);
            //HumanMovement m_humanMovement = m_guardsInTower[i].GetComponent<HumanMovement>();
            //if (m_humanMovement)
            //{
            //    m_humanMovement.FallToGround();
            //    Debug.Log("fall to ground");
            //}

        }
        //remove the tower
        gameObject.SetActive(false);
    }
}
