using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieBrain : MonoBehaviour {

    private Dictionary<string, object> m_memory = new Dictionary<string, object>();
    // CurrentTarget [Done]
    // EnemiesInSight
    // ClosestEnemy 
    // NoisesInHearing
    // LoudestNoise



    // Look 
    public float m_sightRange = 10f;
    private const int m_maxSightResults = 100;
    private Collider[] m_sightResults = new Collider[m_maxSightResults];

    public LayerMask m_lookMask;

    // Listen
    public float m_hearingRange = 50f;

    // Use this for initialization
    void Start ()
    {
        m_memory.Add("CurrentTarget", transform.position);

    }
	
	// Update is called once per frame
	void Update ()
    {
        //object currentTarget = null;
        //if (m_memory.TryGetValue("CurrentTartget", out currentTarget))
        //{
        //    System.Type t = currentTarget.GetType();
        //    if (t == typeof(Vector3))
        //    {
        //        Debug.Log("Position " + ((Vector3)currentTarget).ToString());
        //        m_memory["CurrentTartget"] = transform.gameObject;
        //    }
        //    if (t == typeof(GameObject))
        //    {
        //        Debug.Log("GameObject Position " + ((GameObject)currentTarget).transform.position.ToString());
        //    }

        //}
        //else
        //{
        //    Debug.Log("Unable to find 'CurrentTarget'");
        //}
        Look();


    }

    public bool GetCurrentTargetPosition(out Vector3 pTargetPosition)
    {
        bool result = false;
        Vector3 position = Vector3.zero;
        object currentTarget = null;
        if (m_memory.TryGetValue("CurrentTarget", out currentTarget))
        {
            System.Type t = currentTarget.GetType();
            if (t == typeof(Vector3))
            {
                position = (Vector3)currentTarget;
                //m_memory["CurrentTartget"] = transform.gameObject;
                result = true;
            }
            else if (t == typeof(GameObject))
            {
                position = ((GameObject)currentTarget).transform.position;
                result = true;
            }
            else if (t == typeof(Noise))
            {
                position = ((Noise)currentTarget).m_position;
                result = true;
            }
        }
        else
        {
            Debug.Log("Unable to find 'CurrentTarget'");
        }
        pTargetPosition = position;
        return result;
    }

    private List<GameObject> m_enemies = new List<GameObject>();

    void Look()
    {
        int count = 0;
        count = Physics.OverlapSphereNonAlloc(transform.position, m_sightRange, m_sightResults, m_lookMask);

        // get all nearby objects

        if (count > 0)
        {
            //Debug.Log(count);

            for (int i = 0; i < count; i++)
            {

            }

        }

    }

    void Listen()
    {
    }
}
