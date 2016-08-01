using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class OptionsController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyUp(KeyCode.R))
        {
            // Reload the Current Scene
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

    }
}
