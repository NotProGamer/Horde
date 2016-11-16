using UnityEngine;
using System.Collections;

public class BloodSpread : MonoBehaviour 
{
	Vector3 startSize;
	Vector3 endSize;
	public float duration;
	float despawnTime;


	void Awake()
	{
		startSize = new Vector3 (0.001f, 0.001f, 0.001f);
		endSize = new Vector3 (0.03f, 0.03f, 0.03f);
		gameObject.transform.localScale = startSize;
	}
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (transform.localScale.x < endSize.x) 
		{
			gameObject.transform.localScale = new Vector3 (transform.localScale.x + 0.0002f, transform.localScale.y + 0.0002f, transform.localScale.z + 0.0002f);
			
		}
		else if (transform.localScale.x > endSize.x) 
		{
			despawnTime = Time.time + duration;

		}

		if (Time.time > despawnTime && transform.localScale.x > endSize.x) 
		{
            gameObject.transform.localScale = startSize;
            gameObject.SetActive (false);
		}
	}
}
