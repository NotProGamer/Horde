using UnityEngine;
using System.Collections;

public class AbnormalAbility : MonoBehaviour {

    private Screamer m_screamerScript = null;
    private ZombieBrain m_zombieBrainScript = null;

    public float m_deactivationDelay = 30.0f;
    private float m_deactivationTimer = 0f;

    //public bool m_testAbonormalAbility = false;
    private bool m_abonormalAbilityActivated = false;
    void Awake()
    {
        if (gameObject.CompareTag(Labels.Tags.ZombieScreamer))
        {
            m_screamerScript = GetComponent<Screamer>();
            if (m_screamerScript == null)
            {
                Debug.Log("Screamer Script not included");
            }
        }


        m_zombieBrainScript = GetComponent<ZombieBrain>();
        if (m_zombieBrainScript == null)
        {
            Debug.Log("ZombieBrain Script not included");
        }

    }

    // Use this for initialization
    void Start ()
    {
	
	}
    
	// Update is called once per frame
	void Update ()
    {
        //if (m_testAbonormalAbility)
        //{
        //    Activate();
        //}
        //else
        //{
        //    Deactivate();
        //}


        if (m_abonormalAbilityActivated == true 
            && m_deactivationTimer < Time.time)
        {
            Deactivate();
        }




    }

    public void Activate()
    {
        
        if (gameObject.CompareTag(Labels.Tags.ZombieScreamer))
        {
            if (m_screamerScript)
            {
                m_screamerScript.m_screaming = true;
                m_abonormalAbilityActivated = true;
            }
        }
        else if (gameObject.CompareTag(Labels.Tags.ZombieLittleGirl))
        {
            if (m_zombieBrainScript)
            {
                m_zombieBrainScript.m_becomeAggressive = true;
                m_abonormalAbilityActivated = true;
            }

        }
        else if (gameObject.CompareTag(Labels.Tags.ZombieGlutton))
        {
            if (m_zombieBrainScript)
            {
                m_zombieBrainScript.m_gluttonSettings.m_vomitToxin = true;
                m_abonormalAbilityActivated = true;
            }

        }
        else if (gameObject.CompareTag(Labels.Tags.ZombieDictator))
        {
            if (m_zombieBrainScript)
            {
                m_zombieBrainScript.m_giveSpeedBoost = true;
                m_abonormalAbilityActivated = true;
            }
        }
        else
        {
            Debug.Log("Unknown Abnormal Activation");
        }

        m_deactivationTimer = Time.time + m_deactivationDelay;
    }

    public void Deactivate()
    {
        if (gameObject.CompareTag(Labels.Tags.ZombieScreamer))
        {
            if (m_screamerScript)
            {
                m_screamerScript.m_screaming = false;
                m_abonormalAbilityActivated = false;
            }
        }
        else if (gameObject.CompareTag(Labels.Tags.ZombieLittleGirl))
        {
            if (m_zombieBrainScript)
            {
                m_zombieBrainScript.m_becomeAggressive = false;
                m_abonormalAbilityActivated = false;
            }
        }
        else if (gameObject.CompareTag(Labels.Tags.ZombieGlutton))
        {
            if (m_zombieBrainScript)
            {
                m_zombieBrainScript.m_gluttonSettings.m_vomitToxin = false;
                m_abonormalAbilityActivated = false;
            }
        }
        else if (gameObject.CompareTag(Labels.Tags.ZombieDictator))
        {
            if (m_zombieBrainScript)
            {
                m_zombieBrainScript.m_giveSpeedBoost = false;
                m_abonormalAbilityActivated = false;
            }

        }
        else
        {
            Debug.Log("Unknown Abnormal Deactivation");
        }

    }
}
