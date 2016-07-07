using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Barricade : MonoBehaviour {

    private Health m_healthScript;
    private List<GameObject> m_zombies;
    //private List<GameObject> m_humans;
    public int m_zombieBarricadeRating = 5;
    public GameObject m_gate;
    public GameObject m_northLink;
    public GameObject m_soutLink;
    
    // Use this for initialization
    void Start ()
    {
        m_zombies = new List<GameObject>();
        m_healthScript = GetComponent<Health>();
        if (m_healthScript)
        {
            // make barricade invulnerable
            m_healthScript.m_vulnerable = false;
        }
        else
        {
            Debug.Log("Health script not included.");
        }
        SetGateDestroyed(false);
    }

    // Update is called once per frame
    void Update ()
    {
        if (m_healthScript)
        {
            if (m_healthScript.IsDead())
            {
                SetGateDestroyed(true);
            }
            else
            {
                SetGateDestroyed(false);
            }
        }
    }

    void SetGateDestroyed(bool destroyed)
    {
        if (m_gate.activeSelf == destroyed)
        {
            m_gate.SetActive(!destroyed);
            if (m_northLink && m_soutLink)
            {
                m_northLink.SetActive(destroyed);
                m_soutLink.SetActive(destroyed);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (Tags.IsZombie(other.gameObject))
        {
            m_zombies.Add(other.gameObject);
            // if zombie count exceedes barricade rating, barricade becomes vulnerable
            if (m_zombies.Count >= m_zombieBarricadeRating)
            {
                if (m_healthScript) m_healthScript.m_vulnerable = true;
            }
        }
        //if (Tags.IsHuman(other.gameObject))
        //{
        //    m_humans.Add(other.gameObject);
        //}
    }
    void OnTriggerExit(Collider other)
    {
        if (Tags.IsZombie(other.gameObject))
        {
            m_zombies.Remove(other.gameObject);
            // if zombie count falls below the barricade rating, barricade becomes invulnerable
            if (m_zombies.Count < m_zombieBarricadeRating)
            {
                if (m_healthScript) m_healthScript.m_vulnerable = false;
            }
        }
        //if (Tags.IsHuman(other.gameObject))
        //{
        //    m_humans.Remove(other.gameObject);
        //}

    }

}
