using UnityEngine;
using System.Collections;

public class BrainSquish : MonoBehaviour
{

    private UserController m_userController = null;
    public AudioClip squish;
    AudioSource audio;

    // Use this for initialization
    void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(Labels.Tags.GameController);
        if (obj)
        {
            m_userController = obj.GetComponent<UserController>();
            if (m_userController == null)
            {
                Debug.Log("User Controller not included");
            }
        }

        audio = GetComponent<AudioSource>();
        audio.clip = squish;



    }

    // Update is called once per frame
    void Update()
    {
        if (m_userController != null)
        {
            if (m_userController.m_state == UserControllerState.Tapped)
            {
                float pitch;
                pitch = Random.Range(1.0f, 2.0f);
                audio.pitch = pitch;
                audio.Play();
            }

        }
    }
}
