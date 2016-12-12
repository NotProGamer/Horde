using UnityEngine;
using System.Collections;

public class HumanSpawner : MonoBehaviour
{
    ObjectPoolManager objPool;


    public Transform[] spawnPoints;
    public int spawnCap;





	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        foreach (Transform spawner in spawnPoints)
            {
                SpawnPoint spawnPoint = spawner.GetComponent<SpawnPoint>();
                spawnPoint.SpawnHuman("Human");
            }  
        }

    }


    //void SpawnHuman (string identifier, Transform sp)
    //{
    //    if (objPool != null)
    //    {
    //        objPool.RequestObjectAtPosition(identifier, sp.position);
    //        Debug.Log("spawned a dude");
    //    }
    //    else
    //    {
    //        Debug.Log("no spawner");
    //    }
        
    //}



