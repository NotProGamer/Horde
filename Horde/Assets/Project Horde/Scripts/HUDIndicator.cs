﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDIndicator : MonoBehaviour
{

    public Transform target;
    Canvas canvas;
    Image icon;
    RectTransform CanvasRect = null;

    private float m_screenHeight;
    private float m_screenWidth;

    Vector2 m_screenSize;

    public float borderTop = 20;
    public float borderLeft = 20;
    public float borderRight = 10;
    public float borderBottom = 10;

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


        //first you need the RectTransform component of your canvas
        CanvasRect = canvas.GetComponent<RectTransform>();
        m_screenHeight = Screen.height; //CanvasRect.rect.height;
        m_screenWidth = Screen.width; //CanvasRect.rect.width;
        m_screenSize = CanvasRect.rect.size;
        Debug.Log("Width = " + m_screenWidth);
        Debug.Log("Height = " + m_screenHeight);
        Debug.Log(m_screenSize);
    }

    // Update is called once per frame
    void Update()
    { 
        if (target)
        {
            icon.enabled = true;
            //this is the ui element
            RectTransform UI_Element;

            float maxHeight = m_screenHeight;
            float minHeight = 0;

            float screenLeft = -m_screenWidth * ((50 - borderLeft)*0.01f);
            float screenRight = m_screenWidth * ((50 - borderRight) * 0.01f);

            float screenTop = m_screenHeight * ((50 - borderTop)*0.01f);
            float screenBottom = -m_screenHeight * ((50 - borderBottom)*0.01f);




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
            bool offScreen = WorldObject_ScreenPosition.x < screenLeft || WorldObject_ScreenPosition.x > screenRight
                || WorldObject_ScreenPosition.y < screenBottom || WorldObject_ScreenPosition.y > screenTop;

            WorldObject_ScreenPosition.x = Mathf.Clamp(WorldObject_ScreenPosition.x, screenLeft, screenRight);
            WorldObject_ScreenPosition.y = Mathf.Clamp(WorldObject_ScreenPosition.y, screenBottom, screenTop);
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
        m_offset.y = 0;
        switch (m_indicatorType)
        {
            case IndicatorType.SafeZone:
                icon.color = Color.green;
                break;
            case IndicatorType.Human:
                m_offset.y = 3;
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