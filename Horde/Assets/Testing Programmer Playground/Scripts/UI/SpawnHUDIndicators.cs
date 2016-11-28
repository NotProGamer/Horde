using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnHUDIndicators : MonoBehaviour {
    private ObjectPoolManager m_UIObjectPoolManager = null;

    public GameObject m_HUDIndicatorPrefab = null;
    private List<GameObject> m_inGameObjects = new List<GameObject>();
    public List<string> m_objectTags;// = new List<string>();

    void Awake()
    {
        if (m_HUDIndicatorPrefab == null)
        {
            Debug.Log("HUDIndicator not included");
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
    void Start()
    {
        //for (int i = 0; i < m_objectTags.Count; i++)
        //{
        //    m_inGameObjects.AddRange(GameObject.FindGameObjectsWithTag(m_objectTags[i]));
        //}

        

        m_inGameObjects.AddRange(GameObject.FindGameObjectsWithTag(Labels.Tags.Human));
        //m_destructibleObjects.AddRange(GameObject.FindGameObjectsWithTag(Labels.Tags.Destructible));
        //m_destructibleObjects.AddRange(GameObject.FindGameObjectsWithTag(Labels.Tags.Destructible));
        for (int i = 0; i < m_inGameObjects.Count; i++)
        {
            GameObject obj = m_inGameObjects[i];
            Health health = obj.GetComponent<Health>();
            if (health != null)
            {
                GameObject uiObj = m_UIObjectPoolManager.RequestObjectAtPosition(Labels.Tags.EnemyIndicator, obj.transform.position);
                if (uiObj)
                {
                    uiObj.GetComponent<UIFollowGameObject>().m_target = obj;
                }
                else
                {
                    Debug.Log("error");
                }
                
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
