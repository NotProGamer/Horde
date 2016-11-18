using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIScaler : MonoBehaviour
{
    Canvas canvas;
    RectTransform CanvasRect = null;

    private float m_screenHeight;
    private float m_screenWidth;

    Vector2 m_screenSize;

    private Vector2 scale;
    Vector2 reference;

    // Use this for initialization
    void Start ()
    {
        canvas = transform.parent.GetComponent<Canvas>();

        CanvasRect = canvas.GetComponent<RectTransform>();

        m_screenHeight = Screen.height; //CanvasRect.rect.height;
        m_screenWidth = Screen.width; //CanvasRect.rect.width;

        m_screenSize = CanvasRect.rect.size;

        reference = canvas.GetComponent<CanvasScaler>().referenceResolution;
        scale = new Vector2(m_screenWidth / reference.x, m_screenHeight / reference.y);
        scale *= 3;


        transform.localScale = new Vector3(scale.x, scale.y, 1);
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_screenHeight = Screen.height; //CanvasRect.rect.height;
        m_screenWidth = Screen.width;
        scale = new Vector2(m_screenWidth / reference.x, m_screenHeight / reference.y);
        scale *= 3;

        Debug.Log(scale);
        Debug.Log(transform.localScale);


        //Resize if screen rez changes
        if (scale.x != transform.localScale.x || scale.y != transform.localScale.y)
        {
            reference = canvas.GetComponent<CanvasScaler>().referenceResolution;
            scale = new Vector2(m_screenWidth / reference.x, m_screenHeight / reference.y);

            scale *= 3;

            transform.localScale = new Vector3(scale.x, scale.y, 1);

            Debug.Log("UI rescaled");
        }
	
	}
}
