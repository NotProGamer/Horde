﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
///  Code Author: Reece Howe
///  Project: Horde
///  Engine: Unity
///  Platform: Mobile and PC
///  Notes: zombies attack enemies when they are in range.
///  Status: Complete
/// </summary>
/// 
public class ZombieAttack : MonoBehaviour {

    public float m_attackDelay = 0.5f;
    public int m_attackDamage = 10;
    private float m_nextAttack = 0.0f;

    private Health m_health = null;

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

    void OnTriggerStay(Collider other)
    {
        bool infectious = false;

        if (m_health)
        {
            if (m_health.IsDead())
            {
                return; // if dead you can't attack;
            }
            infectious = m_health.IsInfected();
        }
        if (Tags.IsDestructible(other.gameObject) || Tags.IsHuman(other.gameObject))
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
                        otherHealthScript.ApplyDamage(m_attackDamage, infectious);
                        m_nextAttack = Time.time + m_attackDelay;
                        Debug.Log(name + " attacked " + other.gameObject.name + " for " + m_attackDamage + " damage.");
                    }
                }
            }
        }

    }
}
