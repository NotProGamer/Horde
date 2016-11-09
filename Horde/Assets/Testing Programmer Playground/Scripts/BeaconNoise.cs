using UnityEngine;
using System.Collections;

public class BeaconNoise : MonoBehaviour {

    Noise m_noise = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_noise != null)
        {
            if (m_noise.IsExpired())
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void SetNoise(Noise noise)
    {
        m_noise = noise;
    }
}
