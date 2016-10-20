using UnityEngine;
using System.Collections;

public class MoveToEnemy : BaseBehaviour {

    private HumanEvaluations m_evaluationsScript = null;
    private Movement m_movementScript = null;

    public MoveToEnemy(GameObject pParent) : base(pParent)
    {
        if (m_parent == null)
        {
            Debug.Log("Parent GameObject not included.");
            return; // early exit
        }
        
        m_movementScript = m_parent.GetComponent<Movement>();
        if (m_movementScript == null)
        {
            Debug.Log("Movement Script not included");
        }

        m_evaluationsScript = m_parent.GetComponent<HumanEvaluations>();
        if (m_evaluationsScript == null)
        {
            Debug.Log("HumanEvaluations Script not included");
        }
    }

    public override Status Update()
    {
        if (m_parent == null)
        {
            Debug.Log("Parent GameObject not included.");
            return Status.FAILURE; // early exit
        }
        if (m_movementScript && m_evaluationsScript)
        {
            Vector3 destination;
            // Get Priority Enemy
            GameObject priorityEnemy = null;
            priorityEnemy = m_evaluationsScript.GetHighestPriorityEnemy();
            if (priorityEnemy != null)
            {
                // Move To Priority Enemy
                destination = priorityEnemy.transform.position;
                m_movementScript.SetDestination(destination);
                return Status.SUCCESS;
            }

        }

        return Status.FAILURE;
    }

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}
}
