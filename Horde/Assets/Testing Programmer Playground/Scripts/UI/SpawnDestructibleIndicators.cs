using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnDestructibleIndicators : MonoBehaviour {

    public GameObject m_destructibleIndicatorPrefab = null;

    private List<GameObject> m_destructibleObjects = new List<GameObject>();
    private ObjectPoolManager m_UIObjectPoolManager = null;

    void Awake()
    {
        if (m_destructibleIndicatorPrefab == null)
        {
            Debug.Log("Destructible Indicator not included");
        }
        GameObject obj = GameObject.FindGameObjectWithTag(Labels.Tags.UIController);
        if (obj)
        {
            m_UIObjectPoolManager = obj.GetComponent<ObjectPoolManager>();
        }
        if (m_UIObjectPoolManager == null)
        {
            Debug.Log("UIObjectPoolManager not included");
        }
        

    }

	// Use this for initialization
	void Start ()
    {
        m_destructibleObjects.AddRange(GameObject.FindGameObjectsWithTag(Labels.Tags.Destructible));
        //m_destructibleObjects.AddRange(GameObject.FindGameObjectsWithTag(Labels.Tags.Destructible));
        //m_destructibleObjects.AddRange(GameObject.FindGameObjectsWithTag(Labels.Tags.Destructible));
        for (int i = 0; i < m_destructibleObjects.Count; i++)
        {
            GameObject obj = m_destructibleObjects[i];
            Health health = obj.GetComponent<Health>();
            if (health != null)
            {
                GameObject uiObj = m_UIObjectPoolManager.RequestObjectAtPosition(Labels.Tags.DestructibleIndicator, obj.transform.position);
                //obj.transform.SetParent(transform);
                uiObj.GetComponent<UIFollowGameObject>().m_target = obj;
                //gateHealthBars.Add(obj);
                //GateHealthBar bar = obj.GetComponent<GateHealthBar>();
                //bar.target = destructibles[i];
            }
        }


    }

    // Update is called once per frame
    void Update () {
	
	}
}
