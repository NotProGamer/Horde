using UnityEngine;
using System.Collections;

public class ZombieSoundEffects : MonoBehaviour
{
    public AudioClip attack;
    public AudioClip idle;
    public AudioClip moving;
    public AudioClip devour;
    public AudioClip death;

    private AudioSource audioCenter;

    // Use this for initialization
    void Start ()
    {
        GameObject audio = GameObject.Find("AudioCenter");
        audioCenter = audio.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown("q"))
        {
            audioCenter.PlayOneShot(attack);
        }

        if (Input.GetKeyDown("w"))
        {
            audioCenter.PlayOneShot(idle);
        }

        if (Input.GetKeyDown("e"))
        {
            audioCenter.PlayOneShot(moving);
        }

        if (Input.GetKeyDown("r"))
        {
            audioCenter.PlayOneShot(devour);
        }

        if (Input.GetKeyDown("t"))
        {
            audioCenter.PlayOneShot(death);
        }

    }
}
