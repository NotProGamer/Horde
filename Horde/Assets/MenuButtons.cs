using UnityEngine;
using System.Collections;

public class MenuButtons : MonoBehaviour
{
    public GameObject[] buttons;


	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}



    public void ShowButtons()
    {
        foreach (GameObject obj in buttons)
        {
            obj.SetActive(true);
        }
    }


}
