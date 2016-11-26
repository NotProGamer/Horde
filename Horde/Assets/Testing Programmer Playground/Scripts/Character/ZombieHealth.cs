using UnityEngine;
using System.Collections;

public class ZombieHealth : Health {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }

    public override void ApplyDamage(int damage)
    {
        base.ApplyDamage(damage);
    }


}
