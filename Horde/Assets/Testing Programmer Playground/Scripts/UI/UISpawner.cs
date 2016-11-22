using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UISpawner : MonoBehaviour {


    public GameObject m_zombieHandPrefab = null;
    public GameObject m_zombieHandTarget = null;

 //   // Use this for initialization
 //   void Start () {
	    
	//}
	
	//// Update is called once per frame
	//void Update () {
	
	//}

    protected void CreateGlob(Vector3 position)
    {
        GameObject test = Instantiate(m_zombieHandPrefab, gameObject.transform) as GameObject;
        test.GetComponent<RectTransform>().anchoredPosition = position;
        //test.GetComponent<MoveUISprite>().SetTarget(m_zombieHandTarget);
    }

    public void GenerateGlob(Vector3 position)
    {
        CreateGlob(Camera.main.WorldToViewportPoint(position));
        //CreateGlob(Camera.main.WorldToScreenPoint(position));
    }

}
