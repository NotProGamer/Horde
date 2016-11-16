using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlusZombieManager : MonoBehaviour
{
    //Icon prefab
    public GameObject plusZombieIcon;

    //Destination to move icon too
    //public Transform target;
    public RectTransform m_target;

    //Array of icons
    PlusZombieIcon[] icons;

    //Array of humans
    GameObject[] humans;

    
    private ObjectPoolManager m_opm = null;

    private GameObject m_canvasObj = null;
    private Canvas m_canvas = null;
    public RectTransform m_canvasRect = null;

    private float m_screenHeight;
    private float m_screenWidth;
    Vector2 m_screenSize;
    private Vector2 scale; // reece

    // Use this for initialization
    void Start ()
    {
        //humans = GameObject.FindGameObjectsWithTag("Human");
        //int iconCount = humans.Length;

        //for(int i = 0; i < iconCount; i++)
        //{
        //    GameObject obj = Instantiate<GameObject>(plusZombieIcon);
        //    obj.transform.SetParent(transform);
        //    icons[i] = obj.GetComponent<PlusZombieIcon>();
        //}        
        m_canvasObj = GameObject.FindGameObjectWithTag("Canvas");
        m_canvas = m_canvasObj.GetComponent<Canvas>();
        m_canvasRect = m_canvas.GetComponent<RectTransform>();

        m_screenHeight = Screen.height; //CanvasRect.rect.height;
        m_screenWidth = Screen.width; //CanvasRect.rect.width;
        m_screenSize = m_canvasRect.rect.size;

        Vector2 reference = m_canvas.GetComponent<CanvasScaler>().referenceResolution;
        scale = new Vector2(m_screenWidth / reference.x, m_screenHeight / reference.y);

        scale *= 2.0f;
        transform.localScale = new Vector3(scale.x, scale.y, 1);

        m_opm = GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectPoolManager>();

    }
	
	
	void Update ()
    {
	
	}

    void MoveIcon()
    {

    }


    public void SpawnHand(Vector3 position)
    {
        // make hand
        // tells hand where to go
        //Vector3 position = target.transform.position;


        RectTransform UI_Element;

        float maxHeight = m_screenHeight;
        float minHeight = 0;

        //float screenLeft = -(m_screenWidth / 2) * ((100 - borderLeft) * 0.01f);
        //float screenRight = (m_screenWidth / 2) * ((100 - borderRight) * 0.01f);

        //float screenTop = (m_screenHeight / 2) * ((100 - borderTop) * 0.01f);
        //float screenBottom = -(m_screenHeight / 2) * ((100 - borderBottom) * 0.01f);

        float screenLeft = -(m_screenWidth / 2);
        float screenRight = (m_screenWidth / 2);

        float screenTop = (m_screenHeight / 2);
        float screenBottom = -(m_screenHeight / 2);


        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(position);

        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * m_canvasRect.sizeDelta.x) - (m_canvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * m_canvasRect.sizeDelta.y) - (m_canvasRect.sizeDelta.y * 0.5f)));



        //GameObject test = RequestHand(WorldObject_ScreenPosition);

        GameObject test = RequestHand(ViewportPosition);

        //now you can set the position of the ui element
        UI_Element = test.GetComponent<RectTransform>();
        bool offScreen = WorldObject_ScreenPosition.x < screenLeft || WorldObject_ScreenPosition.x > screenRight
            || WorldObject_ScreenPosition.y < screenBottom || WorldObject_ScreenPosition.y > screenTop;

        WorldObject_ScreenPosition.x = Mathf.Clamp(WorldObject_ScreenPosition.x, screenLeft, screenRight);
        WorldObject_ScreenPosition.y = Mathf.Clamp(WorldObject_ScreenPosition.y, screenBottom, screenTop);
        UI_Element.anchoredPosition = WorldObject_ScreenPosition;


        test.GetComponent<PlusZombieIcon>().MoveTo(m_target.position);

    }

    public GameObject RequestHand(Vector2 position)
    {
        GameObject test = null;
        test = m_opm.RequestObjectAtPosition("NewZombieIcon", /*position*/ new Vector2());
        return test;
    }
}
