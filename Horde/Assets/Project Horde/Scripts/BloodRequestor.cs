using UnityEngine;
using System.Collections;

public class BloodRequestor : MonoBehaviour
{
    ObjectPoolManager objectPool;
    GameObject manager;
    Health hp;
    bool dead = false;

    int health;

    Vector3 offSet = new Vector3(0, 1, 0);
    



    // Use this for initialization
    void Start ()
    {
        manager = GameObject.FindGameObjectWithTag("GameController");

        if (manager)
        {
            objectPool = manager.GetComponent<ObjectPoolManager>();
        }

        hp = GetComponent<Health>();
        health = hp.m_maxHealth;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (hp.m_health < health)
        {
            objectPool.RequestObjectAtPosition("BloodParticle", transform.position + offSet);
            health = hp.m_health;
            Debug.Log("bleeddding");
        }





        if (hp.IsDead() == true && dead == false)
        {
            dead = true;
            objectPool.RequestObjectAtPosition("BloodSprite", transform.position);
        }
	
	}
}
