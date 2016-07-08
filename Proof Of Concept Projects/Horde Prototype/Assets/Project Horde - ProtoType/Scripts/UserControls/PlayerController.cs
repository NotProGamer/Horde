using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
///  Code Author: Reece Howe
///  Project: Horde
///  Engine: Unity
///  Platform: (Mobile) and PC
///  Notes: Controls camera movement and zombie movement
///  Status: Incomplete, still need to implement mobile controls
/// </summary>


//using UnitySampleAssets.CrossPlatformInput;

// Notes: Optimization
// Consider Writing a Controller Object as an example
// NavMesh Agents : https://docs.unity3d.com/Manual/nav-MixingComponents.html


public class PlayerController : MonoBehaviour {

    public Transform m_cameraTarget;
    public float m_deadzoneRadius = 5.0f;

    //public bool m_commandZombies = false;
    public float m_selectionDelay = 1.0f;

    //public float m_speed = 6f;
    private Transform m_zombieLure = null;


    private float m_camRayLength = 200f;          // The length of the ray from the camera into the scene.
    private int m_groundMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.

    public HashSet<GameObject> m_hordeList = new HashSet<GameObject>();
    // could increase selection (trigger) radius based on number of zombies in list
    private float m_selectionTime = 0.0f;
    private bool m_selectZombies = false;

    private bool m_mouseDown = false;
    private bool m_mousePressed = false;
    private bool m_mouseUp = false;
    private Vector3 m_mousePosition = new Vector3(0.0f, 0.0f, 0.0f);

    void Awake()
    {
#if !MOBILE_INPUT
        // Create a layer mask for the floor layer.
        m_groundMask = LayerMask.GetMask(Tags.Layers.Ground);
#endif

    }

    // Use this for initialization
    void Start ()
    {
        GameObject m_zombieLureObject = GameObject.FindGameObjectWithTag(Tags.ZombieLure);
        if (m_zombieLureObject)
        {
            m_zombieLure = m_zombieLureObject.transform;
        }
        else
        {
            Debug.Log("Unable to locate ZombieLure");
        }

    }
	
	// Update is called once per frame
	void Update ()
    {
        //// directional key input
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");

        //Debug.Log("h: " + h +", v:"+ v );

        //Vector2 mousePosition = Input.mousePosition;

        //Debug.Log(mousePosition.ToString());

#if !MOBILE_INPUT
        // https://docs.unity3d.com/ScriptReference/Input.html
        m_mouseDown = Input.GetMouseButtonDown(0); // just pressed
        m_mousePressed = Input.GetMouseButton(0); // currently pressed
        m_mouseUp = Input.GetMouseButtonUp(0); // just released
        m_mousePosition = Input.mousePosition;

        // on mouse up
        // send selected zombies to destination
        // if zombie count < 1
        // move camera target
        if (m_mouseUp)
        {
            CameraTargetOrLureMovement();
        }

        RaycastHit[] hits;
        float sphereRadius = 10.0f;

        // on mouse down
        // select nearby zombies
        // gather nearby zombies
        if (m_mouseDown)
        {
            ZombieSelectorMovement(); // Player

            m_hordeList.Clear();
            hits = Physics.SphereCastAll(transform.position, sphereRadius, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                GameObject other = hit.transform.gameObject;
                if (Tags.IsZombie(other))
                {
                    m_hordeList.Add(other);
                }
            }
            Debug.Log("Horde Selected Count: " + m_hordeList.Count);
        }



#else
        // mobile code
#endif

    }

    void FixedUpdate()
    {

    }



    void ZombieSelectorMovement()
    {
            // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(m_mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit groundHit;

        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out groundHit, m_camRayLength, m_groundMask))
        {
            float distance = (groundHit.point - transform.position).magnitude;
            if (distance > m_deadzoneRadius)
            {
                transform.position = groundHit.point;
                Debug.Log("Move Player");
            }

            // Set player Position

            //// Create a vector from the player to the point on the floor the raycast from the mouse hit.
            //Vector3 playerToMouse = groundHit.point - transform.position;

            //// Ensure the vector is entirely along the floor plane.
            //playerToMouse.y = 0f;

            //// Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            //Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

            //// Set the player's rotation to this new rotation.
            //playerRigidbody.MoveRotation(newRotatation);
        }
    }

    void CameraTargetOrLureMovement()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(m_mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit groundHit;

        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out groundHit, m_camRayLength, m_groundMask))
        {
            if (m_hordeList.Count > 0)
            {
                float distance = (groundHit.point - transform.position).magnitude;
                if (distance > m_deadzoneRadius)
                {
                    if (m_zombieLure)
                    {
                        m_zombieLure.position = groundHit.point;
                        SetHordeDestination(m_zombieLure.position);
                        Debug.Log("Move ZombieLure");
                    }
                }

            }
            else
            {
                float distance = (groundHit.point - m_cameraTarget.position).magnitude;
                if (distance > m_deadzoneRadius)
                {
                    m_cameraTarget.position = groundHit.point;
                    Debug.Log("Move Camera");
                }
            }



            // Set player Position


            //// Create a vector from the player to the point on the floor the raycast from the mouse hit.
            //Vector3 playerToMouse = groundHit.point - transform.position;

            //// Ensure the vector is entirely along the floor plane.
            //playerToMouse.y = 0f;

            //// Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            //Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

            //// Set the player's rotation to this new rotation.
            //playerRigidbody.MoveRotation(newRotatation);
        }
    }

    //void MoveCameraTarget()
    //{
    //    if (m_mouseUp)
    //    {

    //        // Create a ray from the mouse cursor on screen in the direction of the camera.
    //        Ray camRay = Camera.main.ScreenPointToRay(m_mousePosition);

    //        // Create a RaycastHit variable to store information about what was hit by the ray.
    //        RaycastHit groundHit;

    //        // Perform the raycast and if it hits something on the floor layer...
    //        if (Physics.Raycast(camRay, out groundHit, m_camRayLength, m_groundMask))
    //        {
    //            float distance = (groundHit.point - transform.position).magnitude;
    //            if (distance > m_deadzoneRadius)
    //            {
    //                m_cameraTarget.position = groundHit.point;
    //                Debug.Log("Move Camera");
    //            }

    //            // Set player Position


    //            //// Create a vector from the player to the point on the floor the raycast from the mouse hit.
    //            //Vector3 playerToMouse = groundHit.point - transform.position;

    //            //// Ensure the vector is entirely along the floor plane.
    //            //playerToMouse.y = 0f;

    //            //// Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
    //            //Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

    //            //// Set the player's rotation to this new rotation.
    //            //playerRigidbody.MoveRotation(newRotatation);
    //        }
    //    }
    //}

    //void MoveZombieLure()
    //{
    //    // if zombies selected and mouse up
    //    // - move ZombieLure to mouse position
    //    // - update zombie destinations
    //    if (/*m_hordeList.Count > 0 &&*/ m_mouseUp)
    //    {
    //        if (m_zombieLure)
    //        {
    //            // Create a ray from the mouse cursor on screen in the direction of the camera.
    //            Ray camRay = Camera.main.ScreenPointToRay(m_mousePosition);

    //            // Create a RaycastHit variable to store information about what was hit by the ray.
    //            RaycastHit groundHit;

    //            // Perform the raycast and if it hits something on the floor layer...
    //            if (Physics.Raycast(camRay, out groundHit, m_camRayLength, m_groundMask))
    //            {
    //                float distance = (groundHit.point - transform.position).magnitude;
    //                if (distance > m_deadzoneRadius)
    //                {
    //                    m_zombieLure.position = groundHit.point;
    //                    Debug.Log("Move ZombieLure");
    //                }


    //            }
    //        }

    //    }
    //}

    void SetHordeDestination(Vector3 destination)
    {
        if (m_zombieLure == null)
        {
            Debug.Log("ZombieLure not included. Unable to set Horde Destination");
            return;
        }

        if (m_hordeList.Count > 0 && m_mouseUp)
        {
            // calculate destination Vectors


            foreach (GameObject zombie in m_hordeList)
            {
                //NavMeshAgent nav = zombie.GetComponent<NavMeshAgent>();
                //if (nav)
                //{
                //    nav.SetDestination(destination);
                //    Debug.Log("Set Zombie Destination");
                //}
                ZombieMovement zombieMovementScript = zombie.GetComponent<ZombieMovement>();
                if (zombieMovementScript)
                {
                    zombieMovementScript.SetDestination(destination);
                    Debug.Log("Set Zombie Destination");
                }

            }

        }
    }




    //void OnTriggerStay(Collider other)
    //{
    //    Debug.Log("triggerstay");
    //    if (m_selectZombies)
    //    {
    //        if (Tags.IsZombie(other.gameObject))
    //        {
    //            AddZombie(other.gameObject);
    //        }
    //    }
    //}

    //void AddZombie(GameObject other)
    //{
    //    m_hordeList.Add(other);
    //    //could also add horde properties based on zombie type
    //}

    //void RemoveZombie(GameObject other)
    //{
    //    m_hordeList.Remove(other);
    //    //could also remove horde properties based on zombie type
    //}

    ////spherecast : https://docs.unity3d.com/ScriptReference/Physics.SphereCast.html


    //void OnTriggerEnter(Collider other)
    //{
    //    if (Tags.IsZombie(other.gameObject))
    //    {
    //        AddZombie(other.gameObject);
    //    }
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    if (Tags.IsZombie(other.gameObject))
    //    {
    //        RemoveZombie(other.gameObject);
    //    }
    //}

}
