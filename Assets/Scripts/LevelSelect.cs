using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour {

    public GameObject levelSelect;
    public GameObject mainMenu;
    public Button[] m_Level; 
    //public Sprite[] m_SpriteLevel;
    //public Sprite[] m_SpriteLevelLocked;

    public int m_LevelFinish = 0;
    public int m_MaxLevel = 8;
    private void Awake()
    {
        m_LevelFinish = PlayerPrefs.GetInt("LevelFinish", 0);
        for (int i = 0; i <= m_LevelFinish; ++i)
        {
            m_Level[i].interactable = true;
        }
        for (int i = m_LevelFinish + 1; i < m_MaxLevel; ++i)
        {
            m_Level[i].interactable = false;
        }
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void ButtonReturnMainMenu()
    {
        levelSelect.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ButtonLevel1()
    {
        SceneManager.LoadScene(1);
        Data.instance.currentScene = 1;

    }

    public void ButtonLevel2()
    {
        SceneManager.LoadScene(2);
        Data.instance.currentScene = 2;
    }

    public void ButtonLevel3()
    {
        SceneManager.LoadScene(3);
        Data.instance.currentScene = 3;
    }
}
