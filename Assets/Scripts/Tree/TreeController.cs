//*************************************************************************************************
//  说明：该脚本用于管理树的创建与生成等宏观操作。UI控件只需要使用该脚本操控树。
//*************************************************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 描述：树木集合管理大全
/// </summary>
public class TreeController : MonoBehaviour
{

    //------------------------------------
    //          预构体管理区 ↓
    //------------------------------------

    /// <summary>
    /// 描述：果树预制体
    /// </summary>
    public GameObject fruitTree = null;
    /// <summary>
    /// 描述：橡树预制件
    /// </summary>
    public GameObject oakTree = null;

    //------------------------------------
    //           控件管理区 ↓
    //------------------------------------

    private Transform m_Transform;
    /// <summary>
    /// 描述：附着在物体的2D刚体控件。Cpn: Component
    /// </summary>
    public Transform Cpn_Transform
    {
        get { return m_Transform; }
    }

    //------------------------------------
    //         关键数据管理区 ↓
    //------------------------------------

    /// <summary>
    /// 描述：场景中已生成的所有树
    /// </summary>
    public List<Controller.Creator> Trees;

    /// <summary>
    /// 描述：选中的植物
    /// </summary>
    private Controller.Creator m_PreCreate;

    //------------------------------------
    //         关键接口管理区 ↓
    //------------------------------------

    /// <summary>
    /// 描述：选择一种树
    /// </summary>
    public void ChooseTree(Controller.GAMEOBJECT_TYPE type)
    {
        // 准备实例化树木的参数
        if(m_PreCreate.Prefab != null)
        {
            Destroy(m_PreCreate.Prefab);
            m_PreCreate.Prefab = null;
        }

        GameObject seleteObject = null;
        switch (type)
        {
            case Controller.GAMEOBJECT_TYPE.FRUIT_TREE:
                seleteObject = fruitTree;
                break;
            case Controller.GAMEOBJECT_TYPE.OAK_TREE:
                seleteObject = oakTree;
                break;
            default:
                seleteObject = null;
                break;

        }

        // 实例化树木
        if (seleteObject != null)
        {
            m_PreCreate.Prefab = Instantiate(seleteObject, GameHelper.MousePoint2D(), Quaternion.identity);
            m_PreCreate.Type = type;
        }
        else
        {
            m_PreCreate.Prefab = null;
            m_PreCreate.Type = Controller.GAMEOBJECT_TYPE.NONE;
        }

        // 实例化后初始化树木属性
        var script = m_PreCreate.Prefab.GetComponent<Tree>();
        script.IsWorking = false;
        script.LandPositionY = Cpn_Transform.position.y;
        
    }

    /// <summary>
    /// 描述：实体化一颗树
    /// </summary>
    public bool BuildTree()
    {
        bool res = false;
        var script = m_PreCreate.Prefab.GetComponent<Tree>();
        if (script.IsCanWorking == false)
        {
            // 不能在该位置实体化树木
            Destroy(m_PreCreate.Prefab);
            res = false;
        }
        else
        {
            Controller.Creator newTree = new Controller.Creator
            {
                Prefab = m_PreCreate.Prefab,
                Type = m_PreCreate.Type
            };
            Trees.Add(newTree);

            // 把树木实体化
            script.MyController = this;
            script.IsWorking = res = true;
        }

        m_PreCreate.Prefab = null;
        m_PreCreate.Type = Controller.GAMEOBJECT_TYPE.NONE;
        return res;
    }

    /// <summary>
    /// 描述：一波后计算果树的收益。存活一颗就收益300。
    /// </summary>
    /// <returns></returns>
    public float GetMoneyByTree()
    {
        RemoveNullTree();

        float res = 0.0f;

        foreach(Controller.Creator tree in Trees)
        {
            if(tree.Type == Controller.GAMEOBJECT_TYPE.FRUIT_TREE)
            {
                res += 300.0f;
            }
        }
        return res;
    }

    /// <summary>
    /// 描述：拆除树木并返还金额
    /// </summary>
    public float DestroyTree(GameObject tree)
    {
        float res = 0.0f;

        if(tree.tag == "Tree")
        {
            var S = tree.GetComponent<Tree>();
            float cost = TreeCost(S.Type);
            if(S.HP == S.StartHP)
            {
                res = cost;
            }
            else
            {
                res = S.HP / S.StartHP * cost * GameHelper.DestroyDiscount;
            }
        }

        Destroy(tree);
        return res;
    }

    /// <summary>
    /// 描述：清理树集合中已死去的树
    /// </summary>
    private void RemoveNullTree()
    {
        List<int> index = new List<int>();
        for (int i = 0; i < Trees.Count; ++i)
        {
            if (Trees[i].Prefab == null)
                index.Add(i);
        }

        for (int i = 0; i < index.Count; ++i)
        {
            Trees.RemoveAt(index[i] - i);
        }
    }

    /// <summary>
    /// 描述：返回对应树木原价
    /// </summary>
    private float TreeCost(Controller.GAMEOBJECT_TYPE type)
    {
        float cost = 0.0f;
        switch (type)
        {
            case Controller.GAMEOBJECT_TYPE.FRUIT_TREE:
                {
                    cost = GameHelper.BaseCost_FruitTree;
                }
                break;
            case Controller.GAMEOBJECT_TYPE.OAK_TREE:
                {
                    cost = GameHelper.BaseCost_OakTree;
                }
                break;
            default:
                {
                    cost = 0.0f;
                }
                break;
        }
        return cost;
    }

    //------------------------------------
    //          主逻辑管理区 ↓
    //------------------------------------

    public void Awake()
    {
        m_Transform = GetComponent<Transform>();
        Trees = new List<Controller.Creator>();
        m_PreCreate.Prefab = null;
        m_PreCreate.Type = Controller.GAMEOBJECT_TYPE.NONE;
        
    }
}
