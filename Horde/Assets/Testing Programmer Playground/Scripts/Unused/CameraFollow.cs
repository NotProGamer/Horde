using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public Transform target;            // The position that that camera will be following.
    public float smoothing = 5f;        // The speed with which the camera will be following.
    private Vector3 offset;                     // The initial offset from the target.


    // Use this for initialization
    void Start () {
        // Calculate the initial offset.
        offset = transform.position - target.position;

    }

    // Update is called once per frame
    //void Update () {	}

    void FixedUpdate()
    {
        // Create a postion the camera is aiming for based on the offset from the target.
        Vector3 targetCamPos = target.position + offset;

        // Smoothly interpolate between the camera's current position and it's target position.
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
