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

    public GameObject lose;



	// Use this for initialization
	void Start () 
	{
        lose.SetActive(false);
        hordeArray = GameObject.FindGameObjectsWithTag(Labels.Tags.Zombie);

    }
	
	// Update is called once per frame
	void Update () 
    {
		
		if (Time.time > nextTime) 
		{
			
			hordeArray = GameObject.FindGameObjectsWithTag (Labels.Tags.Zombie);

			if (hordeArray != null) 
			{
				hordeSizeInt = hordeArray.Length;
			}

			hordeCount.text = hordeSizeInt.ToString();
			nextTime = Time.time + m_delay;
		}

        if (hordeArray.Length == 0)
        {
            lose.SetActive(true);
        }

	}
}
