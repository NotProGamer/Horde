using UnityEngine;
using System.Collections;

public class WaypointManager : MonoBehaviour
{
    public GameObject indicatorTemplate;
    public int numIndicators;

    HUDIndicator[] indicators;

	// Use this for initialization
	void Start ()
    {
        // set up as many HUDIndicators as we asked for
        indicators = new HUDIndicator[numIndicators];
        for (int i = 0; i < numIndicators; i++)
        {
            GameObject obj = Instantiate<GameObject>(indicatorTemplate);
            obj.transform.SetParent(transform);
            indicators[i] = obj.GetComponent<HUDIndicator>();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void AddTransform(Transform t /*color? */, HUDIndicator.IndicatorType type = HUDIndicator.IndicatorType.SafeZone)
    {
        // if we're already tracking, get out of here.
        foreach (HUDIndicator hi in indicators)
            if (hi.target == t)
                return;

        for (int i = 0; i < numIndicators; i++)
            if (indicators[i].target == null)
            {
                indicators[i].target = t;
                indicators[i].SetType(type);
                // apply color
                return;
            }
    }

    public void RemoveTransform(Transform t)
    {
        for (int i = 0; i < numIndicators; i++)
            if (indicators[i].target == t)
                indicators[i].target = null;
    }


}
