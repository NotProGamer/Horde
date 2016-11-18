using UnityEngine;
using System.Collections;

public class ZombieHandRequestor : MonoBehaviour {

    private GameObject m_canvas = null;
    private UISpawner m_creator = null;

    private bool m_activated = false;

    private Health m_health = null;

    void Awake()
    {
        m_canvas = GameObject.FindGameObjectWithTag("Canvas");
        m_creator = m_canvas.GetComponent<UISpawner>();
        m_health = GetComponent<Health>();
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_creator && !m_activated && m_health.IsDead())
        {
            m_activated = true;
            Spawn();
        }
    }

    public void Spawn()
    {
        m_creator.GenerateGlob(transform.position);
    }
}
