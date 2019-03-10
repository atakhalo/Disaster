using System.Collections;
using System.Collections.Generic;
using UnityEngine;  

public class Controller : MonoBehaviour {

    /// <summary>
    /// 灾害天气的枚举
    /// </summary>
    public enum WEATHER_TYPE
    {
        /// <summary>
        /// 下雨
        /// </summary>
        RAIN,   
        /// <summary>
        /// 闪电
        /// </summary>
        LIGHTNING,
        /// <summary>
        /// 龙卷风
        /// </summary>
        TORNADO,  
        /// <summary>
        /// 垃圾
        /// </summary>
        RUBBISH 
    }

    /// <summary>
    /// Controller生成的GameObject类型的枚举
    /// </summary>
    public enum GAMEOBJECT_TYPE
    {
        /// <summary>
        /// 空物体
        /// </summary>
        NONE,
        /// <summary>
        /// 人物
        /// </summary>
        PERSON,                                                                                 
        /// <summary>
        /// 木材
        /// </summary>
        WOOD = 100,                                                                                   
        /// <summary>
        /// 石材
        /// </summary>
        STONE,                                                                                   
        /// <summary>
        /// 钢材
        /// </summary>
        STEEL,                                                                                  
        /// <summary>
        /// 果树
        /// </summary>
        FRUIT_TREE,                                                        
        /// <summary>
        /// 橡树
        /// </summary>
        OAK_TREE,
        /// <summary>
        /// 避雷针
        /// </summary>
        LIGHTNING_ROD,                                                               
        /// <summary>
        ///  垃圾回收器
        /// </summary>
        RECOVERER,                                                                      
        /// <summary>
        /// 垃圾
        /// </summary>
        RUBBISH                                 
    }

    /// <summary>
    /// 灾害的生成以及其它参数
    /// </summary>
    [System.Serializable]
    public struct WeatherControl
    {
        public WEATHER_TYPE WeatherType;
        public float DelayTime;
        public Vector3 Position;
        public float Range;
        public float HP;
        public Vector2 Velocity;
        private bool m_Happened;
        /// <summary>
        /// 该灾害是否被使用了
        /// </summary>
        public bool IsHappened
        {
            get
            {
               return m_Happened;
            }
            set
            {
                m_Happened = value;
            }
        }
    }

    /// <summary>
    /// 每一波的若干个灾害
    /// </summary>
    [System.Serializable]
    public struct WeatherList
    {
        public WeatherControl[] Weather;
    }

    public struct Creator
    {
        public GameObject Prefab;
        public GAMEOBJECT_TYPE Type;
    }

    
    public GameObject Prefab_Lightning;
    public GameObject Prefab_Tornado;
    public GameObject Rubbish_Controller;
    public GameObject Building_Controller;
    public GameObject Tree_Controller;
    public GameObject Person_Controller;
    public GameObject Cloud_Controller;
    public GameObject Equipment_Controller;

    [HideInInspector]
    public BuildingController Script_Building;
    [HideInInspector]
    public PersonController Script_Person;
    [HideInInspector]
    public TreeController Script_Tree;
    [HideInInspector]
    public Rain_Cloud_Controller Script_Cloud;
    [HideInInspector]
    public Rubbish_Controller Script_Rubbish;
    [HideInInspector]
    public EquipmentController Script_Equipment;
    /// <summary>
    /// 每一关的灾害波
    /// </summary>
    public WeatherList[] Wave;
    [HideInInspector]
    public List<Creator> Creators;
    public float PH;
    private int m_Wave;                                                  //这个关卡的当前波数
    private int m_LengthOfWave;                                 //当前波的天气数
    private int m_GameStatus;                                      //游戏状态
    private float m_WaveSuccessDelay;                      //每波天气结束倒计时   
    GameScene gameScene; 

    // Use this for initialization
    private void Awake()
    {
        //Instantiate(Person_Controller);
        //Instantiate(Cloud_Controller);
        Instantiate(Rubbish_Controller);
        Script_Building = Building_Controller.GetComponent<BuildingController>();
        Script_Person = Person_Controller.GetComponent<PersonController>();
        Script_Tree = Tree_Controller.GetComponent<TreeController>();
        Script_Cloud = Cloud_Controller.GetComponent<Rain_Cloud_Controller>();
        Script_Rubbish = Rubbish_Controller.GetComponent<Rubbish_Controller>();
        Script_Equipment = Equipment_Controller.GetComponent<EquipmentController>();
        gameScene = GameObject.Find("GameScene").GetComponent<GameScene>();
    }

    void Start ()
    {
        GameHelper.PH = PH;
        Creators = new List<Creator>();
        m_Wave = 0;
        m_GameStatus = 0;
        m_LengthOfWave = Wave[m_Wave].Weather.Length;
        m_WaveSuccessDelay = 10.0f;
        //初始化所有灾害
        int waves = Wave.Length;
        for( int i = 0; i < waves; i++)
        {
            int weathers = Wave[i].Weather.Length;
            for(int j = 0; j < weathers; j++)
            {
                Wave[i].Weather[j].IsHappened = false;
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		switch(m_GameStatus)
        {
            case 0:                                                                                                                                           //游戏准备阶段
                { }
                break;
            case 1:                                                                                                                                            //游戏进行阶段
                {
                    int weatherHappened = 0;                                                                                               //已经发生了的天气
                    for (int i = 0; i < m_LengthOfWave; i++)                                                                     //遍历这波天气
                    {
                        if (!Wave[m_Wave].Weather[i].IsHappened)                                  //如果这个天气没有发生
                        {
                            Wave[m_Wave].Weather[i].DelayTime -= Time.deltaTime;                  //结算倒计时
                            if (Wave[m_Wave].Weather[i].DelayTime < 0.0f)                                    //如果倒计时结束了
                            {
                                switch (Wave[m_Wave].Weather[i].WeatherType)                              //检查这个天气的类型
                                {
                                    case WEATHER_TYPE.LIGHTNING:
                                        {
                                            CreateLightning(Wave[m_Wave].Weather[i].Position, Wave[m_Wave].Weather[i].Range);
                                        }
                                        break;
                                    case WEATHER_TYPE.RAIN:
                                        {
                                            CreateRain(Wave[m_Wave].Weather[i].Position, Wave[m_Wave].Weather[i].Range, Wave[m_Wave].Weather[i].HP);
                                        }
                                        break;
                                    case WEATHER_TYPE.RUBBISH:
                                        {
                                           Rubbish_Controller.GetComponent<Rubbish_Controller>().CreateRubbish(Wave[m_Wave].Weather[i].Position, Wave[m_Wave].Weather[i].Velocity, Wave[m_Wave].Weather[i].HP, Wave[m_Wave].Weather[i].Range);
                                        }
                                        break;
                                    case WEATHER_TYPE.TORNADO:
                                        {
                                            CreateTornado(Wave[m_Wave].Weather[i].Position, Wave[m_Wave].Weather[i].Velocity, Wave[m_Wave].Weather[i].HP);
                                        }
                                        break;
                                    default:
                                        { }
                                        break;
                                }
                                Wave[m_Wave].Weather[i].IsHappened = true;                                          //标识该天气已经发生
                            }
                        }
                        else
                        {
                            weatherHappened++;
                        }
                    }
                    if (weatherHappened == m_LengthOfWave)                                                         //如果天气放完了
                    {
                        m_GameStatus = 2;
                    }
                }
                break;
            case 2:                                                                                                                                     //游戏结束倒计时阶段
                {
                    m_WaveSuccessDelay -= Time.deltaTime;
                    if(m_WaveSuccessDelay < 0.0f)
                    {
                        m_Wave++;
                        if(m_Wave < Wave.Length)                                                                            //如果还有天气波数
                        {
                            m_LengthOfWave = Wave[m_Wave].Weather.Length;
                            m_GameStatus = 0;
                            gameScene.FinishWave();
                        }
                        else
                        {
                            
                            gameScene.FinishLevel();
                            
                        }
                    }
                }
                break;
            default:
                { }
                break;
        }
	}

    /// <summary>
    /// 修理建材或设备到满血
    /// </summary>
    public float Repair(GameObject target)
    {
        float res = 0.0f;
        switch(target.tag)
        {
            case "Building":
                {
                    res = Script_Building.FullHP(target);
                }
                break;
            case "Equipment":
                {
                    res = Script_Equipment.FullHP(target);                  
                }
                    break;
            default:
                {
                }
                break;
        }

        return res;
    }

    /// <summary>
    /// 拆除设备
    /// </summary>
    public float Destroy(GameObject target)
    {
        float res = 0.0f;
        switch (target.tag)
        {
            case "Building":
                {
                    res = Script_Building.DestroyBuilding(target);
                }
                break;
            case "Tree":
                {
                    res = Script_Tree.DestroyTree(target);
                }
                break;
            case "Equipment":
                {
                    res = Script_Equipment.DetroyEquipment(target);          
                }
                break;
            default:
                {
                }
                break;
        }

        return res;
    }

    /// <summary>
    /// 开始新的一波天气
    /// </summary>
    public void Ready()
    {
        m_LengthOfWave = Wave[m_Wave].Weather.Length;
        m_GameStatus = 1;
        m_WaveSuccessDelay = 10.0f;
    }

    /// <summary>
    /// 创建下雨的天气
    /// </summary>
    /// <param name="position"></param>
    private  void CreateRain(Vector3 position, float range, float HP)
    {
        Cloud_Controller.GetComponent<Rain_Cloud_Controller>().Cloud_Center = position;
        Cloud_Controller.GetComponent<Rain_Cloud_Controller>().Rains_Range = range;
        Cloud_Controller.GetComponent<Rain_Cloud_Controller>().Rains_Duration = HP;
        Cloud_Controller.GetComponent<Rain_Cloud_Controller>().RainStart();
    }

    /// <summary>
    /// 生成一次闪电
    /// </summary>
    /// <param name="position"></param>
    /// <param name="range"></param>
    private void CreateLightning(Vector3 position, float range)
    {
        Vector3 finPos = new Vector3(Random.Range(position.x - range, 2.0f * range), 11.0f, 0.00f);
        GameObject lightning_rod = GameObject.FindGameObjectWithTag("Lightning_Rod");
        GameObject lightning;
        if (lightning_rod != null)
        {
            lightning = Instantiate(Prefab_Lightning, new Vector3(lightning_rod.transform.position.x, 11.0f, 0.0f), Quaternion.identity);
        }
        else
        {
            lightning = Instantiate(Prefab_Lightning, finPos, Quaternion.identity);
        }
       
        lightning.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -20f);
    }
    
    /// <summary>
    /// 生成一个龙卷风
    /// </summary>
    /// <param name="position"></param>
    /// <param name="velocity"></param>
    /// <param name="HP"></param>
    private void CreateTornado(Vector3 position, Vector2 velocity, float HP)
    {
        GameObject tornado = Instantiate(Prefab_Tornado, position, Quaternion.identity);
        tornado.GetComponent<Rigidbody2D>().velocity = velocity;
        tornado.GetComponent<Tornado>().HP = HP;
    }
}
