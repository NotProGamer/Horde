using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectiveUpdater : MonoBehaviour {


    public string m_identifer = "";
    public string m_objectiveCompletedText = "";
    public ObjectiveStatus m_status = ObjectiveStatus.Disabled; // to become private
    public List<ObjectiveUpdater> m_children = new List<ObjectiveUpdater>();
    private bool m_changed = false;

    public void SetStatus(ObjectiveStatus pStatus)
    {

        if (m_status != pStatus)
        {
            // Exit state
            switch (m_status)
            {
                case ObjectiveStatus.Disabled:
                    break;
                case ObjectiveStatus.Incomplete:
                    break;
                case ObjectiveStatus.InProgress:
                    break;
                case ObjectiveStatus.Complete:
                    break;
                default:
                    break;
            }

            // Change State
            m_status = pStatus;
            m_changed = true;
            // Enter State
            switch (m_status)
            {
                case ObjectiveStatus.Disabled:
                    break;
                case ObjectiveStatus.Incomplete:
                    break;
                case ObjectiveStatus.InProgress:
                    break;
                case ObjectiveStatus.Complete:
                    break;
                default:
                    break;
            }
        }
    }
    public ObjectiveStatus GetStatus()
    {
        return m_status;
    }

    public float m_progress = 0;
    public int m_objectiveCount = 1;
    public int m_completedObjectives = 0;

    // Use this for initialization
    void Start()
    {
        m_objectiveCount = GetObjectiveCount();
        m_completedObjectives = GetCompletedCount();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_children.Count > 0)
        {
            m_progress = GetProgress();
            if (m_progress >= 1.0f)
            {
                SetStatus(ObjectiveStatus.Complete);
            }
            else if (m_progress > 0)
            {
                SetStatus(ObjectiveStatus.InProgress);
            }
        }
    }

    private int GetObjectiveCount()
    {
        int count = 0;
        if (m_children.Count > 0)
        {
            count = m_children.Count;
            foreach (ObjectiveUpdater child in m_children)
            {
                if (child.m_status != ObjectiveStatus.Disabled)
                {
                    count++;
                }
            }
        }
        else
        {
            count = 1;
        }
        return count;
    }
    private int GetCompletedCount()
    {
        int count = 0;
        if (m_children.Count > 0)
        {
            count = m_children.Count;
            foreach (ObjectiveUpdater child in m_children)
            {
                if (child.m_status != ObjectiveStatus.Complete)
                {
                    count++;
                }
            }
        }
        else
        {
            count = (m_status == ObjectiveStatus.Complete) ? 1 : 0;
        }
        return count;
    }
    private float GetProgress()
    {
        float progress = 0;
        if (GetCompletedCount() > 0)
        {
            progress = GetCompletedCount() / GetObjectiveCount();
        }
        return progress;
    }

    public bool Changed()
    {
        return m_changed;
    }
    public void ClearChange()
    {
        if (m_changed)
        {
            m_changed = false;
        }
    }
}
