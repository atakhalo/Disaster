//*************************************************************************************************
//  创建日期：2017-9-30
//  作    者：卢俊廷
//  说    明：该脚本用于管理人物的基本属性。
//*************************************************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 人物数据与操作管理
/// </summary>
public class Person : MonoBehaviour
{
    //------------------------------------
    //          外部数据管理区 ↓
    //------------------------------------

    /// <summary>
    /// 描述：人物的触发器
    /// </summary>
    public GameObject HeadTrigger;
    /// <summary>
    /// 描述：人物的触发器
    /// </summary>
    public GameObject BodyTrigger;
    /// <summary>
    /// 描述：人物的触发器
    /// </summary>
    public GameObject FootTrigger;

    /// <summary>
    /// 描述：人物的初始血量
    /// </summary>
    public float StartHP;

    private PersonController m_MyController;
    /// <summary>
    /// 描述：人物的上司——控制者
    /// </summary>
    public PersonController MyController
    {
        get { return m_MyController; }
        set { m_MyController = value; }
    }

    GameScene gameScene;

    //------------------------------------
    //           状态管理区 ↓
    //------------------------------------

    private bool m_IsDesign;
    /// <summary>
    /// 描述：人物是否在设计版面中
    /// </summary>
    public bool IsDesign
    {
        get { return m_IsDesign; }
        set
        {
            m_IsDesign = value;

            if (value == true)
            {
                // 设计版面
                //Cpn_Rigidbody2D.gravityScale = 0.0f;
                //Cpn_Collider2D.enabled = false;
            }
            else
            {
                // 真实环境
                //Cpn_Rigidbody2D.gravityScale = 1.0f;
                //Cpn_Collider2D.enabled = true;
            }
        }
    }

    private bool m_IsSelete;
    /// <summary>
    /// 描述：人物是否在被鼠标选中
    /// </summary>
    public bool IsSelete
    {
        get { return m_IsSelete; }
        set
        {
            m_IsSelete = value;
            if (value == true)
            {
                GameHelper.SpriteSetColor(gameObject, 0.5f);
            }
            else
            {
                GameHelper.SpriteSetColor(gameObject, 1.0f);
                GameHelper.SpriteSetColor(gameObject, 1.0f, 1.0f, 1.0f);
            }
        }
    }

    private bool m_IsCanSetUp;
    /// <summary>
    /// 描述：人物在设计版面中是否可设置
    /// </summary>
    public bool IsCanSetUp
    {
        get { return m_IsCanSetUp; }
        set
        {
            m_IsCanSetUp = value;
            if (IsDesign == true && IsSelete == true)
            {
                if (value == false)
                {
                    // 不可设置
                    GameHelper.SpriteSetColor(gameObject, 1.0f, 0.5f, 0.5f);
                }
                else
                {
                    // 可设置
                    GameHelper.SpriteSetColor(gameObject, 0.5f, 1.0f, 0.5f);
                }
            }
        }
    }

    /// <summary>
    /// 描述：人物在设计版面移动前的位置
    /// </summary>
    private Vector2 m_BeforePosition;

    //------------------------------------
    //           控件管理区 ↓
    //------------------------------------

    private Rigidbody2D m_Rigidbody2D;
    /// <summary>
    /// 描述：附着在人物的刚体控件
    /// </summary>
    public Rigidbody2D Cpn_Rigidbody2D
    {
        get { return m_Rigidbody2D; }
    }

    private Collider2D m_Collider2D;
    /// <summary>
    /// 描述：附着在人物的碰撞盒
    /// </summary>
    public Collider2D Cpn_Collider2D
    {
        get { return m_Collider2D; }
    }

    private SpriteRenderer m_SpriteRenderer;
    /// <summary>
    /// 描述：附着在人物的渲染精灵
    /// </summary>
    public SpriteRenderer Cpn_SpriteRenderer
    {
        get { return m_SpriteRenderer; }
    }

    //------------------------------------
    //           血量管理区 ↓
    //------------------------------------

    public float m_HP;
    /// <summary>
    /// 描述：人物的当前血量
    /// </summary>
    public float HP
    {
        get { return m_HP; }
        set
        {
            float newHP = Mathf.Clamp(value, 0, StartHP);
            if (IsDesign == false)
            {
                m_HP = newHP;

                if (m_HP <= 0)
                {
                    GoToDie();
                }
            }
        }
    }

    /// <summary>
    /// 描述：恢复人物的所有血量，并有相对应的特效
    /// </summary>
    public void FullHP()
    {
        HP = StartHP;
        // TODO 增加恢复血量特效
    }


    //------------------------------------
    //          主逻辑管理区 ↓
    //------------------------------------

    public void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Collider2D = GetComponent<Collider2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        gameScene = GameObject.Find("GameScene").GetComponent<GameScene>();
    }

    // 数据初始化
    public void Start()
    {
        m_HP = StartHP;
    }

    // 鼠标拖拽物体时
    public void OnMouseDrag()
    {
        if (IsDesign == true)
        {
            Cpn_Rigidbody2D.MovePosition(GameHelper.MousePoint2D());
            Cpn_Rigidbody2D.MoveRotation(0);
        }
    }

    public void OnMouseDown()
    {
        if (IsDesign == true)
        {
            m_IsSelete = true;
            m_BeforePosition = Cpn_Rigidbody2D.position;
            GameHelper.SpriteSetColor(gameObject, 0.5f);
        }
    }

    public void OnMouseUp()
    {
        if (IsDesign == true)
        {
            IsSelete = false;

            if (IsCanSetUp == false)
            {
                Cpn_Rigidbody2D.MovePosition(m_BeforePosition);
            }
        }
    }

    //------------------------------------
    //          特效管理区 ↓
    //------------------------------------

    /// <summary>
    /// 描述：因特定的效果直接销毁人物并有相应的死亡特效。假如只是扣血而亡，不需要调用该函数。
    /// </summary>
    public void GoToDie()
    {
        // TODO 人物死亡
        Destroy(this.gameObject);
    }



}

