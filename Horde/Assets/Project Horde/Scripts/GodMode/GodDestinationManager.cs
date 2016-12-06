using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GodDestinationManager : MonoBehaviour
{
    //Destination for all your zombies to move towards
    public Transform zombieDestination = null;



    UserController m_userController;

    public Transform zombieLure;


    //Variables for targetting humans
    RaycastHit hit;
    //Range on raycast
    float range = 1000;

    //List of humans to attack 
    public List<GameObject> humanTargets = new List<GameObject>();

    public List<GameObject> zombieList = new List<GameObject>();

    


    // Use this for initialization
    void Start ()
    {
        m_userController = GetComponent<UserController>();

        if (!m_userController)
        {
            Debug.Log("userController not included");
        }

        if (!zombieLure)
        {
            Debug.Log("zombieLure not included");
        }


        zombieList.AddRange(GameObject.FindGameObjectsWithTag("Zombie"));
    }
	
	// Update is called once per frame
	void Update ()
    {

        //Clicking on a human sets them as a target with priority 1
        //Clicking on them multiple times increases the priority to a cap

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Clicked");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast (ray, out hit, range))
            {
                if (hit.collider.gameObject.tag == "Human")
                {
                    zombieList.Clear();
                    zombieList.AddRange(GameObject.FindGameObjectsWithTag("Zombie"));
                    //Set human as target
                    Debug.Log("Human selected");
                    AssignZombieToTarget(hit);
                }

                if (hit.collider.gameObject.tag == "Terrain")
                {
                    Debug.Log("Moving lure");
                    zombieLure.position = hit.point;
                }
            }
        }
    }


    void AssignZombieToTarget(RaycastHit hit)
    {
        //Look as list of tapped humans. Put out a request to number of zombies based on priority

        //Find closest zombie to human that is in sight range and set their target to the human

        //Remove human from list if it dies

        GodZombieMovement zombieCheck = null;

        bool targetAssigned = false;

        for (int i = 0; i < zombieList.Count; i++)
        {
            zombieCheck = zombieList[i].GetComponent<GodZombieMovement>();

            if (!zombieCheck.m_humanTarget && targetAssigned == false)
            {
                targetAssigned = true;
                zombieCheck.m_humanTarget = hit.collider.gameObject;
            }
        }
    }
}
