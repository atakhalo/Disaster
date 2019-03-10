using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour {

    private bool musicOn;
    public int maxLevels;
    public int acLevel = 0;
    public int currentScene;
    static private Data data; 
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public static Data instance
    {
        get
        {
            if (data == null)
            {
                data = FindObjectOfType<Data>();
                DontDestroyOnLoad(data.gameObject);
            }
            return data;
        }
    }

    private void Awake()
    {
        if (data == null)
        {
            data = this;
            DontDestroyOnLoad(this);
        }
        else if (this != data)
        {
            Destroy(gameObject);
        }
    }

    public void SetMusic(bool isOn)
    {
        musicOn = isOn;
        if (isOn)
        {
            GetComponent<AudioSource>().Play();
        }
        else
        {
            GetComponent<AudioSource>().Pause();
        }
        
    }

}
