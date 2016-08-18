using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Button
{
    //public enum ButtonStates
    //{
    //    Up,
    //    Down,
    //}
    //private ButtonStates m_buttonState = ButtonStates.Up;
    public enum PressStates
    {
        WasPressed,
        Pressed,
        WasReleased,
        Released,
    }

    private PressStates m_pressState = PressStates.Released;
    public bool IsPressed()
    {
        return m_pressState == PressStates.Pressed /*|| WasPressed()*/;
    }
    public bool WasPressed()
    {
        return m_pressState == PressStates.WasPressed;
    }

    public bool IsReleased()
    {
        return m_pressState == PressStates.Released /*|| WasReleased()*/;
    }

    public bool WasReleased()
    {
        return m_pressState == PressStates.WasReleased;
    }
    public void Press()
    {
        m_pressState = PressStates.WasPressed;
    }
    public void Release()
    {
        m_pressState = PressStates.WasReleased;
    }
    public void Update()
    {
        switch (m_pressState)
        {
            case PressStates.WasPressed:
                m_pressState = PressStates.Pressed;
                break;
            case PressStates.WasReleased:
                m_pressState = PressStates.Released;
                break;
            default:
                break;
        }
    }
}




public class UserController : MonoBehaviour {
    public class Drag
    {
        public Drag(Vector2 start, Vector2 end)
        {
            m_start = start;
            m_end = end;
        }
        public Vector2 m_start = -Vector2.one;
        public Vector2 m_end = -Vector2.one;
    }

    public enum UserControllerState
    {
        Touched,
        Touching,
        Tapped,
        Dragging,
        Dragged,
        Idle, // Not Touching or Dragging
    }
    public UserControllerState m_interfaceState = UserControllerState.Idle;

    public Drag m_lastDrag = null;
    public Button m_touch;
    private Vector2 m_touchOrigin = -Vector2.one;
    private Vector2 m_touchEnd = -Vector2.one;
    public float m_mouseMovementThreshold = 1f;
    public float m_touchMovementThreshold = 1f;
    private float m_currentThreshold = 0f;


    // reference stop raycasting through UI
    // http://answers.unity3d.com/questions/1079066/how-can-i-prevent-my-raycast-from-passing-through.html

    private Collider m_selectedObject = null;

    //#if !MOBILE_INPUT
    //    // use keyboard and mouse conrtols
    //#else
    //    //mobile code
    //#endif



    // Use this for initialization
    void Start ()
    {
#if !MOBILE_INPUT
        m_currentThreshold = m_mouseMovementThreshold;
#else
        currentThreshold = touchMovementThreshold;
#endif

    }
	
	// Update is called once per frame
	void Update ()
    {
        // touch update
        m_touch.Update(); // This changes the buttons states from was to is

        if (!EventSystem.current.IsPointerOverGameObject(0))
        {
        }

#if !MOBILE_INPUT


        // use keyboard and mouse controls
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject(0))
            {
                m_touch.Press();
                m_touchOrigin = Input.mousePosition;
                m_touchEnd = -Vector2.one;
                m_lastDrag = null;
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (m_touch.IsPressed())
            {
                m_touchEnd = Input.mousePosition;
                if (m_lastDrag == null)
                {
                    if ((m_touchEnd - m_touchOrigin).magnitude > m_currentThreshold)
                    {
                        // Start Drag
                        m_lastDrag = new Drag(m_touchOrigin, m_touchEnd);
                    }
                }
                else
                {
                    // Update Drag
                    m_lastDrag.m_end = m_touchEnd;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0) && m_touchOrigin.x >= 0)
        {
            if (m_touch.IsPressed())
            {
                m_touch.Release();
                m_touchEnd = Input.mousePosition;

                // could consider using sqrMagnitude here
                if ((m_touchEnd - m_touchOrigin).magnitude > m_currentThreshold)
                {
                    //Drag
                    m_lastDrag = new Drag(m_touchOrigin, m_touchEnd);
                }
                else
                {
                    m_lastDrag = null;
                    //Tap
                }
                m_touchOrigin.x = -1;
            }
        }

#else
    //mobile code

        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.touches[0];
            if (myTouch.phase == TouchPhase.Began)
            {
                if (!EventSystem.current.IsPointerOverGameObject(0))
                {
                    m_touch.Press();
                    m_touchOrigin = myTouch.position;
                    m_touchEnd = -Vector2.one;
                    m_lastDrag = null;
                }
            }
            else if (myTouch.phase == TouchPhase.Moved)
            {
                if (m_touch.IsPressed())
                {
                    m_touchEnd = myTouch.position;
                    if (m_lastDrag == null)
                    {
                        if ((m_touchEnd - m_touchOrigin).magnitude > m_currentThreshold)
                        {
                            // Start Drag
                            m_lastDrag = new Drag(m_touchOrigin, m_touchEnd);
                        }
                    }
                    else
                    {
                        // Update Drag
                        m_lastDrag.m_end = m_touchEnd;
                    }
                }
            }
            else if (myTouch.phase == TouchPhase.Ended && m_touchOrigin.x >= 0)
            {
                if (m_touch.IsPressed())
                {
                    m_touch.Release();
                    m_touchEnd = myTouch.position;

                    // could consider using sqrMagnitude here
                    if ((m_touchEnd - m_touchOrigin).magnitude > m_currentThreshold)
                    {
                        //Drag
                        m_lastDrag = new Drag(m_touchOrigin, m_touchEnd);
                    }
                    else
                    {
                        m_lastDrag = null;
                        //Tap
                    }
                    m_touchOrigin.x = -1;
                }
            }
        }
#endif



        // update interface controls
        InterfaceUpdate();

        SelectObject();
    }

    public bool Touched()
    {
        return m_touch.WasPressed() && m_lastDrag == null;
    }
    public bool Touching()
    {
        return m_touch.IsPressed() && m_lastDrag == null;
    }

    public bool Tapped()
    {
        return m_touch.WasReleased() && m_lastDrag == null;
    }

    public bool Dragging()
    {
        return m_touch.IsPressed() && m_lastDrag != null;
    }

    public bool Dragged()
    {
        return m_touch.WasPressed() && m_lastDrag != null;
    }


    void InterfaceUpdate()
    {
        if (Touched())
        {
            m_interfaceState = UserControllerState.Touched;
        }
        else if (Touching())
        {
            m_interfaceState = UserControllerState.Touching;
        }
        else if (Tapped())
        {
            m_interfaceState = UserControllerState.Tapped;
        }
        else if (Dragging())
        {
            m_interfaceState = UserControllerState.Dragging;
        }
        else if (Dragged())
        {
            m_interfaceState = UserControllerState.Dragged;
        }
        else
        {
            m_interfaceState = UserControllerState.Idle;
        }
    }

    void SelectObject()
    {
        RaycastHit hit;
        Ray ray;

        if (Touched())
        {
            //ray = Camera.main.ScreenPointToRay(m_startMousePosition);
            //if (Physics.Raycast(ray, out hit, 1000, m_layerMask))
            //{
            //    m_state = State.Moving;
            //    m_grabWorldPosition = hit.point;
            //    m_grabWorldPosition.y = m_rigOrigin.y;
            //}

        }

    }
}
