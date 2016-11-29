using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISafeZonesCaptured : MonoBehaviour {

    private int m_totalSafeZones = 3;
    public int m_safeZonesCaptured = 0;

    public Sprite m_safeZoneFree = null;
    public Sprite m_safeZoneCaptured = null;

    public Image[] test;

    private ObjectiveManager m_objectiveManager = null;

    public GameObject m_win = null;

    void Awake()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(Labels.Tags.GameController);
        if (obj)
        {
            m_objectiveManager = obj.GetComponent<ObjectiveManager>();
        }

    }
    // Use this for initialization
    void Start ()
    {
        if (m_safeZoneFree == null || m_safeZoneCaptured == null)
        {
            Debug.Log("Icons not included");
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (m_objectiveManager)
        {
            m_safeZonesCaptured = m_objectiveManager.completedObjectives;
        }


        if (m_safeZoneFree == null || m_safeZoneCaptured == null)
        {
            return;
        }

        if (m_safeZonesCaptured > m_totalSafeZones || m_safeZonesCaptured < 0 || test.Length < m_totalSafeZones )
        {
            return;
        }

        for (int i = 0; i < m_totalSafeZones; i++)
        {
            if (i < m_safeZonesCaptured)
            {
                test[i].sprite = m_safeZoneCaptured;
            }
            else
            {
                test[i].sprite = m_safeZoneFree;
            }
        }
        if (m_safeZonesCaptured == m_totalSafeZones)
        {
            TriggerWin();
        }
    }


    void TriggerWin()
    {
        m_win.SetActive(true);
        //Debug.Log("test");
    }

}
