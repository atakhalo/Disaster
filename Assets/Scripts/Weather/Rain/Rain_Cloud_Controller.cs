using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rain_Cloud_Controller : MonoBehaviour {

    public GameObject Water;                                                                                                           //雨水的prefab
    public Vector3 Cloud_Center;                                                                                                     //下雨的中心点
    public float Rains_Range = 1;                                                                                                     //以Cloud_Center为原点的下雨半径
    public int Rains_Once = 1;                                                                                                           //每次下雨生成水的量
    public float PH = 7.0f;
    public float Rains_Interval = 0.1f;                                                                                              //下雨的间隔
    public float ColorChangeRate_Before = 0.1f;                                                                          //下雨前雨云颜色每秒增加的值
    public float ColorChangeRate_After = 0.1f;                                                                             //下雨后雨云颜色每秒回复的值
    public float Rains_Duration = 5.0f;                                                                                           //下雨的间隔
    public Image m_Cloud1, m_Cloud2;
    public SpriteRenderer m_Background;                                                 //两个云层和背景

    private float m_Timer;                                                                                                                //计时器
    private float m_Rain_Area;                                                                                                       //由下雨半径和每次下雨数量计算的每个水滴的生成半径
    private int m_Cloud_Status;                                                                                                      //云的状态
    private float m_Color = 1.0f;                                                                                                      //颜色值
    private float duration;                                                                                                                  //剩余下雨时间
    
	// Use this for initialization
	void Start ()
    {
        m_Cloud_Status = 0;
       Component[] components =  GameObject.Find("GameScene").GetComponentsInChildren<Transform>();
        m_Cloud1 = components[1].gameObject.GetComponent<Image>();
        m_Cloud2 = components[2].gameObject.GetComponent<Image>();
        //m_Background = GameObject.Find("BackGround").GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        switch (m_Cloud_Status)
        {
            case 1:
                {
                    m_Color -= Time.deltaTime * ColorChangeRate_Before;
                    if(m_Color < 0.3f)
                    {
                        m_Color = 0.3f;
                        m_Cloud_Status = 2;
                    }
                    Color cloudColor =m_Cloud1.color;
                    cloudColor.r = m_Color;
                    cloudColor.g = m_Color;
                    cloudColor.b = m_Color;
                    m_Cloud1.color = cloudColor;
                    m_Cloud2.color = cloudColor;
                    cloudColor.r = 0.5f + m_Color * 0.5f;
                    cloudColor.g = 0.5f + m_Color * 0.5f;
                    cloudColor.b = 0.5f + m_Color * 0.5f;
                    m_Background.color = cloudColor;
                    break;
                }
            case 2:
                {
                    Rain();
                    m_Timer += Time.deltaTime;
                    duration -= Time.deltaTime;
                    if(duration < 0.0f)
                    {
                        m_Timer = 0.0f;
                        m_Cloud_Status = 3;
                    }
                    break;
                }
            case 3:
                {
                    m_Color += Time.deltaTime * ColorChangeRate_After;
                    if (m_Color > 1.0f)
                    {
                        m_Color = 1.0f;
                        m_Cloud_Status = 0;
                    }
                    Color cloudColor = m_Cloud1.color;
                    cloudColor.r = m_Color;
                    cloudColor.g = m_Color;
                    cloudColor.b = m_Color;
                    m_Cloud1.color = cloudColor;
                    m_Cloud2.color = cloudColor;
                    cloudColor.r = 0.5f + m_Color * 0.5f;
                    cloudColor.g = 0.5f + m_Color * 0.5f;
                    cloudColor.b = 0.5f + m_Color * 0.5f;
                    m_Background.color = cloudColor;
                    break;
                }
            default:
                break;
        }

    }

    void Rain()
    {
        if (m_Timer >= Rains_Interval)
        {
            m_Timer -= Rains_Interval;
            Vector3 pos = Cloud_Center - new Vector3(Rains_Range, 0.0f, 0.0f);
            for (int i = 0; i < Rains_Once; i++)
            {
                Instantiate(Water,
            pos  + new Vector3(Random.Range(0.0f, m_Rain_Area) + i * m_Rain_Area, 5.0f, 0.0f),
            Quaternion.identity);
            }
        }
    }

    public void RainStart()
    {
        if (Rains_Once > 0)
        {
            m_Rain_Area = 2.0f * Rains_Range / Rains_Once;
            m_Cloud_Status = 1;
            m_Timer = 0.0f;
            duration = Rains_Duration;
        }
    }
}
