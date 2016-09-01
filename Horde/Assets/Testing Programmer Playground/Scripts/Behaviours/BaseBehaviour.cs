using UnityEngine;
using System.Collections;

// notes 
// http://davidlegare.ghost.io/behavior-trees-1/
// https://unity3d.com/learn/tutorials/topics/scripting/using-interfaces-make-state-machine-ai
// https://unity3d.com/learn/tutorials/topics/scripting/interfaces


public interface IBehaviour {
    void Enter();
    void Update();
    void Exit();
}

[System.Serializable]
public class BaseBehaviour : IBehaviour
{


    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }

}