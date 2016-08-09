using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{

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
    void Start()
    {
        rigOriginY = transform.position.y;

    }

    private Vector3 startPosition;
    private Vector3 lerpToPosition;
    private float rigOriginY;

    // Update is called once per frame
    void Update()
    {

        Vector3 test = Input.mousePosition;


        RaycastHit hit;
        Ray ray;

        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(test);
            if (Physics.Raycast(ray, out hit, 10000))
            {
                // if you have not selected a rigidbody, then set camera drag start position
                if (hit.rigidbody == null)
                {
                    startPosition = new Vector3(hit.point.x, rigOriginY, hit.point.z);
                    m_state = State.Moving;
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (m_state == State.Moving)
            {
                Vector3 test2 = Camera.main.ScreenToWorldPoint(test);
                test2.y = rigOriginY;
                transform.position += test2 - startPosition;
            }
            //Vector3 cameraMovement = new Vector3();
            //transform.position = startPosition;
            //ray = Camera.main.ScreenPointToRay(test);
            //if (Input.GetMouseButton(0))
            //{
            //    ////transform.position = startPosition;
            //    ////m_state = State.Moving;
            //    if (Physics.Raycast(ray, out hit, 10000))
            //    {
            //        cameraMovement = new Vector3(hit.point.x, rigOriginY, hit.point.z);
            //    }

            //    //transform.position = startPosition - cameraMovement;// - cameraMovement;

            //    lerpToPosition = cameraMovement - startPosition;
            //    Debug.Log(lerpToPosition);
            //    transform.position = transform.position + startPosition - lerpToPosition;
            //}

            //if (Input.GetMouseButtonUp(0))
            //{
            //    m_state = State.Idle;

            //}
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_state = State.Idle;
        }
    }
}


    