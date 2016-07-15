using UnityEngine;
using System.Collections;
/// <summary>
///  Code Author: Reece Howe
///  Project: Horde
///  Engine: Unity
///  Platform: Mobile and PC
///  Notes: DeadRising Script - This script manages a timer that replaces dead humans with zombies
///  Status: Complete
/// </summary>
/// 
public class DeadRising : MonoBehaviour {

    public float m_riseDelay = 3.0f;
    private float m_timeToRise = 0.0f;
    private bool m_turned = false;
    private bool m_rising = false;
    private CharacterPool m_characterPool = null;

    private Health m_health = null;
    private InfectionStatus m_infectionStatus = null;


    void OnDisable()
    {
        m_timeToRise = 0.0f;
        m_turned = false;
        m_rising = false;
    }


    void Awake()
    {
        GameObject gameController = GameObject.FindGameObjectWithTag(Tags.GameController);

        if (gameController)
        {
            m_characterPool = gameController.GetComponent<CharacterPool>();
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

 //   // Use this for initialization
 //   void Start () {}
	
	// Update is called once per frame
	void Update () {
        if (!m_turned)
        {
            if (m_health && m_infectionStatus)
            {
                if (m_health.IsDead() && m_infectionStatus.IsInfected())
                {
                    SetRiseTimer();
                    m_turned = true;
                }
            }
        }

        if (m_rising)
        {
            if (Time.deltaTime > m_timeToRise)
            {
                Rise();
            }
        }



    }

    public void SetRiseTimer()
    {
        m_rising = true;
        m_timeToRise = m_riseDelay;
    }

    void Rise()
    {
        if (m_characterPool)
        {
            if (m_health)
            {
                if (!m_health.IsDevoured())
                {
                    m_characterPool.RequestZombieAtPosition(transform.position);
                }
            }
            gameObject.SetActive(false);
        }
    }

    public bool IsTurned()
    {
        return m_turned;
    }
}
