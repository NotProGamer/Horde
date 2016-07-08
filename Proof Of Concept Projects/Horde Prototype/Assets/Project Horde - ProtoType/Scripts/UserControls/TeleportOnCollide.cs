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

public class TeleportOnCollide : MonoBehaviour {

    private Transform m_controllerArea;
    private Vector3 m_origin;

    public enum ColliderType
    {
        OnEnter,
        OnExit
    }
    public ColliderType m_colliderType = ColliderType.OnEnter;
    void Awake()
    {
        m_controllerArea = transform.parent.transform;

        m_origin = m_controllerArea.position;
        m_origin.y = 0.5f;
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (m_colliderType == ColliderType.OnEnter)
        {
            if (other.CompareTag("ControllerSphere"))
            {
                other.transform.position = m_origin;
            }
        }

    }
    void OnTriggerExit(Collider other)
    {
        if (m_colliderType == ColliderType.OnExit)
        {
            if (other.CompareTag("ControllerSphere"))
            {
                other.transform.position = m_origin;
            }
        }
    }

}
