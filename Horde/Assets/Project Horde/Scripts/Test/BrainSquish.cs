using UnityEngine;
using System.Collections;

public class BrainSquish : MonoBehaviour
{

    private UserController m_userController = null;
    public AudioClip squish;
    private AudioSource m_audio = null;
    public ParticleSystem blood;

    void Awake()
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
        m_audio = GetComponent<AudioSource>();
        if (m_audio)
        {
            Debug.Log("AudioSource not included");
        }
        
    }
    // Use this for initialization
    void Start()
    {

        //audio = GetComponent<AudioSource>();
        m_audio.clip = squish;



    }

    // Update is called once per frame
    void Update()
    {
        if (m_userController != null)
        {
            if (m_userController.m_state == UserControllerState.Tapped)
            {
                //float pitch;
                //pitch = Random.Range(1.0f, 2.0f);
                //audio.pitch = pitch;
                //audio.Play();

                //blood.Emit(10);
                if (m_userController.m_tappedObject != null)
                {
                    if (m_userController.m_tappedObject.m_gameObject == gameObject)
                    {
                        Squish();
                    }
                }
            }
        }
    }

    public void Squish()
    {
        float pitch;
        pitch = Random.Range(1.0f, 2.0f);
        m_audio.pitch = pitch;
        m_audio.Play();

        blood.Emit(10);
    }

    void OnEnable()
    {
        Squish();
    }

}
