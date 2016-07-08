using UnityEngine;
using System.Collections;

/// <summary>
///  Code Author: Reece Howe
///  Project: Horde
///  Engine: Unity
///  Platform: Mobile and PC
///  Notes: Standard Health script for all gameObjects
///  Status: Work In Progress
/// </summary>

public class Health : MonoBehaviour {

    public int m_maxHealth = 100;
    public int m_health = 100;
    public bool m_healToFullOnStart = true;
    public bool m_vulnerable = true;
    public bool m_infected = false;
    private bool m_canBeInfected = false;

    // Use this for initialization
    void Start()
    {
        if (m_healToFullOnStart)
        {
            m_health = m_maxHealth;
        }
        m_infected = false;
        m_canBeInfected = Tags.CanbeInfected(gameObject);
    }

    //// Update is called once per frame
    //void Update () {

    //}

    //public void ApplyDamage(int damage)
    //{
    //    if (m_vulnerable)
    //    {
    //        m_health -= (Mathf.Abs(damage));
    //    }
    //}

    public void ApplyDamage(int damage, bool infectious = false)
    {
        if (m_vulnerable)
        {
            m_health -= (Mathf.Abs(damage));
            if (infectious && m_canBeInfected)
            {
                m_infected = infectious;
            }
        }
    }

    public void RecoverHealth(int health)
    {
        m_health += (Mathf.Abs(health));
        m_health = Mathf.Min(m_health, m_maxHealth); // cap health
    }

    public bool IsDead()
    {
        return m_health < 0;
    }

    public bool IsDamaged()
    {
        return m_health < m_maxHealth;
    }


}
