using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Head Quarters

[System.Serializable]
public class Assignment
{

    [System.Serializable]
    public class Task
    {
        public string m_identifier;
        public List<Transform> items;
        public int m_peopleNeeded = 1;
        public int m_numberOfPostings = 0; // number of people given this assignment
        public bool active = true;
    }

    public Assignment(GameObject requestor, string identifier, int postingIndex, List<Transform> tasks, GameObject assignee = null)
    {
        m_requestor = requestor;
        m_identifier = identifier + postingIndex;
        m_taskIdentifier = identifier;
        m_postIndex = postingIndex;
        m_tasks = tasks;
        m_assignee = assignee;
        if (assignee == null)
        {
            m_status = Status.Posted;
        }
    }

    public string m_identifier = "";
    public string m_taskIdentifier = "";
    public int m_postIndex;
    public GameObject m_requestor;
    public List<Transform> m_tasks;
    protected int m_taskIndex = 0;
    public GameObject m_assignee;

    public enum TaskType
    {
        Default,
        Patrol,
        Guard,
    }
    protected TaskType m_taskType = TaskType.Default;
    public enum Status
    {
        Cancelled, // invalid assignment
        Failed, // all tasks failed
        Posted, // awaiting assignee
        InProgress, // inprogress
        Complete, // assignment completed
    }
    public Status m_status = Status.InProgress;
    public float value = 0f;

    public virtual Transform GetCurrent()
    {
        if (m_tasks.Count <= 0 || m_taskIndex < 0)
        {
            m_status = Status.Cancelled;
            return null;
        }

        if (m_taskIndex >= m_tasks.Count)
        {
            // reset index
            m_taskIndex = 0;
        }

        return m_tasks[m_taskIndex];
    }
    public void NextTask()
    {
        if (m_tasks.Count > 1)
        {
            m_taskIndex++;
        }
    }
    public int GetTaskCount()
    {
        return m_tasks.Count;
    }
    public virtual void UpdateRequestor()
    {
        AssignmentRequestor requestor = m_requestor.GetComponent<AssignmentRequestor>();
        if (requestor)
        {
            //requestor.UpdateTasks(m_identifier, m_taskType, m_status);
            requestor.UpdateTask(m_identifier, m_taskType);
        }
    }
    public virtual bool AssignTo(GameObject assignee)
    {
        if (assignee == null)
        {
            // invalid assignee
            return false; // early exit
        }

        m_assignee = assignee;
        m_status = Status.InProgress;
        return true;
    }
    public TaskType GetTaskType()
    {
        return m_taskType;
    }
}

[System.Serializable]
public class PatrolAssignment : Assignment
{
    public PatrolAssignment(GameObject requestor, string identifier, int postingIndex, List<Transform> tasks, GameObject assignee = null)
        :base(requestor, identifier, postingIndex, tasks, assignee)
    {
        m_taskType = TaskType.Patrol;
    }
    // this class should be defined elsewhere
    public override Transform GetCurrent()
    {
        Transform task = null;
        
        bool valid = false;

        while (valid == false && m_status == Status.InProgress)
        {
            task = base.GetCurrent();

            if (task != null && m_status != Status.Cancelled)
            {
                // do stuff
                // if object is disabled remove task
                if (task.gameObject.activeSelf == false)
                {
                    m_tasks.Remove(task);
                }
                else
                {
                    valid = true;
                }
            }

            if (valid == false)
            {
                if (m_tasks.Count > 0)
                {
                    NextTask();
                }
                else if (m_status != Status.Cancelled)
                {
                    // Fail Patrol Duty
                    m_status = Status.Failed;
                }
            }
        }


        return task;
    }
    //public override void UpdateRequestor()
    //{
    //    AssignmentRequestor requestor = m_requestor.GetComponent<AssignmentRequestor>();
    //    if (requestor)
    //    {
    //        requestor.UpdateTasks(m_identifer, TaskType.Patrol, m_status);
    //    }
    //}
}

[System.Serializable]
public class GuardAssignment : Assignment
{
    public GuardAssignment(GameObject requestor, string identifier, int postingIndex, List<Transform> tasks, GameObject assignee = null)
        :base(requestor, identifier, postingIndex, tasks, assignee)
    {
        m_taskType = TaskType.Guard;
    }
    // this class should be defined elsewhere
    public override Transform GetCurrent()
    {
        Transform task = null;

        bool valid = false;

        while (valid == false && m_status == Status.InProgress)
        {
            task = base.GetCurrent();

            if (task != null && m_status != Status.Cancelled)
            {
                // do stuff
                Health taskHealth = task.GetComponent<Health>();
                // if object is disabled or dead remove task

                if (task.gameObject.activeSelf == false)
                {
                    m_tasks.Remove(task);

                }
                else if (taskHealth)
                {
                    if (taskHealth.IsDead())
                    {
                        m_tasks.Remove(task);
                    }
                }
                else
                {
                    valid = true;
                }
            }

            if (valid == false)
            {
                if (m_tasks.Count > 0)
                {
                    NextTask();
                }
                else if (m_status != Status.Cancelled)
                {
                    // Fail Guard Duty
                    m_status = Status.Failed;
                }
            }
        }


        return task;
    }
    //public override void UpdateRequestor()
    //{
    //    AssignmentRequestor requestor = m_requestor.GetComponent<AssignmentRequestor>();
    //    if (requestor)
    //    {
    //        requestor.UpdateTasks(m_identifer, TaskType.Guard, m_status);
    //    }
    //}

}



public class AssignmentManager : MonoBehaviour {

    //// Head Quarters
    //public List<Assignment> m_assignments;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
