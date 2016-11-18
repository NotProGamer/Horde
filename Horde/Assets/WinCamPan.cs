using UnityEngine;
using System.Collections;

public class WinCamPan : MonoBehaviour
{

    //    public bool panTheCam = false;



    public GameObject looktarget;
    Health hp = null;


    float desiredFov = 10;
    float currentFov;


	// Use this for initialization
	void Start ()
    {
        hp = looktarget.GetComponent<Health>();
        currentFov = Camera.main.fieldOfView;

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (hp.IsDead())
        {
            iTween.LookTo(gameObject, looktarget.transform.position, 2.0f);
            if (currentFov != desiredFov)
            {
                Camera.main.fieldOfView -= 1;
                currentFov = Camera.main.fieldOfView;
            }


            

            
            //iTween.MoveAdd(gameObject, Vector3 amount, float time)
        }
	
	}
}
