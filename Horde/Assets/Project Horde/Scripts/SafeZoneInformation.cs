using UnityEngine;
using System.Collections;

public class SafeZoneInformation : MonoBehaviour
{
    ObjectiveUpdater objective;

    public Renderer[] safeZoneIndicators;

    float duration = 1.0f;
    float alpha = 0;



    // Use this for initialization
    void Start ()
    {
        objective = GetComponent<ObjectiveUpdater>();

        safeZoneIndicators = gameObject.GetComponentsInChildren<Renderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (objective.m_status == ObjectiveStatus.Incomplete)
        {
            foreach (Renderer rend in safeZoneIndicators)
            {
                rend.material.color = Color.blue;
            }
        }
        else if (objective.m_status == ObjectiveStatus.Complete)
        {
            foreach (Renderer rend in safeZoneIndicators)
            {
                rend.material.color = Color.green;
            }       
        }

        LerpAlpha();
    }


    void LerpAlpha()
    {
        float lerp = Mathf.PingPong(Time.time, duration) / duration;
        alpha = Mathf.Lerp(0.0f, 1.0f, lerp);

        foreach (Renderer rend in safeZoneIndicators)
        {
            Color col = rend.material.color;
            col.a = alpha;
            rend.material.color = col;
        }
    }
}
