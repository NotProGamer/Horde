using UnityEngine;
using System.Collections;

public class ZombieCleanUp : MonoBehaviour {

    private CharacterPool m_characterPool = null;
    private Health m_health = null;
    public float m_cleanUpDelay = 3.0f;
    public float m_cleanUpTime = 0.0f;


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

        if (m_characterPool == null)
        {
            Debug.Log("CharacterPool not included on GameController");
        }

        m_health = GetComponent<Health>();
        if (m_health == null)
        {
            Debug.Log("Health not included");
        }

    }


    private bool m_timeToCleanUp = false;
 //   // Use this for initialization
 //   void Start ()
 //   {
	
	//}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_health)
        {
            if (m_health.IsDead())
            {
                if (!m_timeToCleanUp)
                {
                    m_timeToCleanUp = true;
                    m_cleanUpTime = Time.time + m_cleanUpDelay;
                }
                else if (Time.time >= m_cleanUpTime)
                {
                    CleanUp();
                }
            }
        }

    }

    void CleanUp()
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        m_timeToCleanUp = false;
    }
}
