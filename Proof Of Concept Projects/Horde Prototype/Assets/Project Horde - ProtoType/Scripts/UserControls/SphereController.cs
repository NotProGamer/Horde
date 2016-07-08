using UnityEngine;
using System.Collections;

/// <summary>
///  Code Author: Reece Howe
///  Project: Horde
///  Engine: Unity
///  Platform: (Mobile) and PC
///  Notes: horde velocity movement
///  Status: Incomplete, still need to implement mobile controls
/// </summary>

public class SphereController : MonoBehaviour {

    private bool m_mouseUp;
    private bool m_mousePressed;
    private bool m_mouseDown;
    private Vector3 m_mousePosition;

    private Rigidbody m_rigidbody;
    private Vector3 m_beginDragPosition;
    private Vector3 m_endDragPosition;
    private float m_camRayLength = 200f;          // The length of the ray from the camera into the scene.
    private int m_groundMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    public float m_force = 500.0f;
    public float m_maxVelocity = 100.0f;
    private bool m_commandZombies;

    void Awake()
    {
#if !MOBILE_INPUT
        // Create a layer mask for the floor layer.
        m_groundMask = LayerMask.GetMask(Tags.Layers.Ground);
#endif
        m_rigidbody = GetComponent<Rigidbody>();
    }
    // Use this for initialization
    void Start () {
        //m_rigidbody.drag = 0.5f;

    }
	
	// Update is called once per frame
	void Update () {
#if !MOBILE_INPUT
        // https://docs.unity3d.com/ScriptReference/Input.html
        m_mouseDown = Input.GetMouseButtonDown(0); // just pressed
        m_mousePressed = Input.GetMouseButton(0); // currently pressed
        m_mouseUp = Input.GetMouseButtonUp(0); // just released
        m_mousePosition = Input.mousePosition;

        //if (Input.GetKeyUp(KeyCode.Z))
        //{
        //    m_commandZombies = !m_commandZombies;
        //}

        //if (m_commandZombies)
        //{
        //}

        if (m_mouseDown)
        {

            // Create a ray from the mouse cursor on screen in the direction of the camera.
            Ray camRay = Camera.main.ScreenPointToRay(m_mousePosition);

            // Create a RaycastHit variable to store information about what was hit by the ray.
            RaycastHit groundHit;

            // Perform the raycast and if it hits something on the floor layer...
            if (Physics.Raycast(camRay, out groundHit, m_camRayLength, m_groundMask))
            {
                m_beginDragPosition = groundHit.point;
                
            }
        }

        if (m_mouseUp)
        {

            // Create a ray from the mouse cursor on screen in the direction of the camera.
            Ray camRay = Camera.main.ScreenPointToRay(m_mousePosition);

            // Create a RaycastHit variable to store information about what was hit by the ray.
            RaycastHit groundHit;

            // Perform the raycast and if it hits something on the floor layer...
            if (Physics.Raycast(camRay, out groundHit, m_camRayLength, m_groundMask))
            {
                m_endDragPosition = groundHit.point;

                Vector3 direction = (m_endDragPosition - m_beginDragPosition).normalized;
                UpdateVelocity(direction);

            }
        }
        Debug.Log(m_rigidbody.velocity.magnitude);
#else
        // mobile code
#endif
    }

    void UpdateVelocity(Vector3 pDirection)
    {
        if (m_rigidbody == null)
        {
            Debug.Log("No rigidbody included");
            return;
        }
        pDirection.y = 0.0f;
        m_rigidbody.AddForce(pDirection * m_force);
        Vector3 velocity = m_rigidbody.velocity;
        if (velocity.magnitude > m_maxVelocity)
        {
            velocity = velocity.normalized * m_maxVelocity;
        }
        m_rigidbody.velocity = velocity;
        
    }
}
