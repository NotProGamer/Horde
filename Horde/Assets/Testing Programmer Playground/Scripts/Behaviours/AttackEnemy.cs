using UnityEngine;
using System.Collections;

public class AttackEnemy : BaseBehaviour
{
    private HumanEvaluations m_evaluationsScript = null;
    private Movement m_movementScript = null;
    private Attack m_attackScript = null;

    public AttackEnemy(GameObject pParent) : base(pParent)
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

        m_attackScript = m_parent.GetComponent<Attack>();
        if (m_attackScript == null)
        {
            Debug.Log("HumanEvaluations Script not included");
        }
    }

    //   // Use this for initialization
    //   void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public override Status Update()
    {
        if (m_parent == null)
        {
            Debug.Log("Parent GameObject not included.");
            return Status.FAILURE; // early exit
        }
        if (m_movementScript && m_evaluationsScript && m_attackScript)
        {
            // Stop
            // Face Enemy
            // If Facing Enemy
            //  // Attack Enemy


            // Stop
            m_movementScript.Stop();

            // Face Enemy


            // Attack


            GameObject priorityEnemy = null;
            priorityEnemy = m_evaluationsScript.GetHighestPriorityEnemy();
            if (priorityEnemy != null)
            {
                if (m_attackScript.AttackTarget(priorityEnemy))
                {
                    //return Status.SUCCESS;
                }
                else
                {
                    //return Status.FAILURE;
                }
                return Status.SUCCESS;
            }

        }

        return Status.FAILURE;
    }



}
