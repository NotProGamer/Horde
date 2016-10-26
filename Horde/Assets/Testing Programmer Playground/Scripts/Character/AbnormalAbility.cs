using UnityEngine;
using System.Collections;

public class AbnormalAbility : MonoBehaviour {

    private Screamer m_screamerScript = null;
    private ZombieBrain m_zombieBrainScript = null;

    public bool m_testAbonormalAbility = false;
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
        if (m_testAbonormalAbility)
        {
            Activate();
        }
        else
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
            }
        }
        else if (gameObject.CompareTag(Labels.Tags.ZombieLittleGirl))
        {
            if (m_zombieBrainScript)
            {
                m_zombieBrainScript.m_becomeAggressive = true;
            }

        }
        else if (gameObject.CompareTag(Labels.Tags.ZombieGlutton))
        {
            if (m_zombieBrainScript)
            {
                m_zombieBrainScript.m_gluttonSettings.m_vomitToxin = true;
            }

        }
        else if (gameObject.CompareTag(Labels.Tags.ZombieDictator))
        {
            if (m_zombieBrainScript)
            {
                m_zombieBrainScript.m_giveSpeedBoost = true;
            }
        }
        else
        {
            Debug.Log("Unknown Abnormal Activation");
        }
    }

    public void Deactivate()
    {
        if (gameObject.CompareTag(Labels.Tags.ZombieScreamer))
        {
            if (m_screamerScript)
            {
                m_screamerScript.m_screaming = false;
            }
        }
        else if (gameObject.CompareTag(Labels.Tags.ZombieLittleGirl))
        {
            if (m_zombieBrainScript)
            {
                m_zombieBrainScript.m_becomeAggressive = false;
            }
        }
        else if (gameObject.CompareTag(Labels.Tags.ZombieGlutton))
        {
            if (m_zombieBrainScript)
            {
                m_zombieBrainScript.m_gluttonSettings.m_vomitToxin = false;
            }
        }
        else if (gameObject.CompareTag(Labels.Tags.ZombieDictator))
        {
            if (m_zombieBrainScript)
            {
                m_zombieBrainScript.m_giveSpeedBoost = false;
            }

        }
        else
        {
            Debug.Log("Unknown Abnormal Deactivation");
        }

    }
}
