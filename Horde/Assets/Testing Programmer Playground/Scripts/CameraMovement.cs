using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    // note create input controller

    // click to generate noise

    // click and drag to move camera

    public enum State
    {
        Idle,
        Moving
    }
    public State m_state = State.Idle;

	// Use this for initialization
	void Start () {
	
	}

    private Vector3 startPosition;


	// Update is called once per frame
	void Update ()
    {

        Vector3 test = Input.mousePosition;
        

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(test);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, 10000))
            {
                // if you have not selected a rigidbody, then set camera drag start position
                if (hit.rigidbody == null)
                {
                    startPosition = hit.point;
                    m_state = State.Moving;
                }
            }
        }
        Vector3 cameraMovement = new Vector3();

        if (Input.GetMouseButton(0))
        {
            transform.position = startPosition;
            //m_state = State.Moving;
            if (Physics.Raycast(ray, out hit, 10000))
            {
                cameraMovement = hit.point;
            }
            
            transform.position += cameraMovement - startPosition;// - cameraMovement;
        }

        if (Input.GetMouseButtonUp(0))
        {
            m_state = State.Idle;
        }



    }
}
