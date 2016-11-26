using UnityEngine;
using System.Collections.Generic;
using System;

public class SoundLibrary : MonoBehaviour 
{

    [Serializable]
    public class SoundItem
    {
        public string identifier;
        public AudioClip clip;
    }

    [Serializable]
    public class Category
    {
        public List<SoundItem> items;

        public SoundItem GetItem(string identifier)
        {
            foreach (SoundItem item in items)
            {
                if (item.identifier == identifier)
                {
                    return item;
                }
            }
            return null;
        }        
    }

    [Serializable]
    public class Categories
    {
        public Category UISounds;
        public Category Environmental;
        public Category SoundEffects;
        //public Category Player;
        public Category Zombie;

        public Category HumanCivilian;
        public Category AbnormalAbilities;


        private List<Category> categoryCache;

        public SoundItem GetItem(string identifier)
        {
            if (categoryCache == null)
            {
                categoryCache = new List<Category>();
                categoryCache.Add(UISounds);
                categoryCache.Add(Zombie);
                categoryCache.Add(Environmental);
                categoryCache.Add(SoundEffects);
                //categoryCache.Add(Player);
                categoryCache.Add(HumanCivilian);
                categoryCache.Add(AbnormalAbilities);
            }

            foreach (Category category in categoryCache)
            {
                SoundItem item = category.GetItem(identifier);
                if (item != null)
                {
                    return item;
                }                
            }

            Debug.LogWarning("Warning could not locate sound item named: " + identifier);
            return null;
        }
    }

    public Categories categories;

    private static SoundLibrary instance = null;

    static SoundItem GetItem(string identifier)
    {
        return instance.categories.GetItem(identifier);
    }

    public static AudioSource PlaySound(GameObject target, string identifier, float volume = 1.0f, float pitch = 1.0f, bool loop = false)
    {
        AudioSource source = target.GetComponentInChildren<AudioSource>();
        SoundItem item = GetItem(identifier);

        if (source == null)
        {
            Debug.LogWarning("Could not locate audio source on target: " + target.name);
        }

        if (item != null && source != null)
        {
            source.volume = volume;
            source.pitch = pitch;
            source.loop = loop;
            source.PlayOneShot(item.clip);
            return source;
        }
        return null;
    }

    void Awake()
    {
        if (instance != null) 
        {
            Debug.LogError("Error Sound Library already exists.");
        }
        instance = FindObjectOfType<SoundLibrary>();
    }

	// Use this for initialization
	void Start () 
    {
        //GameObject.DontDestroyOnLoad(gameObject);
	}

	// Update is called once per frame
	void Update () 
    {
	
	}
}

[Serializable]
public class SoundProperties
{
    public string identifier;
    public int audioClipLength;
}
