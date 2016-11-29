using UnityEngine;
using System.Collections;
using System;

public class WinCamPan : MonoBehaviour
{

    public GameObject m_win = null;
    //    public bool panTheCam = false;
    GameObject GameController;
    ObjectiveManager objManager;

    GameObject looktarget;
    Health hp = null;

    float desiredFov = 10;
    float currentFov;


    Quaternion cameraRotation;
    Quaternion faceRotation;
    Vector3 difference;


    public GameObject[] safeZoneHumans;
    bool oneActive = false;


	// Use this for initialization
	void Start ()
    {
        GameController = GameObject.FindGameObjectWithTag("GameController");
        objManager = GameController.GetComponent<ObjectiveManager>();
        currentFov = Camera.main.fieldOfView;
        cameraRotation = Camera.main.transform.rotation;
    }
	
	// Update is called once per frame
	void Update ()
    {
        int remainingHumans = 0;

        if (objManager.m_objectives.Count - objManager.completedObjectives == 1)
        {
            //Debug.Log("checking remaininghumans");
            for (int i = 0; i < safeZoneHumans.Length; i++)
            {
                if (safeZoneHumans[i].activeInHierarchy)
                {
                    remainingHumans++;
                }
            }
        }

        if (remainingHumans == 1 && oneActive == false)
        {
            for (int i = 0; i < safeZoneHumans.Length; i++)
            {
                if (safeZoneHumans[i].activeInHierarchy)
                {
                    hp = safeZoneHumans[i].GetComponent<Health>();
                    looktarget = safeZoneHumans[i];
                    oneActive = true;
                }
            }
        }


        if (hp != null)
        {
            if (hp.IsDead() && oneActive == true)
            {
                difference = looktarget.transform.position - Camera.main.transform.position;
                faceRotation = Quaternion.LookRotation(difference);
                Quaternion lerp = Quaternion.Lerp(cameraRotation, faceRotation, 500);
                Camera.main.transform.rotation = lerp;

                if (currentFov != desiredFov)
                {
                    Camera.main.fieldOfView -= 1;
                    currentFov = Camera.main.fieldOfView;
                    //Debug.Log("test12");
                }
                else
                {
                    TriggerTerritoryInfected();
                }

            }
        }
        
	}

    private void TriggerTerritoryInfected()
    {
        //Debug.Log("test");
        if (m_win)
        {
            m_win.SetActive(true);
        }
    }
}
