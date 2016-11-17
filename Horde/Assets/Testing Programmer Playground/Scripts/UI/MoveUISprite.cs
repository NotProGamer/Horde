using UnityEngine;
using System.Collections;

public class MoveUISprite : MonoBehaviour {

    private GameObject m_target;
    private bool activated = false;
    // Use this for initialization
    void Start()
    {

    }
    // ref: http://answers.unity3d.com/questions/917757/itweenmoveto-is-tweening-the-button-to-the-wrong-p.html

    private float speed = 2.0f;

    // Update is called once per frame
    void Update()
    {
        if (m_target && !activated)
        {
            activated = true;
            iTween.ValueTo(gameObject, iTween.Hash(
                "from", GetComponent<RectTransform>().anchorMin,
                "to", m_target.GetComponent<RectTransform>().anchorMin,
                "time", speed,
                "easeType", "easeOutExpo",
                "onupdate", "OnUpdateUI"));
            iTween.ValueTo(gameObject, iTween.Hash(
                "from", GetComponent<RectTransform>().anchorMax,
                "to", m_target.GetComponent<RectTransform>().anchorMax,
                "time", speed,
                "easeType", "easeOutExpo",
                "onupdate", "OnUpdateUI2"));
            //iTween.ValueTo(gameObject, iTween.Hash(
            //    "from", GetComponent<RectTransform>().anchoredPosition,
            //    "to", m_target.GetComponent<RectTransform>().anchoredPosition,
            //    "time", speed,
            //    "easeType", "easeOutExpo",
            //    "onupdate", "OnUpdateUI3"));
        }
    }

    public void SetTarget(GameObject target)
    {
        m_target = target;
    }

    public void OnUpdateUI(Vector2 pos)
    {
        GetComponent<RectTransform>().anchorMin = pos;

    }
    public void OnUpdateUI2(Vector2 pos)
    {
        GetComponent<RectTransform>().anchorMax = pos;
    }

    public void OnUpdateUI3(Vector2 pos)
    {
        GetComponent<RectTransform>().anchoredPosition = pos;
    }

}
