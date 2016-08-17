using UnityEngine;
using System.Collections;

public class Tapper : MonoBehaviour {

    public float m_tapTimeOutDelay = 0.5f;
    private float m_lastTapTime = 0f;
    private Vector3 m_currentTapPosition = new Vector3();
    private float m_tapRadius = 5f;
    private int m_tapCount = 0;

    //private float m_tapVolume = 0f;
    public float m_baseVolume = 4f;
    public float m_volumeIncrementPerTap = 1f;
    public float m_expiryDelay = 1f;

    private NoiseGenerator m_noiseGenerator = null;
    private Noise m_currentNoise = null;

    private LayerMask m_layerMask;
    

    public int m_maxTaps = 10;

    private bool m_userTapped = false;

    void Awake()
    {
        m_noiseGenerator = GetComponent<NoiseGenerator>();
        if (m_noiseGenerator == null)
        {
            Debug.Log("NoiseGenerator not included.");
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
        // Check for Input
        m_userTapped = Input.GetMouseButtonUp(0);

        // Act
        MoveLure(); // Move Lure To Tap Position
        TapNoise(); // Generate or modify tap noise
    }

    private void MoveLure()
    {
        Vector3 position = new Vector3();
        if (m_userTapped)
        {
            RaycastHit hit;
            Ray ray;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000/*, m_layerMask*/))
            {
                position = hit.point;
                if (m_currentNoise == null)
                {
                    m_currentTapPosition = hit.point;
                }
                else
                {
                    float sqrDistance = (position - m_currentTapPosition).sqrMagnitude;
                    float sqrRadius = m_tapRadius * m_tapRadius;
                    // if tapping outside of current tap space
                    if (sqrDistance > sqrRadius)
                    {
                        // initialise for new noise
                        m_currentTapPosition = hit.point;
                        m_currentNoise = null;
                    }
                }
                transform.position = m_currentTapPosition;
            }
        }
    }

    void TapNoise()
    {
        if (m_noiseGenerator != null)
        {
            if (m_userTapped)
            {
                if (m_currentNoise == null)
                {
                    //Make Noise
                    m_currentNoise = m_noiseGenerator.GenerateNoise(m_baseVolume, m_expiryDelay, NoiseIdentifier.UserTap);
                    m_tapCount = 1;
                }
                else
                {
                    if (m_tapCount < m_maxTaps)
                    {
                        // Increase Volume
                        m_currentNoise.m_volume += m_volumeIncrementPerTap;
                        m_currentNoise.m_expiry = Time.time + m_expiryDelay;
                        m_tapCount++;
                    }
                }
                m_lastTapTime = Time.time;
            }

            if (Time.time > m_lastTapTime + m_tapTimeOutDelay || m_tapCount >= m_maxTaps)
            {
                m_currentNoise = null;
            }
        }
    }

    
}

