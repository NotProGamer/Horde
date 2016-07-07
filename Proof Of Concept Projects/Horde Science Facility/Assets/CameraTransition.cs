using UnityEngine;
using System.Collections;

public class CameraTransition : MonoBehaviour {


    public GameObject camera;
    Animator controller;


    string location;



	// Use this for initialization
	void Start ()
    {
        controller = camera.GetComponent<Animator>();
        location = "main";
	
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (location == "main")
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                controller.SetTrigger("MainToLab");
                location = "lab";
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                controller.SetTrigger("MainToBar");
                location = "bar";
            }
        }


        if (location == "lab")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                controller.SetTrigger("LabToMain");
                location = "main";
            }
        }

        if (location == "bar")
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                controller.SetTrigger("BarToMain");
                location = "main";
            }
        }
    }
}
