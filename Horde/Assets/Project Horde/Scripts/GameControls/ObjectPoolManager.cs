using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPoolManager : MonoBehaviour {

    [System.Serializable]
    public class ObjectPool
    {
        public string m_identifier = "";
        public GameObject m_prefab = null;
        public List<GameObject> m_pool = new List<GameObject>();
        public int m_size = 10;
        public bool m_willGrow = false;
        public GameObject m_container = null;
        private bool m_disabled = false;
        public void Start()
        {
            if (m_prefab == null)
            {
                Debug.Log("Prefab not included!");
                m_disabled = true;
                return; // early exit
            }

            if (m_identifier == "")
            {
                //Debug.Log("Identifier not set!");
                //m_disabled = true;
                //return;
                m_identifier = m_prefab.tag;
            }

            if (m_pool.Count < m_size)
            {
                // fill
                for (int i = m_pool.Count; i < m_size; i++)
                {
                    GameObject obj = Instantiate(m_prefab) as GameObject;
                    obj.SetActive(false);
                    if (m_container)
                    {
                        obj.transform.SetParent(m_container.transform);
                    }
                    m_pool.Add(obj);
                }
            }
            else if (m_pool.Count > m_size)
            {
                m_size = m_pool.Count;
            }
        }
        public GameObject GetObject()
        {
            if (m_disabled)
            {
                return null;
            }

            GameObject obj = null;
            // find an inactive object in to the pool
            for(int i = 0; i < m_pool.Count; i++)
            {
                if (obj == null && !m_pool[i].activeInHierarchy)
                {
                    obj = m_pool[i];
                }
            }
            // if no inactive objects but pool allowed to grow add new object to pool and return it
            if (obj == null && m_willGrow)
            {
                obj = Instantiate(m_prefab) as GameObject;
                obj.transform.SetParent(m_container.transform);
                obj.SetActive(false);
                m_pool.Add(obj);
                m_size = m_pool.Count;
            }
            return obj;
        }
    }

    public List<ObjectPool> m_objectPools = new List<ObjectPool>();

	// Use this for initialization
	void Start ()
    {
        foreach (ObjectPool pool in m_objectPools)
        {
            pool.Start();
        }
	}
	
	// Update is called once per frame
	//void Update () {	}

    private GameObject RequestObject(string identifer)
    {
        GameObject obj = null;

        ObjectPool pool = m_objectPools.Find(item => item.m_identifier == identifer);
        if (pool != null)
        {
            obj = pool.GetObject();
            if (obj == null)
            {
                //Debug.Log("No '"+ identifer +"' objects available.");
            }
        }
        else
        {
            Debug.Log("'" + identifer + "' ObjectPool not found.");
        }
        return obj;
    }
    public GameObject RequestObjectAtPosition(string identifer, Vector3 position)
    {
        GameObject obj = RequestObject(identifer);

        if (obj != null)
        {
            obj.transform.position = position;
            obj.SetActive(true);
        }
        
        return obj;
    }
    public GameObject RequestObjectAtPosition(string identifer, Vector2 position)
    {
        GameObject obj = RequestObject(identifer);

        if (obj != null)
        {
            obj.GetComponent<RectTransform>().position = position;
            obj.SetActive(true);
        }

        return obj;
    }
}
