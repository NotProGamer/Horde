using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// notes 
// http://davidlegare.ghost.io/behavior-trees-1/
// https://unity3d.com/learn/tutorials/topics/scripting/using-interfaces-make-state-machine-ai
// https://unity3d.com/learn/tutorials/topics/scripting/interfaces


//public interface IBehaviour {
//    void Enter();
//    void Update();
//    void Exit();
//}

[System.Serializable]
public class BaseBehaviour /*:*/ /*ScriptableObject,*/ /*IBehaviour*/
{
    public BaseBehaviour(GameObject pParent)
    {
        m_parent = pParent;
    }

    public enum Status
    {
        INVALID,
        SUCCESS,
        FAILURE,
        RUNNING,
        PENDING,
        EMPTY,
    }

    protected GameObject m_parent = null;
    private Status m_status = Status.INVALID;

    public Status Tick()
    {
        if (m_status == Status.INVALID)
        {
            Enter();
        }

        m_status = Update();

        if (m_status != Status.RUNNING)
        {
            Exit(m_status);
        }

        return m_status;
    }
    protected virtual void Enter() { }
    public virtual Status Update() { return Status.EMPTY; }
    protected virtual void Exit(Status pStatus) { }

}

public class CompositeBehaviour: BaseBehaviour
{
    protected List<BaseBehaviour> m_children;

    public CompositeBehaviour(GameObject pParent) : base(pParent)
    {
        m_children = new List<BaseBehaviour>();
    }


}

public class SequenceBehaviour: CompositeBehaviour
{
    public List<BaseBehaviour>.Enumerator m_currentSequenceChild;
    private bool m_running = true;
    public SequenceBehaviour(GameObject pParent) : base(pParent)
    {
        
    }

    public override Status Update()
    {
        bool running = true;
        while (running)
        {
            // run the current behaviour tick
            Status status = m_currentSequenceChild.Current.Tick();

            // if the tick is not a success return result.
            if (status != Status.SUCCESS)
            {
                return status;
            }

            // move to the next behaviour, if no more behaviours return success
            if (m_currentSequenceChild.MoveNext() == false)
            {
                return Status.SUCCESS;
            }
        }
        // unexpected loop exit;
        return Status.INVALID;
    }
    protected override void Enter()
    {
        //base.Enter();
        ResetEnumerator();
    }
    protected override void Exit(Status pStatus)
    {
        //base.Exit(pStatus);
        if (pStatus == Status.FAILURE || pStatus == Status.SUCCESS)
        {
            ResetEnumerator();
        }
    }

    private void ResetEnumerator()
    {
        m_running = true;
        m_currentSequenceChild = m_children.GetEnumerator();
        m_running = m_currentSequenceChild.MoveNext();
    }
}

public class SelectorBehaviour : CompositeBehaviour
{
    public List<BaseBehaviour>.Enumerator m_currentSequenceChild;
    private bool m_running = true;
    public SelectorBehaviour(GameObject pParent) : base(pParent)
    {

    }

    public override Status Update()
    {
        bool running = true;
        while (running)
        {
            // run the current behaviour tick
            Status status = m_currentSequenceChild.Current.Tick();

            // if the tick is not a failure return result.
            if (status != Status.FAILURE)
            {
                return status;
            }

            // move to the next behaviour, if no more behaviours return success
            if (m_currentSequenceChild.MoveNext() == false)
            {
                return Status.FAILURE;
            }
        }
        // unexpected loop exit;
        return Status.INVALID;
    }
    protected override void Enter()
    {
        //base.Enter();
        ResetEnumerator();
    }
    protected override void Exit(Status pStatus)
    {
        //base.Exit(pStatus);
        if (pStatus == Status.SUCCESS)
        {
            ResetEnumerator();
        }
    }

    private void ResetEnumerator()
    {
        m_running = true;
        m_currentSequenceChild = m_children.GetEnumerator();
        m_running = m_currentSequenceChild.MoveNext();
    }
}

public class NotDecoratorBehaviour: BaseBehaviour
{
    BaseBehaviour m_child = null;

    public NotDecoratorBehaviour(GameObject pParent, BaseBehaviour pChild) : base(pParent)
    {
        m_child = pChild;
        if (m_child == null)
        {
            Debug.Log("Child behaviour not included.");
        }
    }
    public override Status Update()
    {
        if (m_child == null)
        {
            return Status.FAILURE; // early exit;
        }

        //Status status = Tick();


        Status status = m_child.Update();

        switch (status)
        {
            case Status.SUCCESS:
                status = Status.FAILURE;
                break;
            case Status.FAILURE:
                status = Status.SUCCESS;
                break;
            default:
                break;
        }

        return status;
    }
}