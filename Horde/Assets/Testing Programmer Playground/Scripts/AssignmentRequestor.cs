using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// Base

public class AssignmentRequestor : MonoBehaviour {

    

    // if someone enters this area they are will request an assignment from HQ

    // if an assignment is available they will be given an assignment

    // post available assignments

    public List<Assignment.Task> m_avaliablePatrolDuties;
    public List<Assignment.Task> m_avaliableGuardDuties;

    //private AssignmentManager m_assignmentManager = null;
    //void Awake()
    //{
    //    //GameObject obj = GameObject.FindGameObjectWithTag(Labels.Tags.GameController);
    //    //if (obj != null)
    //    //{
    //    //    obj.GetComponent<AssignmentManager>();
    //    //    if (m_assignmentManager == null)
    //    //    {
    //    //        Debug.Log("Assignment Manager not included"); // Head Quarters Script
    //    //    }
    //    //}
    //}
    public List<Assignment> m_assignments;
    public bool m_assignmentsAvailable = false;


    // Use this for initialization
    void Start ()
    {
        PostAssignments();
    }

    // Update is called once per frame
    void Update ()
    {
	    
	}

    private bool IsDeadOrDisabled(GameObject other)
    {
        // this could probably be a static function if not for get component

        bool result = false;

        result = !other.activeSelf; // if disabled

        if (result == false)
        {
            Health otherHealth = other.GetComponent<Health>();
            if (otherHealth)
            {
                result = otherHealth.IsDead(); // if dead
            }
        }

        return result;
    }

//public void UpdateTasks(string identifier, 
//        Assignment.TaskType taskType = Assignment.TaskType.Default,
//        Assignment.Status m_status = Assignment.Status.InProgress)
//    {
//        int modifier = 0;
//        switch (m_status)
//        {
//            //case Assignment.Status.Cancelled:
//            //    break;
//            case Assignment.Status.Failed:
//                modifier = -1;
//                break;
//            case Assignment.Status.InProgress:
//                modifier = +1;
//                break;
//            //case Assignment.Status.Complete:
//            //    break;
//            default:
//                break;
//        }
//        if (modifier != 0)
//        {
//            switch (taskType)
//            {
//                //case Assignment.TaskType.Default:
//                //    // no change
//                //    break;
//                case Assignment.TaskType.Patrol:
//                    var patrolTask = m_avaliablePatrolDuties.FirstOrDefault(d => d.m_identifier == identifier);
//                    if (patrolTask != null)
//                    {
//                        patrolTask.m_peopleAssigned += modifier;
//                    }
//                    break;
//                case Assignment.TaskType.Guard:
//                    var guardTask = m_avaliableGuardDuties.FirstOrDefault(d => d.m_identifier == identifier);
//                    if (guardTask != null)
//                    {
//                        guardTask.m_peopleAssigned += modifier;
//                    }
//                    break;
//                default:
//                    break;
//            }
//        }
//    }
    public void UpdateTask(string identifier, Assignment.TaskType taskType = Assignment.TaskType.Default)
    {
        var assignment = m_assignments.FirstOrDefault(d => d.m_identifier == identifier);


        if (assignment != null)
        {
            switch (assignment.m_status)
            {
                case Assignment.Status.Cancelled:
                    //DisableTask(taskType, assignment.m_taskIdentifier);
                    // might consider removing assignments aswell
                    //break;
                case Assignment.Status.Failed:

                    // if assignee dead
                    //  repost
                    // else
                    //  disable task

                    //if (Repost(taskType, assignment.m_taskIdentifier))
                    //{
                    //    m_assignmentsAvailable = true;
                    //};

                    if (IsDeadOrDisabled(assignment.m_assignee))
                    {
                        if (Repost(taskType, assignment.m_taskIdentifier))
                        {
                            m_assignmentsAvailable = true;
                        };
                    }
                    else
                    {
                        DisableTask(taskType, assignment.m_taskIdentifier);
                        // might consider removing assignments aswell
                    }

                    break;
                case Assignment.Status.Posted:
                    break;
                case Assignment.Status.InProgress:
                    break;
                case Assignment.Status.Complete:
                    // consider management of completed tasks
                    break;
                default:
                    break;
            }
        }
    }

    protected bool Repost(Assignment.TaskType taskType, string identifier)
    {
        bool result = false;
        switch (taskType)
        {
            //case Assignment.TaskType.Default:
            //    break;
            case Assignment.TaskType.Patrol:
                var duty = m_avaliablePatrolDuties.FirstOrDefault(d => d.m_identifier == identifier);
                if (duty != null && duty.active)
                {
                    m_assignments.Add(new PatrolAssignment(this.gameObject, duty.m_identifier, duty.m_numberOfPostings++, duty.items));
                    result = true;
                }
                break;
            case Assignment.TaskType.Guard:
                var dutyG = m_avaliableGuardDuties.FirstOrDefault(d => d.m_identifier == identifier);
                if (dutyG != null && dutyG.active)
                {
                    m_assignments.Add(new PatrolAssignment(this.gameObject, dutyG.m_identifier, dutyG.m_numberOfPostings++, dutyG.items));
                    result = true;
                }
                break;
            default:
                // nothing happens
                break;
        }
        return result;
    }
    protected bool DisableTask(Assignment.TaskType taskType, string identifier)
    {
        bool result = false;
        switch (taskType)
        {
            //case Assignment.TaskType.Default:
            //    break;
            case Assignment.TaskType.Patrol:
                var duty = m_avaliablePatrolDuties.FirstOrDefault(d => d.m_identifier == identifier);
                if (duty != null)
                {
                    duty.active = false;
                    result = true;
                }
                break;
            case Assignment.TaskType.Guard:
                var dutyG = m_avaliableGuardDuties.FirstOrDefault(d => d.m_identifier == identifier);
                if (dutyG != null)
                {
                    dutyG.active = false;
                    result = true;
                }
                break;
            default:
                // nothing happens
                break;
        }
        return result;
    }

    protected void PostAssignments()
    {
        // post assignments when the game/level starts
        foreach (var item in m_avaliablePatrolDuties)
        {
            // Post Patrol Duty
            if (item.active)
            {
                for (int i = 0; i < item.m_peopleNeeded; i++)
                {
                    m_assignments.Add(new PatrolAssignment(this.gameObject, item.m_identifier, item.m_numberOfPostings++, item.items));
                    m_assignmentsAvailable = true;
                }
            }
        }
        foreach (var item in m_avaliableGuardDuties)
        {
            // Post Guard Duty
            if (item.active)
            {
                for (int i = 0; i < item.m_peopleNeeded; i++)
                {
                    m_assignments.Add(new GuardAssignment(this.gameObject, item.m_identifier, item.m_numberOfPostings++, item.items));
                    m_assignmentsAvailable = true;
                }
            }
        }
    }

    public void RequestAssignment(GameObject assignee)
    {
        if (assignee == null)
        {
            // invalid assignee
            return; // early exit
        }
        var duty = m_assignments.FirstOrDefault(d => d.m_assignee == null);
        if (duty != null)
        {
            Brain assigneeBrain = assignee.GetComponent<Brain>();
            if (assigneeBrain.m_lookingForAssignments && duty.AssignTo(assignee))
            {
                assigneeBrain.AddAssignment(duty);
            }
            // increment assignment counters
        }
        else
        {
            m_assignmentsAvailable = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (Labels.Tags.IsHuman(other.gameObject))
        {
            // if has no assignments

            RequestAssignment(other.gameObject);
        }
    }

}
