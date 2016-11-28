using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivateAbnormal : MonoBehaviour {

    public List<GameObject> m_abnormals = new List<GameObject>();
    public string m_abnormalTag = Labels.Tags.ZombieScreamer;

    void Awake()
    {
        m_abnormals.AddRange(GameObject.FindGameObjectsWithTag(m_abnormalTag));
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Activate()
    {
        for (int i = 0; i < m_abnormals.Count; i++)
        {
            AbnormalAbility test = m_abnormals[i].GetComponent<AbnormalAbility>();
            test.Activate();
        }
    }
}
