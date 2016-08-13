using UnityEngine;
using System.Collections;

public class Tapper : MonoBehaviour {

    public float m_tapTimeOutDelay = 0.5f;
    private float m_lastTap = 0f;
    private int m_tapCount = 0;

    private float m_tapVolume = 0f;
    public float m_baseVolume = 1f;
    public float m_noiseReduction = 1f;

    private NoiseGenerator m_noiseGenerator = null;

    private LayerMask m_layerMask;
    public Transform m_zombieLure = null;

    void Awake()
    {
        m_noiseGenerator = GetComponent<NoiseGenerator>();
        if (m_noiseGenerator == null)
        {
            Debug.Log("NoiseGenerator not included.");
        }

        if (m_zombieLure == null)
        {
            Debug.Log("Zombie Lure not included.");
        }
    }

    // Use this for initialization
    void Start ()
    {
        m_layerMask = LayerMask.GetMask("Ground");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonUp(0))
        {
            MoveLure();
            GenerateTapNoise();
        }
        if (m_tapCount > 0 && Time.time > m_lastTap + m_tapTimeOutDelay)
        {
            // Generate noise
            if (m_noiseGenerator != null)
            {
                m_noiseGenerator.GenerateNoise(m_tapVolume, m_noiseReduction, NoisePriority.HighPriority);
                Debug.Log("Tapper Noise Volume: " + m_tapVolume + " Noise Reduction: " + m_noiseReduction);
            }
            //Reset Tapper
            m_tapCount = 0;
            m_tapVolume = 0f;
        }


    }

    private void GenerateTapNoise()
    {
        if (Time.time < m_lastTap + m_tapTimeOutDelay)
        {
            // Increment Tapper
            m_tapVolume += m_baseVolume;
            m_tapCount += 1;
        }
        else
        {
            // Start Tapper
            m_tapVolume = m_baseVolume;
            m_tapCount = 1;
        }
        m_lastTap = Time.time;

    }
    private void MoveLure()
    {
        Vector3 position = new Vector3();

        if (m_zombieLure)
        {

            RaycastHit hit;
            Ray ray;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000/*, m_layerMask*/))
            {
                position = hit.point;
                m_zombieLure.transform.position = position;
            }

        }

    }
}

