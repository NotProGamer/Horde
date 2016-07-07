using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

    public int m_maxHealth = 100;
    public int m_health = 100;
    public bool m_healToFullOnStart = true;
    public bool m_vulnerable = true;

    // Use this for initialization
    void Start()
    {
        if (m_healToFullOnStart)
        {
            m_health = m_maxHealth;
        }
    }

    //// Update is called once per frame
    //void Update () {

    //}

    public void ApplyDamage(int damage)
    {
        if (m_vulnerable)
        {
            m_health -= (Mathf.Abs(damage));
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
