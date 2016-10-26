using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour
{
    public Transform target;


    void Awake()
    {
        target = Camera.main.transform;
    }


	// Update is called once per frame
	void Update ()
    {
        if (target != null)
        {
            transform.LookAt(target);   
        }
    }
}
