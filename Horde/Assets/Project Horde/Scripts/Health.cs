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
///  Created: 2016 08 19
/// </summary>


public class Health : MonoBehaviour {
    public int m_maxHealth = 100;
    public int m_health = 100;
    public bool m_healToFullOnStart = true;
    public bool m_vulnerable = true;
    public bool m_disableOnDeath = false;


    // Damage Effects
    public bool m_enableDamageEffects = false;
    private bool m_wasDamaged = true;
    public Color m_damageColor = Color.red;
    public List<MeshRenderer> m_meshRendererList = new List<MeshRenderer>();
    public List<SkinnedMeshRenderer> m_skinnedMeshRendererList = new List<SkinnedMeshRenderer>();


    private bool m_isDevourable = false;
    public bool m_extendTurnTimerIfBeingDevoured = true;
    private Reanimator m_reanimationScript = null; //private DeadRising m_deadRisingScript = null;


    void Awake()
    {
        m_reanimationScript = GetComponent<Reanimator>();
        if (m_reanimationScript == null)
        {
            if (Labels.Tags.IsHuman(gameObject))
            {
                Debug.Log("Reanimator Script not included");
            }
        }
    }

    void OnEnable()
    {
        InitialiseHealth();
    }

    // Use this for initialization
    void Start () {
        InitialiseHealth();
        m_isDevourable = Labels.Tags.IsDevourable(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_enableDamageEffects && m_wasDamaged)
        {
            DamageFlash();
            m_wasDamaged = false;
        }
        if (m_disableOnDeath && IsDead())
        {
            gameObject.SetActive(false);
        }
    }

    void InitialiseHealth()
    {
        if (m_healToFullOnStart)
        {
            m_health = m_maxHealth;
        }

    }

    public virtual void ApplyDamage(int damage)
    {
        if (m_vulnerable)
        {
            m_health -= (Mathf.Abs(damage));
            m_wasDamaged = true;

            if (m_soundsEnabled)
            {
                if (Labels.Tags.IsZombie(gameObject))
                {
                    SoundLibrary.PlaySound(gameObject, m_sounds.ZombieTakeDamage);
                }
                else if (Labels.Tags.IsHuman(gameObject))
                {
                    SoundLibrary.PlaySound(gameObject, m_sounds.HumanTakeDamage);
                }
                
            }
        }

    }

    public void ApplyHeal(int health)
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


    /// <summary>
    /// Eat Corpse to Gain Health
    /// Apply damage to corpse 
    /// </summary>
    /// <param name="pDamage"></param>
    /// <returns> health recovered. </returns>
    public int DevourCorpse(int pDamage)
    {
        int healthRecovered = 0;
        // check if is devourable AND dead AND not devoured AND not turned
        bool turned = false;
        if (m_reanimationScript)
        {
            turned = m_reanimationScript.IsTurned();
        }

        if (m_isDevourable && IsDead() && !IsDevoured() && !turned)
        {
            int damage = (Mathf.Abs(pDamage));
            healthRecovered = damage; // could change to only recover a portion of damage dealt
            m_health -= damage;
            // extend turn timer
            if (m_reanimationScript && m_extendTurnTimerIfBeingDevoured)
            {
                m_reanimationScript.StartReanimationTimer(); //m_turnTime = Time.time + m_turnDelay;
            }
        }
        return healthRecovered;
    }


 
    // Damage Effects
    void DamageFlash()
    {
        for (int i = 0; i < m_meshRendererList.Count; i++)
        {
            MeshRenderer art = null;
            art = m_meshRendererList[i];
            if (art == null)
            {
                Debug.Log("MeshRender set to null");
            }
            else
            {
                if (art.material.color != m_damageColor)
                {
                    StartCoroutine("MeshRendererFlash", art);
                }
            }
        }
        for (int i = 0; i < m_skinnedMeshRendererList.Count; i++)
        {
            SkinnedMeshRenderer art = null;
            art = m_skinnedMeshRendererList[i];
            if (art == null)
            {
                Debug.Log("MeshRender set to null");
            }
            else
            {
                if (art.material.color != m_damageColor)
                {
                    StartCoroutine("SkinnedMeshRendererFlash", art);
                }
            }
        }
    }

    //// Damage Effects
    //void DamageFlash()
    //{
    //    for (int i = 0; i < m_meshRendererList.Count; i++)
    //    {
    //        MeshRenderer art = null;
    //        art = m_meshRendererList[i];
    //        StartCoroutine("MeshRendererFlash", art);
    //    }
    //    for (int i = 0; i < m_skinnedMeshRendererList.Count; i++)
    //    {
    //        SkinnedMeshRenderer art = null;
    //        art = m_skinnedMeshRendererList[i];
    //        StartCoroutine("SkinnedMeshRendererFlash", art);
    //    }
    //}

    public float GetPercentageHealth()
    {
        return (float) m_maxHealth != 0 && m_health > 0 ? (float)m_health / (float)m_maxHealth : 0;
    }

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

    [System.Serializable]
    public class SoundsStrings
    {
        //public string Idle ="";
        public string ZombieTakeDamage = "ZombieTakeDamage";
        public string HumanTakeDamage = "HumanTakeDamage";
        //public string Investigate = "";
        //public string Devour = "ZombieDevour";
        //public string Chase = "";
        //public string GoToUserTap = "ZombieHearsUserTap";
        //public string Death = "ZombieDeath";
        //public string Reanimating = "ZombieReanimating";

    }
    public SoundsStrings m_sounds;
    public bool m_soundsEnabled = true;


}
