using UnityEngine;
using System.Collections;

public class CameraTargetMovement : MonoBehaviour {

    private UserController m_userController = null;
    private bool m_validDragPosition = false;

    public enum State
    {
        Idle,
        Moving
    }
    public State m_state = State.Idle;

    private LayerMask m_layerMask;
    private Vector3 m_rigStartPosition;
    private Vector3 m_dragStartPosition; // x , y , z(normally zero)
    private Vector3 m_grabWorldPosition;

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
    void Start ()
    {
        m_layerMask = LayerMask.GetMask("Ground");
        m_rigStartPosition = transform.position;
    }

    // Update is called once per frame
    void Update ()
    {
        if (m_userController != null)
        {
            if (m_userController.m_state != UserControllerState.Idle)
            {
                if (m_userController.m_touchedObject.m_gameObject != null)
                {
                    if (m_userController.m_touchedObject.m_gameObject.CompareTag(Labels.Tags.Beacon))
                    {
                        return; // early exit
                    }
                }
            }

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
