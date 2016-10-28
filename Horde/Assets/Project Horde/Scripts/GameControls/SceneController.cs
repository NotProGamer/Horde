using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    [System.Serializable]
    public class SceneLoadOverride
    {
        public enum LoadBy
        {
            Default,
            Title,
            Index,
        }
        public LoadBy m_loadBy = LoadBy.Default;
        public int m_index = 0;
        public string m_title = "";
    }

    public SceneLoadOverride m_nextScene;
    public SceneLoadOverride m_prevScene;

    public void LoadSceneNext()
    {

        switch (m_nextScene.m_loadBy)
        {
            case SceneLoadOverride.LoadBy.Default:
                LoadSceneByIndex(SceneManager.GetActiveScene().buildIndex + 1);
                break;
            case SceneLoadOverride.LoadBy.Title:
                if (m_nextScene.m_title != "")
                {
                    SceneManager.LoadScene(m_nextScene.m_title);
                }
                break;
            case SceneLoadOverride.LoadBy.Index:
                LoadSceneByIndex(m_nextScene.m_index);
                break;
            default:
                break;
        }
    }
    public void LoadScenePrev()
    {
        switch (m_prevScene.m_loadBy)
        {
            case SceneLoadOverride.LoadBy.Default:
                LoadSceneByIndex(SceneManager.GetActiveScene().buildIndex - 1);
                break;
            case SceneLoadOverride.LoadBy.Title:
                if (m_nextScene.m_title != "")
                {
                    SceneManager.LoadScene(m_prevScene.m_title);
                }
                break;
            case SceneLoadOverride.LoadBy.Index:
                LoadSceneByIndex(m_prevScene.m_index);
                break;
            default:
                break;
        }

    }
    public void LoadSceneCurrent()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void LoadSceneMainMenu()
    {
        LoadSceneByName(Labels.Scenes.MainMenu);
    }
    public void LoadSceneCredits()
    {
        LoadSceneByName(Labels.Scenes.Credits);
    }
    public void LoadSceneByIndex(int index)
    {
        if (index >= 0 && index < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(index);
        }
        else
        {
            Debug.Log("Invalid Scene Index: " + index);
        }

    }
    public void LoadSceneByName(string sceneName)
    {
        if (sceneName != "")
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.Log("Invalid Scene Name: '" + sceneName + "'");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public virtual void UserInterfaceControls()
    {
        if (Input.GetKey(KeyCode.R))
        {
            LoadSceneCurrent(); 
        }

        if (Input.GetKey(KeyCode.M))
        {
            LoadSceneMainMenu(); 
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            QuitGame(); 
        }

        if (Input.GetKey(KeyCode.Greater))
        {
            LoadSceneNext();
        }

        if (Input.GetKey(KeyCode.Less))
        {
            LoadScenePrev();
        }


    }

    //// Use this for initialization
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

}
