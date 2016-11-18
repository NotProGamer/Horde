using UnityEngine;
using System.Collections;

public class SpawnZombieIcon : MonoBehaviour
{
    ObjectPoolManager objectPool;
    GameObject manager;
    Health hp;
    bool dead = false;

    //int health;

    Vector3 offSet = new Vector3(0, 1, 0);

    // Use this for initialization
    void Start ()
    {
        manager = GameObject.FindGameObjectWithTag("GameController");

        //if (manager)
        //{
        //    objectPool = manager.GetComponent<ObjectPoolManager>();
        //}

        hp = GetComponent<Health>();
        //health = hp.m_maxHealth;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (hp.IsDead() == true && dead == false)
        {
            dead = true;
            // call PlusZombieManager.SpawnHand(gameObject.transform.position)
            manager.GetComponent<PlusZombieManager>().SpawnHand(transform.position/* + offSet*/);

        }
    }
}
