using UnityEngine;
using System.Collections;

public class TargetMarker : MonoBehaviour
{
    public GameObject brainMarker;
    SpriteRenderer brainSprite;


    public float timesTargeted;


    Color brainColor;



	// Use this for initialization
	void Start ()
    {
        if (brainMarker != null)
        {
            brainSprite = brainMarker.GetComponentInChildren<SpriteRenderer>();

            if (brainSprite == null)
            {
                Debug.Log("brainSprite not assigned");
            }
        }
        else
        {
            Debug.Log("Brain Marker gameobject needs to be assigned in inspector");
        }

        
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (timesTargeted > 0)
        {

            brainMarker.SetActive(true);

            if (brainMarker.activeInHierarchy)
            {

                float lerpFloat = timesTargeted / 5;


                brainColor = Color.Lerp(Color.white, Color.red, lerpFloat);

                //Debug.Log(timesTargeted);
                //Debug.Log(lerpFloat);
                
                //Mathf.PingPong(Time.time, 1)

                brainSprite.color = brainColor;

            }
        }
	
	}
}
