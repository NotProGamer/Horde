using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    //private bool m_infected = false;
    //private bool m_canBeInfected = false;
    private bool m_isDevourable = false;
    //private bool m_turned = false;
    //public float m_turnDelay = 3.0f;
    //private float m_turnTime = 0.0f;
    public bool m_extendTurnTimerIfBeingDevoured = true;

    public Color m_damageColor = Color.red; 
    public List<MeshRenderer> m_meshRendererList = new List<MeshRenderer>();
    public List<SkinnedMeshRenderer> m_skinnedMeshRendererList = new List<SkinnedMeshRenderer>();
    //public MeshRenderer m_art = null;

    private DeadRising m_deadRisingScript = null;

    void Awake()
    {
        m_deadRisingScript = GetComponent<DeadRising>();
        if (m_deadRisingScript == null)
        {
            //Debug.Log("DeadRising not included");
        }

    }
    void OnEnable()
    {
        if (m_healToFullOnStart)
        {
            m_health = m_maxHealth;
        }
        //m_infected = Tags.IsZombie(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        if (m_healToFullOnStart)
        {
            m_health = m_maxHealth;
        }
        //m_infected = Tags.IsZombie(gameObject);
        //m_canBeInfected = Tags.CanBeInfected(gameObject);
        m_isDevourable = Tags.IsDevourable(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //// if dead and infected and turnTime expired then turn human
        //if (IsDead() & m_infected && Time.time > m_turnTime)
        //{
        //    m_turned = true;
        //}
    }

    public void ApplyDamage(int damage)
    {
        if (m_vulnerable)
        {
            m_health -= (Mathf.Abs(damage));
            DamageFlash();
        }
    }

    //public void ApplyDamage(int damage, bool infectious = false)
    //{
    //    if (m_vulnerable)
    //    {
    //        m_health -= (Mathf.Abs(damage));
    //        if (infectious && m_canBeInfected)
    //        {
    //            m_infected = infectious;
    //        }
    //         if dead and infected start turn countdown
    //        if (IsDead() & m_infected)
    //        {
    //             set turn timer
    //            m_turnTime = Time.time + m_turnDelay;
    //        }
    //    }
    //}

    void DamageFlash()
    {
        for (int i = 0; i < m_meshRendererList.Count; i++)
        {
            MeshRenderer art = null;
            art = m_meshRendererList[i];
            StartCoroutine("MeshRendererFlash", art);
        }
        for (int i = 0; i < m_meshRendererList.Count; i++)
        {
            MeshRenderer art = null;
            art = m_skinnedMeshRendererList[i];
            StartCoroutine("SkinnedMeshRendererFlash", art);
        }
        //StartCoroutine("collideFlash", );
    }
    //IEnumerator collideFlash()
    //{
    //    if (m_art)
    //    {
    //        Material m = m_art.material;
    //        Color32 c = m_art.material.color;
    //        m_art.material = null;
    //        m_art.material.color = m_damageColor;
    //        yield return new WaitForSeconds(0.1f);
    //        m_art.material = m;
    //        m_art.material.color = c;
    //        //Debug.Log("test");
    //    }
    //}

    IEnumerator MeshRendererFlash(MeshRenderer art)
    {
        if (art)
        {
            Material m = art.material;
            Color32 c = art.material.color;
            art.material = null;
            art.material.color = m_damageColor;
            yield return new WaitForSeconds(0.1f);
            art.material = m;
            art.material.color = c;
            //Debug.Log("test");
        }
    }

    IEnumerator SkinnedMeshRendererFlash(SkinnedMeshRenderer art)
    {
        if (art)
        {
            Material m = art.material;
            Color32 c = art.material.color;
            art.material = null;
            art.material.color = m_damageColor;
            yield return new WaitForSeconds(0.1f);
            art.material = m;
            art.material.color = c;
            //Debug.Log("test");
        }
    }



    public void RecoverHealth(int health)
    {
        m_health += (Mathf.Abs(health));
        m_health = Mathf.Min(m_health, m_maxHealth); // cap health
    }

    public bool IsDead()
    {
        return m_health <= 0;
    }

    public bool IsDamaged()
    {
        return m_health < m_maxHealth;
    }

    public bool IsDevoured()
    {
        return m_health <= -m_maxHealth;
    }

    //public bool IsTurned()
    //{
    //    return m_turned;
    //}

    //public bool IsInfected()
    //{
    //    return m_infected;
    //}

    /// <summary>
    /// Eat Corpse to Gain Health
    /// Apply damage to corpse 
    /// </summary>
    /// <param name="pDamage"></param>
    /// <returns> health recovered. </returns>
    public int Devour(int pDamage)
    {
        if (m_isDevourable)
        {

        }
        int healthRecovered = 0;
        // check if is devourable AND dead AND not devoured AND not turned
        bool turned = false;
        if (m_deadRisingScript)
        {
            turned = m_deadRisingScript.IsTurned();
        }

        if (m_isDevourable && IsDead() && !IsDevoured() && !turned)
        {
            int damage = (Mathf.Abs(pDamage));
            healthRecovered = damage; // could change to only recover a portion of damage dealt
            m_health -= damage;
            // extend turn timer
            if (m_extendTurnTimerIfBeingDevoured)
            {
                m_deadRisingScript.SetRiseTimer(); //m_turnTime = Time.time + m_turnDelay;
            }
        }
        return healthRecovered;
    }


}
