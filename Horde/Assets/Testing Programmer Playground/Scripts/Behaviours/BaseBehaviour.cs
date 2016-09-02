using UnityEngine;
using System.Collections;

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

    private GameObject m_parent = null;
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
    // csharp type def

    public CompositeBehaviour(GameObject pParent) : base(pParent)
    {
        
    }


}
