using UnityEngine;
using System.Collections;

public class TestSpawner : MonoBehaviour {

    public GameObject m_UIPrefab = null;
    public GameObject m_canvasParent = null;
    public GameObject m_hordeScore = null;
    public GameObject m_hordeCount = null;

    
    // Use this for initialization
    void Start () {
        if (m_UIPrefab != null)
        {
            SpawnUIAtWorldLocation(transform.position);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SpawnAtMe()
    {
        SpawnUIAtWorldLocation(transform.position);
    }

    void SpawnUIAtWorldLocation(Vector3 position)
    {
        Vector3 viewPortPos = Camera.main.WorldToViewportPoint(position);
        viewPortPos.z = 0;
        RectTransform canvasRect = m_canvasParent.GetComponent<RectTransform>();
        //Vector3 canvasPos = new Vector3 (
        //    canvasRect.rect.width * viewPortPos.x,
        //    canvasRect.rect.height * viewPortPos.y,
        //    viewPortPos.z
        //    );

        Vector3 canvasPos = new Vector3 (
            m_canvasParent.GetComponent<RectTransform>().rect.width * viewPortPos.x,
            m_canvasParent.GetComponent<RectTransform>().rect.height * viewPortPos.y,
            canvasRect.position.z
            );

        Vector3 target = /*m_hordeScore.GetComponent<RectTransform>().position+ */m_hordeCount.GetComponent<RectTransform>().position;

        target.z = m_hordeCount.GetComponent<RectTransform>().position.z;
        GameObject test = Instantiate(m_UIPrefab, m_canvasParent.transform) as GameObject;
        test.GetComponent<RectTransform>().position = canvasPos;
        test.GetComponent<MoveUISprite>().SetTarget(target);

    }
}
