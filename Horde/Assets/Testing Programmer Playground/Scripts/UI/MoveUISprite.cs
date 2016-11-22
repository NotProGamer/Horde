using UnityEngine;
using System.Collections;

public class MoveUISprite : MonoBehaviour {

    private Vector3 m_target;
    private bool activated = true;
    // Use this for initialization
    void Start()
    {

    }
    // ref: http://answers.unity3d.com/questions/917757/itweenmoveto-is-tweening-the-button-to-the-wrong-p.html

    private float speed = 10.0f;

    // Update is called once per frame
    void Update()
    {
        if (!activated)
        {
            activated = true;
            iTween.ValueTo(gameObject, iTween.Hash(
                "from", GetComponent<RectTransform>().position,
                "to", m_target,
                "time", speed,
                "easeType", "easeOutExpo",
                "onupdate", "OnUpdateUI"));
            //iTween.ValueTo(gameObject, iTween.Hash(
            //    "from", GetComponent<RectTransform>().anchorMax,
            //    "to", m_target.GetComponent<RectTransform>().anchorMax,
            //    "time", speed,
            //    "easeType", "easeOutExpo",
            //    "onupdate", "OnUpdateUI2"));
            //iTween.ValueTo(gameObject, iTween.Hash(
            //    "from", GetComponent<RectTransform>().anchoredPosition,
            //    "to", m_target.GetComponent<RectTransform>().anchoredPosition,
            //    "time", speed,
            //    "easeType", "easeOutExpo",
            //    "onupdate", "OnUpdateUI3"));
        }
    }

    public void SetTarget(Vector3 target)
    {
        m_target = target;
        activated = false;
    }

    public void OnUpdateUI(Vector3 pos)
    {
        GetComponent<RectTransform>().position = pos;

    }
    public void OnUpdateUI2(Vector3 pos)
    {
        GetComponent<RectTransform>().anchorMax = pos;
    }

    public void OnUpdateUI3(Vector3 pos)
    {
        GetComponent<RectTransform>().anchoredPosition = pos;
    }

}
