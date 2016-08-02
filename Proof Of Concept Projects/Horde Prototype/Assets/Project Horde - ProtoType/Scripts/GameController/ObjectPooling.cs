using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooling : MonoBehaviour {

    public static ObjectPooling m_current;
    public GameObject m_prefab;
    public int m_pooledAmount = 20;
    public bool m_willGrow = true;

    private List<GameObject> m_pooledObjects;

    void Awake()
    {
        m_current = this;
    }

	// Use this for initialization
	void Start ()
    {
        m_pooledObjects = new List<GameObject>();
        for (int i = 0; i < m_pooledAmount; i++)
        {
            GameObject obj = Instantiate(m_prefab) as GameObject;
            obj.SetActive(false);
            m_pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        GameObject obj = null;
        for (int i = 0; i < m_pooledObjects.Count; i++)
        {
            if (!m_pooledObjects[i].activeInHierarchy)
            {
                obj = m_pooledObjects[i];
            }
        }

        if (m_willGrow)
        {
            obj = Instantiate(m_prefab) as GameObject;
            obj.SetActive(false);
            m_pooledObjects.Add(obj);
        }

        return obj;
    }
}
