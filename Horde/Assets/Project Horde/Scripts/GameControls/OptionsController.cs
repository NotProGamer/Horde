using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class OptionsController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetKeyUp(KeyCode.R))
        {
            ReloadCurrentScene();
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            QuitGame();
        }

    }

    public void ReloadCurrentScene()
    {
        // Reload the Current Scene
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
