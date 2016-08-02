using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectiveManager : MonoBehaviour {

    private List<bool> m_objectives = null;
    private int m_completedObjectives = 0;

    public enum ObjectiveTypes
    {
        ObjectiveA,
        ObjectiveB,
        ObjectiveC,
        ObjectCount
    }

    [System.Serializable]
    public class ObjectiveUpdater
    {
        public bool m_objectiveComplete = false;
        public ObjectiveTypes m_ObjectiveLabel = ObjectiveTypes.ObjectiveA;
        public ObjectiveManager m_objectiveManager = null;
        public void Awake()
        {
            GameObject gameController = GameObject.FindGameObjectWithTag(Tags.GameController);

            if (gameController)
            {
                m_objectiveManager = gameController.GetComponent<ObjectiveManager>();
            }
            else
            {
                Debug.Log("Unable to Find GameController");
            }

            if (m_objectiveManager == null)
            {
                Debug.Log("ObjectiveManager not included on GameController");
            }

        }

        public void Update()
        {
            if (m_objectiveComplete)
            {
                m_objectiveManager.MarkObjectiveComplete(m_ObjectiveLabel);
            }

        }
    }

    // Use this for initialization
    void Start () {
        m_objectives = new List<bool>();
        for (int i = 0; i < (int)ObjectiveTypes.ObjectCount; i++)
        {
            m_objectives.Add(false);
        }
    }
	
	//// Update is called once per frame
	//void Update ()
 //   {

	
	//}

    public void MarkObjectiveComplete(ObjectiveTypes pObjective)
    {
        int objective = (int)pObjective;
        if (objective > -1 && objective < (int)ObjectiveTypes.ObjectCount)
        {
            m_objectives[objective] = true;
            m_completedObjectives++;
        }
    }

    public int GetCompletedObjectiveCount()
    {
        return m_completedObjectives;
    }

    public int GetTotalObjectives()
    {
        return m_objectives.Count;
    }

    public bool AllObjectivesComplete()
    {
        return m_completedObjectives == m_objectives.Count;
    }
}
