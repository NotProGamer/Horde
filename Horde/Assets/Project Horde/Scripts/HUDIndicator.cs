using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDIndicator : MonoBehaviour
{

    public Transform target;
    Canvas canvas;
    Image icon;

    public enum IndicatorType
    {
        SafeZone,
        Human,
        Destructible,
    }

    public Vector3 m_offset = Vector3.zero;

    public IndicatorType m_indicatorType = IndicatorType.SafeZone;

    void Start()
    {
        canvas = transform.parent.parent.GetComponent<Canvas>();
        icon = GetComponent<Image>();
        SetType(m_indicatorType);
    }

    // Update is called once per frame
    void Update()
    { 
        if (target)
        {
            icon.enabled = true;
            //this is the ui element
            RectTransform UI_Element;

            //first you need the RectTransform component of your canvas
            RectTransform CanvasRect = canvas.GetComponent<RectTransform>();

            //then you calculate the position of the UI element
            //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.
            Vector3 position = target.transform.position + m_offset;
            Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(position);

            // invert the coordinates if we've fallen behind the camera plane
            float distInFront = Vector3.Dot((position - Camera.main.transform.position), Camera.main.transform.forward);
            if (distInFront < 0)
                ViewportPosition *= -1;


            Vector2 WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

            //now you can set the position of the ui element
            UI_Element = GetComponent<RectTransform>();
            bool offScreen = Mathf.Abs(WorldObject_ScreenPosition.x) > 900 || Mathf.Abs(WorldObject_ScreenPosition.y) > 420;

            WorldObject_ScreenPosition.x = Mathf.Clamp(WorldObject_ScreenPosition.x, -900, 900);
            WorldObject_ScreenPosition.y = Mathf.Clamp(WorldObject_ScreenPosition.y, -480, 420);
            UI_Element.anchoredPosition = WorldObject_ScreenPosition;

            if (!offScreen)
                UI_Element.eulerAngles = new Vector3(0, 0, 180);
            else
                UI_Element.eulerAngles = new Vector3(0, 0, 270.0f + (180.0f / 3.1415f * Mathf.Atan2(WorldObject_ScreenPosition.y, WorldObject_ScreenPosition.x)));
        }
        else if (!target)
        {
            icon.enabled = false;
        }
    }

    public void SetType(IndicatorType type)
    {
        m_indicatorType = type;
        switch (m_indicatorType)
        {
            case IndicatorType.SafeZone:
                icon.color = Color.green;
                break;
            case IndicatorType.Human:
                m_offset.y += 3;
                icon.color = Color.yellow;
                break;
            case IndicatorType.Destructible:
                icon.color = Color.red;
                break;
            default:
                //m_offset= Vector3.zero;
                break;
        }
    }
}
