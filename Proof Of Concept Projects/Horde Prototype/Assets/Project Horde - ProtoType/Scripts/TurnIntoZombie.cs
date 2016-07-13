using UnityEngine;
using System.Collections;

public class TurnIntoZombie : MonoBehaviour {

    private CharacterPool m_characterPool = null;
    private Health m_health = null;

    void Awake()
    {
        m_health = GetComponent<Health>();
        if (m_health == null)
        {
            Debug.Log("Health not included");
        }

        GameObject m_gameController = GameObject.FindGameObjectWithTag(Tags.GameController);
        if (m_gameController)
        {
            m_characterPool = m_gameController.GetComponent<CharacterPool>();
        }
        else
        {
            Debug.Log("Unable to Find GameController");
        }

        if (m_characterPool = null)
        {
            Debug.Log("CharacterPool not included on GameController");
        }
        
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (m_health && m_characterPool)
        {
            if (m_health.IsTurned())
            {
                Vector3 position = transform.position;
                gameObject.SetActive(false);
                m_characterPool.RequestZombieAtPosition(position);
            }
        }
	}
}
