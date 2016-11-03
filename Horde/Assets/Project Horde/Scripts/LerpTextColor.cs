using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LerpTextColor : MonoBehaviour
{
    Text text;
    Image image;

    Color lerpedColor;
    public Color lerpStart = new Color(0, 0, 0, 100);
    public Color lerpEnd = new Color(0, 0, 0, 100);



	// Use this for initialization
	void Start ()
    {
        text = gameObject.GetComponent<Text>();
        image = gameObject.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (text != null)
        {
            lerpedColor = Color.Lerp(lerpStart, lerpEnd, Mathf.PingPong(Time.time, 1));
            text.color = lerpedColor;
        }

        if (image)
        {
            lerpedColor = Color.Lerp(lerpStart, lerpEnd, Mathf.PingPong(Time.time, 1));
            image.color = lerpedColor;
        }
        
    }
}
