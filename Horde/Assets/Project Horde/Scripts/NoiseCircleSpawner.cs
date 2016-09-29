using UnityEngine;
using System.Collections;

public class NoiseCircleSpawner : MonoBehaviour
{
    private UserController m_userController = null;
    public GameObject NoiseVis;
    public float height = 0.5f;

    public float NoiseRadius = 5;



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

	// Update is called once per frame
	void Update ()
    {
        if (m_userController != null)
        {
            if (m_userController.m_state == UserControllerState.Tapped)
            {
                
                //Vector3 test = m_userController.m_tappedObject.hitPosition;
                //test.y = height;
                transform.position = new Vector3 (transform.position.x, height, transform.position.z);

            }
        }

    }
}
