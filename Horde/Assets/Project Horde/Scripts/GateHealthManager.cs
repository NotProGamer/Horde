using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GateHealthManager : MonoBehaviour
{
    //HealthBar Prefab
    public GameObject healthBarPrefab;
    //List of HealthBar Prefabs
    public List<GameObject> gateHealthBars;
    //List of objects in scene tagged as destructible
    public List<GameObject> destructibles;

	void Start ()
    {
        destructibles.AddRange(GameObject.FindGameObjectsWithTag("Destructible"));
        for (int i = 0; i < destructibles.Count; i++)
        {
            Health health = destructibles[i].GetComponent<Health>();
            if (health != null)
            {
                GameObject obj = Instantiate<GameObject>(healthBarPrefab);
                obj.transform.SetParent(transform);
                gateHealthBars.Add(obj);
                GateHealthBar bar = obj.GetComponent<GateHealthBar>();
                bar.target = destructibles[i];
            }
        }
	}
}
