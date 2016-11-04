using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GateHealthBar : MonoBehaviour
{
    public GameObject target;
    Health health;
    Transform position;
    Image bar;
    Canvas canvas;

    public Vector3 m_offset = Vector3.zero;

    RectTransform CanvasRect = null;

    void Start ()
    {
        health = target.GetComponent<Health>();
        bar = GetComponent<Image>();

        canvas = transform.parent.parent.GetComponent<Canvas>();
        CanvasRect = canvas.GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (target.activeSelf)
        {
            RectTransform UI_Element;
            UI_Element = GetComponent<RectTransform>();

            // move the healthbar to track the target (without clamping)
            //then you calculate the position of the UI element
            //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.
            Vector3 position = target.transform.position + m_offset;
            Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(position);


            Vector2 WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

            UI_Element.anchoredPosition = WorldObject_ScreenPosition;
        }
        else if (!target.activeSelf)
        {
            gameObject.SetActive(false);
        }

        if (health)
        {
            float m_health = health.m_health;
            float healthPercent = m_health / 100;
            bar.color = Color.Lerp(Color.red, Color.green, healthPercent);         
        }
    }
}
