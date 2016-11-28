using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour {

    // create an object spawner that spawn object from the object pool

    private ObjectPoolManager m_objectPool = null;
    public float m_delay = 10f;
    private float m_timer = 0;
    public string m_objectName = "";
    public float m_detectionRange = 100f;
    public bool testingSpawner = false;
    void Awake()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(Labels.Tags.GameController);
        if (obj)
        {
            m_objectPool = obj.GetComponent<ObjectPoolManager>();
            if (m_objectPool == null)
            {
                Debug.Log("ObjectPoolManager not included");
            }
        }

    }

    // Use this for initialization
    void Start ()
    {
        m_timer = m_delay;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_timer < Time.time)
        {
            m_timer = Time.time + m_delay;
            if (!CheckForNearbyZombies())
            {
                if (!TriggerSpawn())
                {
                    //Debug.Log("No humans to spawn");
                }
                testingSpawner = true;
            }
            else { testingSpawner = false; }
        }
	}

    protected bool TriggerSpawn()
    {
        bool result = false;
        if (m_objectName != "")
        {
            GameObject spawnedObject = m_objectPool.RequestObjectAtPosition(m_objectName, gameObject.transform.position);
            if (spawnedObject != null)
            {
                result = true;
            }
        }
        return result;
    }
    private const int m_maxResults = 100;
    private Collider[] m_foundObjects = new Collider[m_maxResults];
    public LayerMask m_layerMask;

    protected bool CheckForNearbyZombies()
    {
        bool result = false;

        int count = Physics.OverlapSphereNonAlloc(transform.position, m_detectionRange, m_foundObjects, m_layerMask);
        for (int i = 0; i < count; i++)
        {
            if (Labels.Tags.IsZombie(m_foundObjects[i].gameObject))
            {
                return true;
            }
        }
        
        return result;
    }
}
