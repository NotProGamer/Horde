using UnityEngine;
using System.Collections;

public class GodZombieMovement : MonoBehaviour
{
    //GameController
    GameObject m_gameController;
    
    //Where zombies get their current destination
    GodDestinationManager m_destinationManager;

    //Check for input
    UserController m_userController;

    //Zombie Navmesh Agent
    NavMeshAgent m_agent;

    //Animator Controller
    public Animator m_animator;

    //Assigned Human Target
    public GameObject m_humanTarget = null;
    //Target Human Health
    Health m_targetHealth;

    Transform m_targetDestination;




	// Use this for initialization
	void Start ()
    {
        m_gameController = GameObject.FindGameObjectWithTag(Labels.Tags.GameController);

        m_destinationManager = m_gameController.GetComponent<GodDestinationManager>();

        if (!m_destinationManager)
        {
            Debug.Log("Could not find GodDestinationManager");
        }

        m_userController = m_gameController.GetComponent<UserController>();

        if (!m_userController)
        {
            Debug.Log("Could not find userController");
        }

        m_agent = GetComponent<NavMeshAgent>();


        m_animator.SetBool("Idle", true);

    }
	
	// Update is called once per frame
	void Update ()
    {
        //Move zombies to Destination if doing nothing else


        //Chase and attack human if zombies has a target


        //Continue to follow Destination if target dies


        //Move closer to Horde (large group of tighly clustered zombies) and then continue moving to Destination

        if (!m_humanTarget)
        {
            // m_agent.destination = m_destinationManager.zombieLure.position;
            m_agent.speed = 3;
            m_targetDestination = m_destinationManager.zombieLure;

        }

        if (m_humanTarget)
        {
            m_agent.speed = 5;
            m_targetDestination = m_humanTarget.transform;
            transform.LookAt(m_humanTarget.transform);
            m_targetHealth = m_humanTarget.GetComponent<Health>();


            if (m_targetHealth.IsDead())
            {
                m_humanTarget = null;
            }

            //float distanceFromTarget = Vector3.Distance(transform.position, m_humanTarget.transform.position);

            //if (distanceFromTarget < 2)
            //{
            //    Vector3 faceTarget = new Vector3(m_humanTarget.transform.position.x, lookHeight, m_humanTarget.transform.position.z);
            //    transform.LookAt(faceTarget);
            //}

        }


        m_agent.destination = m_targetDestination.position;



        m_animator.SetFloat("Movement", m_agent.velocity.magnitude);
        if (m_agent.velocity != new Vector3(0, 0, 0))
        {
            m_animator.SetBool("Moving", true);
            m_animator.SetBool("Idle", false);
        }
        else
        {
            m_animator.SetBool("Moving", false);
            m_animator.SetBool("Idle", true);
        }

        
	
	}
}
