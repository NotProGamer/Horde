using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CameraMovement : MonoBehaviour
{
    private UserController m_userController = null;
    private bool m_validDragPosition = false;
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
    private Vector3 m_dragStartPosition; // x , y , z(normally zero)
    private Vector3 m_rigStartPosition;
    private Vector3 m_grabWorldPosition;
    public float m_smoothing = 5f;


    void Awake()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(Labels.Tags.GameController);
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
        m_rigStartPosition = transform.position;
        m_layerMask = LayerMask.GetMask("Ground");
    }




    // Update is called once per frame
    void Update()
    {

        if (m_userController != null)
        {
            if (m_userController.m_state == UserControllerState.Touched)
            {
                StartMove();
            }
            else if (m_userController.m_state == UserControllerState.Dragging)
            {

                if (m_state == State.Idle)
                {
                    if (m_validDragPosition)
                    {
                        m_state = State.Moving;
                    }
                }
                else if (m_state == State.Moving)
                {
                    Move();
                }
                else if (m_userController.m_state == UserControllerState.Dragged)
                {
                    FinishMove();
                    m_validDragPosition = false;
                }
            }
        }


        //RaycastHit hit;
        //Ray ray;
        //if (Input.GetMouseButtonDown(0))
        //{
        //    m_dragStartPosition = Input.mousePosition;
        //    m_rigStartPosition = transform.position;
        //    ray = Camera.main.ScreenPointToRay(m_dragStartPosition);
        //    if (Physics.Raycast(ray, out hit, 1000, m_layerMask))
        //    {
        //        m_state = State.Moving;
        //        m_grabWorldPosition = hit.point;
        //        m_grabWorldPosition.y = m_rigStartPosition.y;
        //    }
        //    //if (Physics.Raycast(ray, out hit, 10000))
        //    //{
        //    //    // if you have not selected a rigidbody, then set camera drag start position
        //    //    if (hit.rigidbody == null)
        //    //    {
        //    //        startPosition = new Vector3(hit.point.x, rigOriginY, hit.point.z);
        //    //        m_state = State.Moving;
        //    //    }
        //    //}
        //}
        //else if (Input.GetMouseButton(0))
        //{
        //    if (m_state == State.Moving)
        //    {
        //        Vector3 currentRigPosition = transform.position;
        //        transform.position = m_rigStartPosition;
        //        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //        if (Physics.Raycast(ray, out hit, 1000, m_layerMask))
        //        {
        //            Vector3 dragOffset = hit.point;
        //            dragOffset.y = m_rigStartPosition.y;
        //            Vector3 direction = dragOffset - m_grabWorldPosition;
        //            //clamp movement to playArea
        //            Vector3 proposedPosition = transform.position - direction;
        //            Vector3 proposedScreenPosition = Camera.main.WorldToScreenPoint(proposedPosition);
        //            ray = Camera.main.ScreenPointToRay(proposedScreenPosition);
        //            if (Physics.Raycast(ray, out hit, 1000, m_layerMask))
        //            {
        //                transform.position = proposedPosition;
        //            }
        //            else
        //            {
        //                transform.position = currentRigPosition;
        //            }
        //            //transform.position = Vector3.Lerp(transform.position, transform.position - direction, m_smoothing * Time.deltaTime);
        //        }
        //        else
        //        {
        //            transform.position = currentRigPosition;
        //        }
        //    }
        //    //if (m_state == State.Moving)
        //    //{
        //    //    Vector3 test2 = Camera.main.ScreenToWorldPoint(test);
        //    //    test2.y = rigOriginY;
        //    //    transform.position += test2 - startPosition;
        //    //}
        //    //Vector3 cameraMovement = new Vector3();
        //    //transform.position = startPosition;
        //    //ray = Camera.main.ScreenPointToRay(test);
        //    //if (Input.GetMouseButton(0))
        //    //{
        //    //    ////transform.position = startPosition;
        //    //    ////m_state = State.Moving;
        //    //    if (Physics.Raycast(ray, out hit, 10000))
        //    //    {
        //    //        cameraMovement = new Vector3(hit.point.x, rigOriginY, hit.point.z);
        //    //    }
        //    //    //transform.position = startPosition - cameraMovement;// - cameraMovement;
        //    //    lerpToPosition = cameraMovement - startPosition;
        //    //    Debug.Log(lerpToPosition);
        //    //    transform.position = transform.position + startPosition - lerpToPosition;
        //    //}
        //    //if (Input.GetMouseButtonUp(0))
        //    //{
        //    //    m_state = State.Idle;
        //    //}
        //}
        //else if (Input.GetMouseButtonUp(0))
        //{
        //    m_state = State.Idle;
        //}
    }
    private void StartMove()
    {
        RaycastHit hit;
        Ray ray;

        m_dragStartPosition = m_userController.GetDragStart();
        m_rigStartPosition = transform.position;

        ray = Camera.main.ScreenPointToRay(m_dragStartPosition);
        if (Physics.Raycast(ray, out hit, 1000, m_layerMask))
        {
            //m_state = State.Moving;
            m_validDragPosition = true;
            m_grabWorldPosition = hit.point;
            m_grabWorldPosition.y = m_rigStartPosition.y;
        }

    }

    private void FinishMove()
    {
        m_state = State.Idle;
    }


    private void Move()
    {

        RaycastHit hit;
        Ray ray;

        // reset transform
        // raycast to get drag offset
        // calculate proposed transform position
        // clamp proposed position within play area
        // if valid, move camera, else set position to last transform position

        bool validMove = false;
        Vector3 currentPosition = transform.position;
        Vector3 proposedPosition = Vector3.zero;

        transform.position = m_rigStartPosition;

        ray = Camera.main.ScreenPointToRay(m_userController.m_lastDrag.m_end);
        if (Physics.Raycast(ray, out hit, 1000, m_layerMask))
        {

            Vector3 worldPosition = hit.point;
            worldPosition.y = m_rigStartPosition.y;
            Vector3 dragOffset = worldPosition - m_grabWorldPosition;

            proposedPosition = transform.position - dragOffset;
            //clamp movement to playArea
            Vector3 proposedScreenPosition = Camera.main.WorldToScreenPoint(proposedPosition);
            ray = Camera.main.ScreenPointToRay(proposedScreenPosition);
            if (Physics.Raycast(ray, out hit, 1000, m_layerMask))
            {
                validMove = true;
            }
        }

        if (validMove)
        {
            transform.position = proposedPosition;

        }
        else
        {
            transform.position = currentPosition;
        }
    }
}

    