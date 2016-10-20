using UnityEngine;
using System.Collections;

public class GunTest : MonoBehaviour
{
    public class Bullet
    {
        public Vector3 start;
        public Vector3 end;
        public float t; // value between 0 (start) and 1 (end)

        public void Update(LineRenderer lr)
        {
            lr.SetPosition(0, Vector3.Lerp(start, end, t));
            lr.SetPosition(1, Vector3.Lerp(start, end, t + 0.025f));
            lr.enabled = (t <= 1.0f);
            t += 0.1f;
        }
    }

    Bullet bullet = new Bullet();

    GameObject target;

    public float range;

    public float timeBetweenBullets = 0.5f;

    float timer;
    Ray shootRay;
    RaycastHit shootHit;
    LineRenderer gunLine;
    float effectsDisplayTime = 0.4f;



    void Awake ()
    {
        target = GameObject.FindGameObjectWithTag ("Zombie");
        gunLine = GetComponent<LineRenderer>();
    }



    void Start ()
    {
     
    }
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;

        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets)
        {
            target = GameObject.FindGameObjectWithTag("Zombie");
            Fire();
        }
        bullet.Update(gunLine);

        //if (timer >= timeBetweenBullets * effectsDisplayTime)
        //{
        //    DisableEffects();
        //}
	
	}

    void Fire()
    {
        timer = 0f;

        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = target.transform.position - transform.position;


        if (Physics.Raycast(shootRay, out shootHit, range))
        {
            bullet.start = shootRay.origin;
            bullet.end = target.transform.position;
            bullet.t = 0;
            //gunLine.SetPosition(1, shootHit.point);
        }
    }


    public void DisableEffects()
    {
        gunLine.enabled = false;
    }

}
