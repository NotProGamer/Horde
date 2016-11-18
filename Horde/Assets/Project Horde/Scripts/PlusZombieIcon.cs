using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlusZombieIcon : MonoBehaviour
{

    //GameObject target;

    //Canvas canvas;
    //RectTransform CanvasRect = null;

    Vector2 m_targetPosition;
    bool m_moving = false;
    bool m_positionSet = false;
    RectTransform test = null;

    // Use this for initialization
    void Start ()
    {
        m_targetPosition = transform.position;
        test = GetComponent<RectTransform>();
    }
	

	// Update is called once per frame
	void Update ()
    {
        if (m_moving && !m_positionSet)
        {
            //iTween.MoveTo(gameObject, target.transform.position, 10f);
            //iTween.MoveTo(gameObject, m_targetPosition, 10f);
            m_positionSet = true;

            iTween.ValueTo(test.gameObject, iTween.Hash(
         "from", test.anchoredPosition,
         "to", m_targetPosition,
         "time", 10f
         ,
         "onupdatetarget", this.gameObject,
         "onupdate", "MoveGuiElement"
         ));
        
        }
    }

    public void MoveTo(Vector2 position)
    {
        m_targetPosition = position;
        m_moving = true;
    }
}
