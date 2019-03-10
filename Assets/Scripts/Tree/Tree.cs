using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{

    //------------------------------------
    //           控件管理区 ↓
    //------------------------------------

    private Rigidbody2D m_Rigidbody2D;
    /// <summary>
    /// 描述：附着在物体的2D刚体控件。Cpn: Component
    /// </summary>
    public Rigidbody2D Cpn_Rigidbody2D
    {
        get { return m_Rigidbody2D; }
    }

    private Collider2D m_Collider2D;
    /// <summary>
    /// 描述：附着在物体的2D碰撞盒控件。Cpn: Component
    /// </summary>
    public Collider2D Cpn_Collider2D
    {
        get { return m_Collider2D; }
    }

    private SpriteRenderer m_Renderer;
    /// <summary>
    /// 描述：附着在物体的渲染精灵控件。Cpn: Component
    /// </summary>
    public SpriteRenderer Cpn_Renderer
    {
        get { return m_Renderer; }
    }

    //------------------------------------
    //           外部数据管理区 ↓
    //------------------------------------

    /// <summary>
    /// 描述：树木的种类
    /// </summary>
    public Controller.GAMEOBJECT_TYPE Type;

    private bool m_IsCanWorking;
    /// <summary>
    /// 描述：当前位置是否可放置
    /// </summary>
    public bool IsCanWorking
    {
        get { return m_IsCanWorking; }
        set
        {
            if (IsWorking == false)
            {
                if (value == false)
                {
                    // 不能放在该位置
                    Color color = Cpn_Renderer.color;
                    color.r = 1.0f;
                    color.g = 0.5f;
                    color.b = 0.5f;
                    Cpn_Renderer.color = color;

                }
                else
                {
                    // 允许放在该位置
                    Color color = Cpn_Renderer.color;
                    color.r = 0.5f;
                    color.g = 1.0f;
                    color.b = 0.5f;
                    Cpn_Renderer.color = color;
                }
            }
            m_IsCanWorking = value;
        }
    }

    private bool m_IsWorking;
    /// <summary>
    /// 描述：树是否已经放置在真实世界中
    /// </summary>
    public bool IsWorking
    {
        get { return m_IsWorking; }
        set
        {
            if (value == false)
            {
                Cpn_Collider2D.enabled = false;

                Color color = Cpn_Renderer.color;
                color.a = 0.5f;
                Cpn_Renderer.color = color;
            }
            else
            {
                Cpn_Collider2D.enabled = true;

                Color color = Cpn_Renderer.color;
                color.a = 1.0f;
                color.r = 1.0f;
                color.g = 1.0f;
                color.b = 1.0f;
                Cpn_Renderer.color = color;
            }

            m_IsWorking = value;
        }
    }

    /// <summary>
    /// 描述：游戏场景中的地面Y轴位置
    /// </summary>
    public float LandPositionY = 0.0f;

    /// <summary>
    /// 描述：树的初始血量
    /// </summary>
    public float StartHP;


    private TreeController m_MyController;
    /// <summary>
    /// 描述：树的上司——控制者
    /// </summary>
    public TreeController MyController
    {
        get { return m_MyController; }
        set { m_MyController = value; }
    }

    //------------------------------------
    //           血量管理区 ↓
    //------------------------------------

    public float m_HP;
    /// <summary>
    /// 描述：树的当前血量
    /// </summary>
    public float HP
    {
        get { return m_HP; }
        set
        {
            float newHP = Mathf.Clamp(value, 0, StartHP);
            if (IsWorking == true)
            {
                m_HP = newHP;
                if (m_HP <= 0)
                {
                    GoToDie();
                }
            }
        }
    }

    //------------------------------------
    //          主逻辑管理区 ↓
    //------------------------------------

    public void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Collider2D = GetComponent<Collider2D>();
        m_Renderer = GetComponent<SpriteRenderer>();
    }

    public void Start()
    {
        m_HP = StartHP;
    }

    public void FixedUpdate()
    {
        if (false == IsWorking)
        {
            // 跟随鼠标
            Vector2 newPosition = new Vector2(GameHelper.MousePoint2D().x, LandPositionY);
            Cpn_Rigidbody2D.MovePosition(newPosition);
        }

    }

    //------------------------------------
    //          方法接口管理区 ↓
    //------------------------------------
    public void GoToDie()
    {
        // TODO 树木死亡
        Destroy(this.gameObject);
    }

}
