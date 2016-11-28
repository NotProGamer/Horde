using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
    ObjectPoolManager objPool;

    public float spawnWaitTime;
    float timeSinceLastSpawn;


	// Use this for initialization
	void Start ()
    {
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        if (gameController != null)
        {
            objPool = gameController.GetComponent<ObjectPoolManager>();
        }
        if (objPool == null)
        {
            Debug.Log("no spawner");
        }
    }

    // Update is called once per frame
    void Update ()
    {
	
	}

    public void SpawnHuman(string identifier)
    {
        if (objPool != null)
        {
            if (Time.time - spawnWaitTime >= timeSinceLastSpawn)
            {
                timeSinceLastSpawn = Time.time;
                objPool.RequestObjectAtPosition(identifier, transform.position);
                //Debug.Log("spawned a dude");
            }
        }

    }
}
