using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour {
    //------------------------------------
    //          外部数据管理区 ↓
    //------------------------------------

    /// <summary>
    /// 描述：初始血量
    /// </summary>
    public float StartHP;

    /// <summary>
    /// 描述：外部 Unity 赋值
    /// </summary>
    public Controller.GAMEOBJECT_TYPE Type;

    private EquipmentController m_MyController;
    /// <summary>
    /// 描述：我的上司——控制者
    /// </summary>
    public EquipmentController MyController
    {
        get { return m_MyController; }
        set { m_MyController = value; }
    }

    //------------------------------------
    //           状态管理区 ↓
    //------------------------------------

    private bool m_IsCanWorking;
    /// <summary>
    /// 描述：设备是否可以实体化
    /// </summary>
    public bool IsCanWorking
    {
        get { return m_IsCanWorking; }
        set
        {
            m_IsCanWorking = value;
            if (IsWorking == false)
            {
                if (value == false)
                {
                    // 不能在该地方实体化
                    GameHelper.SpriteSetColor(gameObject, 1.0f, 0.5f, 0.5f);
                }
                else
                {
                    // 可以在该地方实体化
                    GameHelper.SpriteSetColor(gameObject, 0.5f, 1.0f, 0.5f);
                }
            }
        }
    }

    private bool m_IsWorking;
    /// <summary>
    /// 描述：设备是否已经实体化
    /// </summary>
    public bool IsWorking
    {
        get { return m_IsWorking; }
        set
        {
            m_IsWorking = value;
            if (false == value)
            {
                // 设计版面
                Cpn_Collider2D.enabled = false;
                GameHelper.SpriteSetColor(gameObject, 0.5f);
            }
            else
            {
                // 真实环境
                Cpn_Collider2D.enabled = true;
                GameHelper.SpriteSetColor(gameObject, 1.0f);
                GameHelper.SpriteSetColor(gameObject, 1.0f, 1.0f, 1.0f);
            }
        }
    }

    //------------------------------------
    //           控件管理区 ↓
    //------------------------------------

    private Collider2D m_Collider2D;
    /// <summary>
    /// 描述：物体的碰撞盒
    /// </summary>
    public Collider2D Cpn_Collider2D
    {
        get { return m_Collider2D; }
    }

    private Rigidbody2D m_Rigidbody2D;
    /// <summary>
    /// 描述：物体的刚体
    /// </summary>
    public Rigidbody2D Cpn_Rigidbody2D
    {
        get { return m_Rigidbody2D; }
    }

    //------------------------------------
    //           血量管理区 ↓
    //------------------------------------

    private float m_HP;
    /// <summary>
    /// 描述：设备当前耐久度
    /// </summary>
    public float HP
    {
        get { return m_HP; }
        set
        {
            if (IsWorking == true)
            {
                m_HP = Mathf.Clamp(value, 0.0f, StartHP);

                if (m_HP == 0)
                {
                    GoToDie();
                }
            }
        }
    }

    /// <summary>
    /// 描述：把设备的耐久度恢复为 100%
    /// </summary>
    public void FullHP()
    {
        HP = StartHP;
    }

    //------------------------------------
    //          主逻辑管理区 ↓
    //------------------------------------

    public void Awake()
    {
        m_Collider2D = GetComponent<Collider2D>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        m_HP = StartHP;

    }

    public void FixedUpdate()
    {
        if (false == IsWorking)
        {
            Cpn_Rigidbody2D.MovePosition(GameHelper.MousePoint2D());
        }
    }

    //------------------------------------
    //          特效管理区 ↓
    //------------------------------------

    /// <summary>
    /// 描述：设备完全损坏
    /// </summary>
    void GoToDie()
    {
        Destroy(this.gameObject);
        MyController.SendMessage("RemoveNullBuilding");
    }
}
