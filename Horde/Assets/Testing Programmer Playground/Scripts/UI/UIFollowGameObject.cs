using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIFollowGameObject : MonoBehaviour {

    public GameObject m_target = null;
    //public GameObject m_target = null;

    public Sprite m_onScreenSprite = null;
    public Sprite m_offScreenSprite = null;
    private Image m_image = null;

    public bool m_lerpColor = false;
    public Color m_start = Color.red;
    public Color m_finish = Color.green;
    private Health m_targetHealth = null;

    void Awake()
    {
        m_image = GetComponent<Image>();
        if (m_image == null)
        {
            Debug.Log("Image not included.");
        }
        if (m_target)
        {
            m_targetHealth = m_target.GetComponent<Health>();
            if (m_targetHealth == null)
            {
                Debug.Log("Target Health not included.");
            }
        }
    }


    // Update is called once per frame
    void Update ()
    {
        if (m_target)
        {
            if (m_targetHealth && m_lerpColor)
            {
                float test = m_targetHealth.GetPercentageHealth();
                m_image.color = Color.Lerp(m_finish, m_start, test);
            }

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
                transform.rotation = Quaternion.Euler(0, 0, 0);
                m_image.sprite = m_onScreenSprite;
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
                transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
                m_image.sprite = m_offScreenSprite;
            }

            
            


        }
	}
}
