//*****************************************************************************
//  作者：卢俊廷
//  说明：该脚本附着在建材Perfab中。负责储存并管理建材的主要属性。
//  附着物体的创建：
//      1. 创建一个 Sprite2D 对象，把木材/石材/钢材放在上面。
//      2. 在Transform中设置最小的尺寸，x 对应建材的宽，y 对应建材的长。
//      3. 创建 Box Collider 2D。
//      4. 创建 Rigidbody 2D。
//      5. 把该脚本放在该物体中。
//      6. 在 Inspector 设置脚本中建材的属性。
//      6. 在该物体里面再创建一个空物体，放置脚本 BuildingTrigger。
//
//  使用说明：
//      1. 设置建材的规格、旋转直接对 Length、Rotation 赋值即可。内部已自带
//         越界数值检测，把值控制在合理的范围内，不需要另外判断。
//*****************************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
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

    private BoxCollider2D m_Collider2D;
    /// <summary>
    /// 描述：附着在物体的碰撞体控件。Cpn: Component
    /// </summary>
    public BoxCollider2D Cpn_Collider2D
    {
        get { return m_Collider2D; }
    }

    private SpriteRenderer m_SpriteRenderer;
    /// <summary>
    /// 描述：附着在物体的渲染精灵控件。Cpn: Component
    /// </summary>
    public SpriteRenderer Cpn_SpriteRenderer
    {
        get { return m_SpriteRenderer; }
    }

    //------------------------------------
    //           外部数据管理区 ↓
    //------------------------------------

    /// <summary>
    /// 描述：建材的 Trigger 对象。
    /// </summary>
    public GameObject Trigger;

    private BuildingController m_MyController;
    /// <summary>
    /// 描述：建材的上司——控制者
    /// </summary>
    public BuildingController MyController
    {
        get { return m_MyController; }
        set { m_MyController = value; }
    }

    //------------------------------------
    //           血量管理区 ↓
    //------------------------------------

    /// <summary>
    /// 描述：建材种类
    /// </summary>
    public Controller.GAMEOBJECT_TYPE Type;

    /// <summary>
    /// 描述：建材的初始血量
    /// </summary>
    public float startHP;

    private float m_HP;
    /// <summary>
    /// 描述：建材的当前血量。范围：0 - 初始血量
    /// </summary>
    public float HP
    {
        get { return m_HP; }
        set
        {
            m_HP = Mathf.Clamp(value, 0.0f, startHP);
        }
    }

    /// <summary>
    /// 描述：恢复建材的所有血量，并有相对应的特效
    /// </summary>
    public void FullHP()
    {

        m_HP = startHP;
        // TODO 增加恢复血量特效
    }

    //------------------------------------
    //            尺寸管理区 ↓
    //------------------------------------


    private Vector2 m_InitRendererSize;
    /// <summary>
    /// 描述：建材渲染控件中的 Tile 模式下的 Size。
    /// </summary>
    public Vector2 InitSize
    {
        get { return m_InitRendererSize; }
    }

    /// <summary>
    /// 描述：建材最大尺寸时的长度（int）
    /// </summary>
    public int MaxLength;

    private int m_Length = 1;
    /// <summary>
    /// 描述：建材的当前长度规格（int）
    /// </summary>
    public int Length
    {
        get { return m_Length; }
        set
        {
            if (value >= 1 && value <= MaxLength)
            {
                m_Length = value;
                Vector2 newSize = new Vector2(InitSize.x, InitSize.y * m_Length);
                Cpn_SpriteRenderer.size = newSize;
                Cpn_Collider2D.size = newSize;
                Trigger.GetComponent<BuildingTrigger>().Cpn_BoxCollider2D.size = newSize;
            }
        }
    }

    private float m_Rotation;
    /// <summary>
    /// 描述：建材在设计版面中的旋转角度(角度制)。该值不会跟真实环境中的旋转角度同步。
    /// </summary>
    public float Rotation
    {
        get { return m_Rotation; }
        set
        {
            Cpn_Rigidbody2D.MoveRotation(value);
            m_Rotation = value;
        }

    }



    //------------------------------------
    //            状态管理区 ↓
    //------------------------------------

    private bool m_IsCanWorking;

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
                    Color color = m_SpriteRenderer.color;
                    color.r = 1.0f;
                    color.g = 0.5f;
                    color.b = 0.5f;
                    m_SpriteRenderer.color = color;
                }
                else
                {
                    // 可以在该地方实体化
                    Color color = m_SpriteRenderer.color;
                    color.r = 0.5f;
                    color.g = 1.0f;
                    color.b = 0.5f;
                    m_SpriteRenderer.color = color;
                }
            }
        }
    }

    private bool m_IsWorking;
    /// <summary>
    /// 描述：建材是否在真实环境中
    /// </summary>
    public bool IsWorking
    {
        get { return m_IsWorking; }
        set
        {
            m_IsWorking = value;
            if (false == value)
            {
                m_Collider2D.enabled = false;

                GameHelper.SpriteSetColor(gameObject, 0.5f);
            }
            else
            {
                m_Collider2D.enabled = true;

                GameHelper.SpriteSetColor(gameObject, 1.0f);
                GameHelper.SpriteSetColor(gameObject, 1.0f, 1.0f, 1.0f);
            }

        }
    }

    //------------------------------------
    //          主逻辑管理区 ↓
    //------------------------------------

    // 脚本构造初始化
    private void Awake()
    {
        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        m_Collider2D = gameObject.GetComponent<BoxCollider2D>();
        m_SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        HP = startHP;
        m_InitRendererSize = Cpn_SpriteRenderer.size;
    }

    public void Update()
    {
        if(HP <= 0)
        {
            GoToDie();
        }
        
    }

    // 固定时间更新
    public void FixedUpdate()
    {
        if (false == IsWorking)
        {
            Cpn_Rigidbody2D.MovePosition(GameHelper.MousePoint2D());
        }
    }

    //------------------------------------
    //          方法接口管理区 ↓
    //------------------------------------
    public void GoToDie()
    {
        // 建材完全损坏
        Destroy(this.gameObject);
    }
}

