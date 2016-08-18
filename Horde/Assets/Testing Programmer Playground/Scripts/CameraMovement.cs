using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraMovement : MonoBehaviour
{
    private UserController m_userController = null;

    // note create input controller

    // click to generate noise

    // click and drag to move camera

    public enum State
    {
        Idle,
        Moving
    }
    public State m_state = State.Idle;

    private LayerMask m_layerMask;
    private Vector3 m_startMousePosition; // x , y , z(normally zero)
    private Vector3 m_rigOrigin;
    private Vector3 m_grabWorldPosition;
    public float m_smoothing = 5f;


    void Awake()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(Tags.GameController);
        if (obj)
        {
            m_userController = obj.GetComponent<UserController>();
            if (m_userController == null)
            {
                Debug.Log("User Controller not included");
            }
        }
        
    }


    // Use this for initialization
    void Start()
    {
        //rigOriginY = transform.position.y;
        m_rigOrigin = transform.position;
        m_layerMask = LayerMask.GetMask("Ground");
    }


    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;
        Ray ray;

        //switch (m_userController.m_interfaceState)
        //{
        //    case UserController.UserControllerState.Touched:
        //        break;
        //    case UserController.UserControllerState.Touching:
        //        break;
        //    case UserController.UserControllerState.Tapped:
        //        break;
        //    case UserController.UserControllerState.Dragging:
        //        break;
        //    case UserController.UserControllerState.Dragged:
        //        break;
        //    case UserController.UserControllerState.Idle:
        //        break;
        //    default:
        //        break;
        //}



        if (Input.GetMouseButtonDown(0))
        {

            m_startMousePosition = Input.mousePosition;
            m_rigOrigin = transform.position;

            ray = Camera.main.ScreenPointToRay(m_startMousePosition);
            if (Physics.Raycast(ray, out hit, 1000, m_layerMask))
            {
                m_state = State.Moving;
                m_grabWorldPosition = hit.point;
                m_grabWorldPosition.y = m_rigOrigin.y;
            }

            //if (Physics.Raycast(ray, out hit, 10000))
            //{
            //    // if you have not selected a rigidbody, then set camera drag start position
            //    if (hit.rigidbody == null)
            //    {
            //        startPosition = new Vector3(hit.point.x, rigOriginY, hit.point.z);
            //        m_state = State.Moving;
            //    }
            //}
        }
        else if (Input.GetMouseButton(0))
        {
            if (m_state == State.Moving)
            {
                Vector3 currentRigPosition = transform.position;
                transform.position = m_rigOrigin;
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 1000, m_layerMask))
                {
                    Vector3 dragOffset = hit.point;
                    dragOffset.y = m_rigOrigin.y;
                    Vector3 direction = dragOffset - m_grabWorldPosition;

                    //clamp movement to playArea
                    Vector3 proposedPosition = transform.position - direction;

                    Vector3 proposedScreenPosition = Camera.main.WorldToScreenPoint(proposedPosition);
                    ray = Camera.main.ScreenPointToRay(proposedScreenPosition);
                    if (Physics.Raycast(ray, out hit, 1000, m_layerMask))
                    {
                        transform.position = proposedPosition;
                    }
                    else
                    {
                        transform.position = currentRigPosition;
                    }
                    //transform.position = Vector3.Lerp(transform.position, transform.position - direction, m_smoothing * Time.deltaTime);
                }
                else
                {
                    transform.position = currentRigPosition;
                }
            }




            //if (m_state == State.Moving)
            //{
            //    Vector3 test2 = Camera.main.ScreenToWorldPoint(test);
            //    test2.y = rigOriginY;
            //    transform.position += test2 - startPosition;
            //}



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


    