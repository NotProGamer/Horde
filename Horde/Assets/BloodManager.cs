using UnityEngine;
using System.Collections;

public class BloodManager : MonoBehaviour
{
    ObjectPoolManager objectPool;
    GameObject manager;
    GameObject[] humans;


    int currentHumans;
    int lastHumans;

	// Use this for initialization
	void Start ()
    {
        manager = GameObject.FindGameObjectWithTag("GameController");

        if (manager)
        {
            objectPool = manager.GetComponent<ObjectPoolManager>();
        }

        humans = GameObject.FindGameObjectsWithTag("Human");

        currentHumans = humans.Length;
        lastHumans = currentHumans;

	}
	
	// Update is called once per frame
	//void Update ()
 //   {
 //       currentHumans = humans.Length;

 //       if (currentHumans < lastHumans)
 //       {
 //           lastHumans = currentHumans;

 //           foreach (GameObject human in humans)
 //           {
 //               Health hp = human.GetComponent<Health>();
 //               if (hp.IsDead)
 //               {
 //                   objectPool.RequestObjectAtPosition("Blood", human.transform.position);
 //               }
 //           }

 //       }



        
	
	}
}
