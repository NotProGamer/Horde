using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ZombieCount : MonoBehaviour 
{
    public Text hordeCount;
    int hordeSizeInt;
	GameObject[] hordeArray;
	private float m_delay = 1.0f;
	private float nextTime = 0f;



	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
    {
		
		if (Time.time > nextTime) 
		{
			
			hordeArray = GameObject.FindGameObjectsWithTag ("Player");

			if (hordeArray != null) 
			{
				hordeSizeInt = hordeArray.Length;
			}

			hordeCount.text = hordeSizeInt.ToString();
			nextTime = Time.time + m_delay;
		}

	}
}
