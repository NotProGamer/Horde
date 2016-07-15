using UnityEngine;
using System.Collections;
/// <summary>
///  Code Author: Reece Howe
///  Project: Horde
///  Engine: Unity
///  Platform: Mobile and PC
///  Notes: Infection Script - Units that can be infected should have this script
///  Status: Complete
/// </summary>
/// 
public class InfectionStatus : MonoBehaviour {

    private bool m_infected = false;
    private bool m_immune = false;


    void OnDisable()
    {
        // this should be trigger when an object is return to the object pool
        if (Tags.IsHuman(gameObject))
        {
            m_infected = false;
        }
        else if (Tags.IsZombie(gameObject))
        {
            m_infected = true;
        }
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool IsInfected()
    {
        return m_infected;
    }


    public void Infect()
    {
        if (!m_immune)
        {
            m_infected = true;
        }
    }

    public void Cure()
    {
        m_infected = false;
    }

    //bool IsImmune()
    //{
    //    return m_immune;
    //}

    //void BecomeImmune()
    //{
    //    m_immune = true;
    //}
}
