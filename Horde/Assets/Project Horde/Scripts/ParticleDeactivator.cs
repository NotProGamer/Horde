using UnityEngine;
using System.Collections;

public class ParticleDeactivator : MonoBehaviour
{

    private ParticleSystem ps;

	// Use this for initialization
	void Start ()
    {
        ps = GetComponent<ParticleSystem>();
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (ps)
        {
            if (!ps.IsAlive())
            {
                gameObject.SetActive(false);
            }
        }
	
	}
}
