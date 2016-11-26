using UnityEngine;
using System.Collections;

public class ScreamerMovement : ZombieMovement {
    private Screamer m_screamerScript = null;
    private bool wasScreaming = false;
    new void Awake()
    {
        base.Awake();
        m_screamerScript = GetComponent<Screamer>();
        if (m_screamerScript == null)
        {
            Debug.Log("Screamer script not included");
        }
    }


    // Use this for initialization
    //new void Start()
    //{
    //    base.Start();
    //}

    // Update is called once per frame
    new protected void Update()
    {
        base.Update();

        if (m_screamerScript)
        {
            if (m_screamerScript.m_screaming != wasScreaming)
            {
                wasScreaming = m_screamerScript.m_screaming;
                m_anim.SetBool("Screaming", wasScreaming);
            }
        }

    }
}
