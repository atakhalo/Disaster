using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class GameScene : MonoBehaviour {

    bool test = false; //---*
    public bool m_GameRunning;
    public int m_Wave;
    public int m_CurGold;
    private int m_priGold;
    public Text m_GoldText;
    public Text m_LengehText;
    public Text m_AngleText;
    public Text m_CostText;

    public Slider m_WaveSlider;
    public Slider m_LengthSlider;
    public Slider m_AngleSlider;
    public Button m_ButtonStart;
    public Button m_ButtonReturn;
    //public Toggle m_ToggleBuildArrow;
    public Toggle m_ToggleRepair;
    public Toggle m_ToggleDestroy;

    public Toggle m_ToggleBuildArrow;
    public Toggle m_ToggleWood;
    public Toggle m_ToggleStone;
    public Toggle m_ToggleIron;
    public Toggle m_ToggleTree1;
    public Toggle m_ToggleTree2;
    public Toggle m_ToggleLightRod;
    public Toggle m_ToggleDirtWagon;
    //public GameObject m_Panel;
    public GameObject m_MaterialPanel;
    public GameObject m_TreePanel;
    public GameObject m_DevicePanel;
    public Animator m_PanelAnimator;
    public Camera m_MainCamera;

    public Image m_Arrow;
   
    public Sprite[] m_SpriteArrow;
    public Sprite[] m_SpriteUserObject;
    public Sprite[] m_SpriteUserObjectChosen;

    public Texture m_TextureRepair;
    public Texture m_TextureDestroy;

    public Controller m_Controller;

    public GameObject m_GameSuccess;
    public GameObject m_GameFailed;
    public GameObject m_Star2;
    public GameObject m_Star3;

    public PersonController m_PersonController;


    private Controller.GAMEOBJECT_TYPE m_ObjectType = Controller.GAMEOBJECT_TYPE.WOOD;
    private int m_Length = 1;
    private int m_Rotation = 0;
    private int m_Cost = 0;
    public enum PanelType { MATERIAL, TREE, DEVICE };
    private PanelType panelType = PanelType.MATERIAL;


    public enum BuildStatus { NORMAL, REPAIR, DESTROY };
    private BuildStatus m_BuildStatus = BuildStatus.NORMAL;


    public BuildStatus buildStatus
    {
        get { return m_BuildStatus; }
    }
    // Use this for initialization
    void Start()
    {
        m_BuildStatus = BuildStatus.NORMAL;
        m_GoldText.text = m_CurGold.ToString();
        m_priGold = m_CurGold;
        m_PersonController.CreatePerson();
    }

    // Update is called once per frame
    void Update()
    {
        m_LengehText.text = m_LengthSlider.value.ToString();
        m_Length = (int)m_LengthSlider.value;
        m_AngleText.text = m_AngleSlider.value.ToString();
        m_Rotation = (int)m_AngleSlider.value;
        m_GoldText.text = m_CurGold.ToString();
        m_WaveSlider.value = m_Wave;
        m_WaveSlider.maxValue = m_Controller.Wave.Length;

        
        m_Cost = (int)Mathf.Round(GameHelper.BuildingCost(m_ObjectType, m_Length));
        m_CostText.text = m_Cost.ToString();

        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                string tag;
                GameObject target = hit.collider.gameObject;

                if(target.tag == "Trigger")
                {
                    target = target.GetComponent<Trigger>().Father;
                    tag = target.tag;
                }
                else
                {
                    tag = target.tag;
                }

                if (m_BuildStatus == BuildStatus.REPAIR && (tag == "Building" ||
                    tag == "Lighting_Rod" || tag == "Recover" || tag == "Equipment"))
                {
                    m_CurGold -= (int)Mathf.Round(m_Controller.Repair(target));
                }
                else if (m_BuildStatus == BuildStatus.DESTROY && (tag == "Building" ||
                    tag == "Lighting_Rod" || tag == "Recover" ||
                    tag == "Tree" || tag == "Equipment"))
                {
                    m_CurGold += (int)Mathf.Round(m_Controller.Destroy(target));
                }
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            if(test)
            {
                if (panelType == PanelType.MATERIAL)
                {
                    if (m_Controller.Script_Building.BuildBuilding())
                        m_CurGold -= (int)Mathf.Round(GameHelper.BuildingCost(m_ObjectType, m_Length));
                }
                else if (panelType == PanelType.TREE)
                {
                    if (m_Controller.Script_Tree.BuildTree())
                        m_CurGold -= (int)Mathf.Round(GameHelper.BuildingCost(m_ObjectType, m_Length));
                }
                else if (panelType == PanelType.DEVICE)
                {
                    if (m_Controller.Script_Equipment.BuildEquipment())
                        m_CurGold -= (int)Mathf.Round(GameHelper.BuildingCost(m_ObjectType, m_Length));
                }
            }
            
        }
        
    }

    private void OnGUI()
    {
        if (m_BuildStatus == BuildStatus.REPAIR)
        {
            var mousePos = Input.mousePosition;
            GUI.DrawTexture(new Rect(mousePos.x - m_TextureRepair.width / 2, Screen.height - mousePos.y - m_TextureRepair.height / 2, m_TextureRepair.width, m_TextureRepair.height), m_TextureRepair);
        }
        else if (m_BuildStatus == BuildStatus.DESTROY)
        {
            var mousePos = Input.mousePosition;
            GUI.DrawTexture(new Rect(mousePos.x - m_TextureRepair.width / 2, Screen.height - mousePos.y - m_TextureRepair.height / 2, m_TextureDestroy.width, m_TextureDestroy.height), m_TextureDestroy);
        }
    }

    public void ButtonReturn()
    {
        SceneManager.LoadScene(0);
    }

    public void ButtonStart()
    {
        m_GameRunning = true;
        m_ButtonStart.interactable = false;
        m_Controller.Ready();
        m_PersonController.SetAllPersonInDesign(false);
        m_ToggleDestroy.gameObject.SetActive(false);
        m_ToggleRepair.gameObject.SetActive(false);
        m_ToggleBuildArrow.isOn = true;
        m_ToggleBuildArrow.interactable = false;
    }

    public void FinishWave()
    {
        m_Wave++;
        m_GameRunning = false;
        if (m_Wave != m_WaveSlider.maxValue)
            m_ButtonStart.interactable = true;
        m_PersonController.SetAllPersonInDesign(true);
        m_ToggleDestroy.gameObject.SetActive(true);
        m_ToggleRepair.gameObject.SetActive(true);
        m_ToggleBuildArrow.interactable = true;
        m_CurGold += 1000 + (int)Mathf.Round(m_Controller.Script_Tree.GetMoneyByTree());
    }

    public void FinishLevel()
    {
        m_Wave++;
        m_GameSuccess.SetActive(true);
        Data.instance.acLevel++;
        PlayerPrefs.SetInt("LevelFinish", Data.instance.acLevel);
        int star = 1;
        if (m_CurGold >= m_priGold * 0.6)
        {
            ++star;
        }
        if (m_PersonController.GetNowHP() >= m_PersonController.GetAllHP()*0.6f) 
            ++star;
        if (star == 2)
            m_Star2.SetActive(true);
        if (star == 3)
        {
            m_Star2.SetActive(true);
            m_Star3.SetActive(true);
        }
    }

    public void Die()
    {
        m_GameFailed.SetActive(true);
    }

    public void ToggleBuildArrow(bool isOn)
    {
        if (isOn)
        {
            
            m_PanelAnimator.SetTrigger("RightToLeft");
            //m_Panel.panelType = Panel.PanelType.MATERIAL;
            //m_Panel.userm_ObjectType = Panel.Userm_ObjectType.WOOD;
            //m_Panel.UpdateCost();
            m_Arrow.sprite = m_SpriteArrow[1];
       
        }
        else
        {
            
            ToggleMaterial(true);
            m_PanelAnimator.SetTrigger("LeftToRight");
            m_Arrow.sprite = m_SpriteArrow[0];
            m_ObjectType = Controller.GAMEOBJECT_TYPE.WOOD;
            panelType = PanelType.MATERIAL;
            m_Length = 1;
            m_Rotation = 180;
        }
        
    }

    public void ToggleMaterial(bool isOn)
    {
        if (isOn)
        {
            m_MaterialPanel.SetActive(true);
            m_TreePanel.SetActive(false);
            m_DevicePanel.SetActive(false);

            ToggleWood(true);
            panelType = PanelType.MATERIAL;
            m_Length = 1;
            m_Rotation = 0;
            m_LengthSlider.value = m_LengthSlider.minValue;
            m_AngleSlider.value = m_AngleSlider.minValue;
        }
    }

    public void ToggleTree(bool isOn)
    {
        if (isOn)
        {
            panelType = PanelType.TREE;
            m_ObjectType = Controller.GAMEOBJECT_TYPE.OAK_TREE;
            m_MaterialPanel.SetActive(false);
            m_TreePanel.SetActive(true);
            m_DevicePanel.SetActive(false);

            ToggleTree1(true);
        }
    }

    public void ToggleDevice(bool isOn)
    {
        if (isOn)
        {
            panelType = PanelType.DEVICE;
            m_ObjectType = Controller.GAMEOBJECT_TYPE.LIGHTNING_ROD;
            m_MaterialPanel.SetActive(false);
            m_TreePanel.SetActive(false);
            m_DevicePanel.SetActive(true);

            ToggleLightRod(true);
        }
    }

    public void ToggleRepair(bool isOn)
    {
        if (isOn)
        {
            m_BuildStatus = BuildStatus.REPAIR;
            m_ToggleRepair.image.color = new Color(1.0f, 150.0f / 255.0f, 0.0f, 150.0f / 255.0f);

        }
        else
        {
            m_BuildStatus = BuildStatus.NORMAL;
            m_ToggleRepair.image.color = Color.white;
        }
    }

    public void ToggleDestroy(bool isOn)
    {
        if (isOn)
        {
            m_BuildStatus = BuildStatus.DESTROY;
            m_ToggleDestroy.image.color = new Color(1.0f, 150.0f / 255.0f, 0.0f, 150.0f / 255.0f);
        }
        else
        {
            m_BuildStatus = BuildStatus.NORMAL;
            m_ToggleDestroy.image.color = Color.white;
        }
    }

    public void ToggleWood(bool isOn)
    {
        if (isOn)
        {
            m_ObjectType = Controller.GAMEOBJECT_TYPE.WOOD;
            m_ToggleWood.image.sprite = m_SpriteUserObjectChosen[0];
            m_ToggleStone.image.sprite = m_SpriteUserObject[1];
            m_ToggleIron.image.sprite = m_SpriteUserObject[2];
        }
        
    }

    public void ToggleStone(bool isOn)
    {
        if (isOn)
        {
            m_ObjectType = Controller.GAMEOBJECT_TYPE.STONE;
            m_ToggleWood.image.sprite = m_SpriteUserObject[0];
            m_ToggleStone.image.sprite = m_SpriteUserObjectChosen[1];
            m_ToggleIron.image.sprite = m_SpriteUserObject[2];
        }
        
    }

    public void ToggleIron(bool isOn)
    {
        if (isOn)
        {
            m_ObjectType = Controller.GAMEOBJECT_TYPE.STEEL;
            m_ToggleWood.image.sprite = m_SpriteUserObject[0];
            m_ToggleStone.image.sprite = m_SpriteUserObject[1];
            m_ToggleIron.image.sprite = m_SpriteUserObjectChosen[2];
        }
        
    }

    public void ToggleTree1(bool isOn)
    {
        if (isOn)
        {
            m_ObjectType = m_ObjectType = Controller.GAMEOBJECT_TYPE.OAK_TREE;
            m_ToggleTree1.image.sprite = m_SpriteUserObjectChosen[3];
            m_ToggleTree2.image.sprite = m_SpriteUserObject[4];
        }
    }

    public void ToggleTree2(bool isOn)
    {
        if (isOn)
        {
            m_ObjectType = Controller.GAMEOBJECT_TYPE.FRUIT_TREE;
            m_ToggleTree1.image.sprite = m_SpriteUserObject[3];
            m_ToggleTree2.image.sprite = m_SpriteUserObjectChosen[4];
        }
    }

    public void ToggleLightRod(bool isOn)
    {
        if (isOn)
        {
            m_ObjectType = Controller.GAMEOBJECT_TYPE.LIGHTNING_ROD;
            m_ToggleLightRod.image.sprite = m_SpriteUserObjectChosen[5];
            m_ToggleDirtWagon.image.sprite = m_SpriteUserObject[6];
        }
    }

    public void ToggleDirtWagon(bool isOn)
    {
        if (isOn)
        {
            m_ObjectType = Controller.GAMEOBJECT_TYPE.RECOVERER;
            m_ToggleLightRod.image.sprite = m_SpriteUserObject[5];
            m_ToggleDirtWagon.image.sprite = m_SpriteUserObjectChosen[6];
        }
    }

    public void ButtonCost()
    {
        if (m_CurGold < m_Cost)
            return;
        m_ToggleBuildArrow.isOn = true;
        if (panelType == PanelType.MATERIAL)
        {
            m_Controller.Script_Building.ChooseBuilding(m_ObjectType);
            m_Controller.Script_Building.SetLength(m_Length);
            m_Controller.Script_Building.SetRotation(m_Rotation);
        }
        else if (panelType == PanelType.TREE)
            m_Controller.Script_Tree.ChooseTree(m_ObjectType);
        else if (panelType == PanelType.DEVICE)
            m_Controller.Script_Equipment.ChooseEquipment(m_ObjectType);


        test = true;//---*
    }

    public void ButtonRestart()
    {
        SceneManager.LoadScene(Data.instance.currentScene);
    }

    public void ButtonNextLevel()
    {
        if (Data.instance.currentScene < Data.instance.maxLevels)
            SceneManager.LoadScene(++Data.instance.currentScene);
        else
            SceneManager.LoadScene(0);
    }

}
