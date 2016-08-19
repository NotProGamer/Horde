using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {

    public Transform target;            // The position that that camera will be following.
    public float smoothing = 5f;        // The speed with which the camera will be following.

	void FixedUpdate ()
    {
        // Smoothly interpolate between the camera's current position and it's target position.
        transform.position = Vector3.Lerp(transform.position, target.position, smoothing * Time.deltaTime);
    }
}
