using UnityEngine;
using System.Collections;

public class ZombieHandSpawner : MonoBehaviour {

    public GameObject m_UIPrefab = null;
    private GameObject m_canvasParent = null;
    public GameObject m_hordeScore = null;
    public GameObject m_hordeCount = null;

    private ObjectPoolManager m_UIObjectPoolManagerScript = null;
    void Awake()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(Labels.Tags.UIController);
        if (obj)
        {
            m_UIObjectPoolManagerScript = obj.GetComponent<ObjectPoolManager>();
        }
        if (m_UIObjectPoolManagerScript == null)
        {
            Debug.Log("UIObjectPoolManager not included");
        }
        m_canvasParent = gameObject;
    }
    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update () {}

    public void SpawnUIAtWorldLocation(Vector3 position)
    {
        Vector3 viewPortPos = Camera.main.WorldToViewportPoint(position);
        viewPortPos.z = 0;
        RectTransform canvasRect = m_canvasParent.GetComponent<RectTransform>();
        //Vector3 canvasPos = new Vector3 (
        //    canvasRect.rect.width * viewPortPos.x,
        //    canvasRect.rect.height * viewPortPos.y,
        //    viewPortPos.z
        //    );

        Vector3 canvasPos = new Vector3(
            m_canvasParent.GetComponent<RectTransform>().rect.width * viewPortPos.x,
            m_canvasParent.GetComponent<RectTransform>().rect.height * viewPortPos.y,
            canvasRect.position.z
            );

        Vector3 target = /*m_hordeScore.GetComponent<RectTransform>().position+ */m_hordeCount.GetComponent<RectTransform>().position;

        target.z = m_hordeCount.GetComponent<RectTransform>().position.z;

        GameObject uiObj = m_UIObjectPoolManagerScript.RequestObjectAtPosition(Labels.Tags.PlusZombieIndicator, canvasPos);
        //GameObject test = Instantiate(m_UIPrefab, m_canvasParent.transform) as GameObject;
        //uiObj.GetComponent<RectTransform>().position = canvasPos;
        //uiObj.GetComponent<MoveUISprite>().SetTarget(target);
        MoveUISprite spriteMoveScript = uiObj.GetComponent<MoveUISprite>();

        if (spriteMoveScript)
        {
            spriteMoveScript.SetTarget(target);
        }

    }

}
