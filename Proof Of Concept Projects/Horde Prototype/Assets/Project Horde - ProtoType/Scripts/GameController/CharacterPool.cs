using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CharacterPool : MonoBehaviour {

    [System.Serializable]
    public class ObjectPool
    {
        public GameObject m_prefab;
        public int m_pooledAmount = 20;
        public bool m_willGrow = true;
        private List<GameObject> m_pooledObjects;
        public GameObject m_poolContainer;
        public void Start()
        {
            if (m_prefab == null)
            {
                Debug.Log("Prefab not included!");
                return; // early exit
            }
            m_pooledObjects = new List<GameObject>();
            for (int i = 0; i < m_pooledAmount; i++)
            {
                if (m_prefab != null)
                {
                    GameObject obj = Instantiate(m_prefab) as GameObject;
                    obj.SetActive(false);
                    if (m_poolContainer)
                    {
                        obj.transform.SetParent(m_poolContainer.transform);
                    }
                    m_pooledObjects.Add(obj);
                }
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
                if (m_prefab != null)
                {
                    obj = Instantiate(m_prefab) as GameObject;
                    obj.SetActive(false);
                    m_pooledObjects.Add(obj);
                }
            }
            return obj;
        }
    }

    public ObjectPool m_zombiePool;
    public ObjectPool m_humanPool;


    // Use this for initialization
    void Start () {

        m_zombiePool.Start();
        m_humanPool.Start();

    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public GameObject RequestZombieAtPosition(Vector3 pPosition)
    {
        GameObject obj = m_zombiePool.GetPooledObject();
        obj.transform.position = pPosition;
        obj.SetActive(true);
        return obj;
    }

    public GameObject RequestHumaneAtPosition(Vector3 pPosition)
    {
        GameObject obj = m_humanPool.GetPooledObject();
        obj.transform.position = pPosition;
        obj.SetActive(true);
        return obj;
    }


}
