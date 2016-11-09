using UnityEngine;
using System.Collections;

public class BeaconMovement : MonoBehaviour {

    private UserController m_userController = null;

    private LayerMask m_layerMask;

    private bool m_validDragPosition = false;
    private Vector3 m_rigStartPosition;
    private Vector3 m_dragStartPosition; // x , y , z(normally zero)
    private Vector3 m_grabWorldPosition;

    private Noise m_noise = null;
    private GameObject m_noiseEffectObj = null;
    private NoiseVisualization noiseVisScript = null;

    public enum State
    {
        Idle,
        Moving
    }
    public State m_state = State.Idle;

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
        m_noiseEffectObj = gameObject.transform.FindChild("NoiseEffect").gameObject;
        if (m_noiseEffectObj)
        {
            noiseVisScript = m_noiseEffectObj.GetComponent<NoiseVisualization>();
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
            if (m_userController.m_touchedObject == null || m_userController.m_touchedObject.m_gameObject != gameObject)
            {
                if (m_state == State.Moving)
                {
                    m_state = State.Idle;
                }
            }

            // noise management
            if (m_noise != null)
            {
                if (m_state == State.Moving)
                {
                    m_noise.m_position = transform.position;
                    m_noise.ResetExpiry();
                }
                else if (m_state == State.Idle)
                {
                    // stuff
                }


                if (m_noise.IsExpired())
                {
                    transform.SetParent(m_parent);
                    gameObject.SetActive(false);
                }
            }
            else
            {
                transform.SetParent(m_parent);
                gameObject.SetActive(false);
            }

            if (m_userController.m_state != UserControllerState.Idle)
            {
                if (m_userController.m_touchedObject.m_gameObject != gameObject)
                {
                    if (m_state == State.Moving)
                    {
                        m_state = State.Idle;
                    } 
                    return; // early exit
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
            else
            {
                m_state = State.Idle;
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

            proposedPosition = transform.position + dragOffset;
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

    public void SetNoise(Noise noise)
    {
        m_noise = noise;
        if (noiseVisScript != null)
        {
            noiseVisScript.SetNoise(noise);
        }
    }

    public Transform m_parent = null;

    void OnEnable()
    {
        m_parent = transform.parent;
        transform.SetParent(null);
    }

    void OnDisable()
    {
        
    }
}
