using UnityEngine;
using System.Collections;

public class HealthCondition : MonoBehaviour {

    private bool m_infected = false;
    private bool m_immune = false;


    // Use this for initialization
    void Start ()
    {
        InitialiseCondition();
    }

    void OnDisable()
    {
        InitialiseCondition();
    }

	//// Update is called once per frame
	//void Update ()    {	}

    void InitialiseCondition()
    {
        if (Labels.Tags.IsHuman(gameObject))
        {
            m_infected = false;
        }
        else if (Labels.Tags.IsZombie(gameObject))
        {
            m_infected = true;
        }

    }

    public void Infect()
    {
        if (!m_immune)
        {
            m_infected = true;
        }
    }

    public bool IsInfected()
    {
        return m_infected;
    }

    public void Cure()
    {
        m_infected = false;
    }

    public bool IsImmune()
    {
        return m_immune;
    }

    public void BecomeImmune()
    {
        m_immune = true;
    }


}
