using UnityEngine;
using System.Collections;

public class UIFollowGameObject : MonoBehaviour {

    public GameObject m_target = null;
    //public GameObject m_target = null;


    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_target)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(m_target.transform.position);
            //viewport.z = 0;
            //Debug.Log(viewport);

            Vector2 bounds = new Vector2(
                transform.parent.GetComponent<RectTransform>().rect.width,
                transform.parent.GetComponent<RectTransform>().rect.height);

            if (screenPos.x > 0
                && screenPos.x < bounds.x
                && screenPos.y > 0
                && screenPos.y < bounds.y)
            {
                // set onscreen indicator
                transform.GetComponent<RectTransform>().position = screenPos;
            }
            else
            {
                // set offscreen indicator


                if (screenPos.z < 0)
                {
                    screenPos *= -1; // flips position when objects are behind us.
                }

                
                Vector3 screenCentre = new Vector3(bounds.x, bounds.y, 0) / 2;

                // make 00 the centre of the screen to the object position
                screenPos -= screenCentre;

                // find the angle from the centre of the screen to the object position
                float angle = Mathf.Atan2(screenPos.y, screenPos.x);
                angle -= 90 * Mathf.Deg2Rad;

                float cos = Mathf.Cos(angle);
                float sin = -Mathf.Sin(angle);

                screenPos = screenCentre + new Vector3(sin * 150, cos * 150, 0);

                // y = mx+b format
                float m = cos / sin;

                Vector3 screenBounds = screenCentre * 0.9f;

                // check up down
                if (cos > 0)
                {
                    // up
                    screenPos = new Vector3(screenBounds.y / m, screenBounds.y, 0);
                }
                else
                {
                    //down
                    screenPos = new Vector3(-screenBounds.y / m, -screenBounds.y, 0);
                }

                // if out of bounds, get point on appropriate side
                if (screenPos.x > screenBounds.x)
                {
                    //out of bounds must be on right
                    screenPos = new Vector3(screenBounds.x, screenBounds.x * m, 0);
                }
                else if (screenPos.x < -screenBounds.x)
                {
                    // else out of bounds on left
                    screenPos = new Vector3(-screenBounds.x, -screenBounds.x * m, 0);
                } // else in bounds

                // remore coordinate translation
                screenPos += screenCentre;

                transform.GetComponent<RectTransform>().position = screenPos;
            }

            
            


        }
	}
}
