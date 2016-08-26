using UnityEngine;
using System.Collections;

public abstract class Behaviour {

    public float m_weight = 0;
    public abstract float Evaluate();
    
    public virtual void Enter() { }

    // Update is called once per frame
    public abstract void Update();

    public virtual void Exit() { }

}
