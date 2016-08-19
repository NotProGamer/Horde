using UnityEngine;
using System.Collections;

public class Scaler : MonoBehaviour {

    public float m_scale = 1f;
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.localScale = new Vector3(m_scale, m_scale, m_scale);
	}
}
