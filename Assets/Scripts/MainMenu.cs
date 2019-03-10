using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour {

    public GameObject levelSelect;
    public GameObject mainMenu;
    public GameObject musicOn;
    public GameObject musicOff;
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void ButtonStart()
    {
        levelSelect.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void ButtonExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ToggleMusic(bool isOn)
    {
        
        Data.instance.SetMusic(isOn);
        
        //TODO
        if (isOn)
        {
            musicOn.SetActive(true);
            musicOff.SetActive(false);
        }
        else
        {
            musicOn.SetActive(false);
            musicOff.SetActive(true);
        }

    }
}
