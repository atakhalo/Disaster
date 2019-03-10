//*****************************************************************************
//  作者：卢俊廷
//  说明：该脚本为UI提供相关接口
//  
//  使用说明：
//      1. SetLength 用于设置正在选中物体的长度。
//      2. SetRotation 用于设置正在选中物体的旋转角度。
//      3. ChooseBuilding : 选中建材。
//      4. BuildBuilding : 实体化建材。
//*****************************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    //------------------------------------
    //          预构体管理区 ↓
    //------------------------------------

    /// <summary>
    /// 描述：木材预构体
    /// </summary>
    public GameObject woodBuilding = null;
    /// <summary>
    /// 描述：石材预构体
    /// </summary>
    public GameObject stoneBuilding = null;
    /// <summary>
    /// 描述：钢材预构体
    /// </summary>
    public GameObject steelBuilding = null;

    //------------------------------------
    //         关键数据管理区 ↓
    //------------------------------------

    /// <summary>
    /// 描述：场景中所用的建材
    /// </summary>
    public List<Controller.Creator> Buildings;

    /// <summary>
    /// 描述：选中的建材
    /// </summary>
    private Controller.Creator m_PreCreate;
    //private Controller.Creator m_temp;
    
    //------------------------------------
    //         关键接口管理区 ↓
    //------------------------------------

    /// <summary>
    /// 描述：选择一种建材
    /// </summary>
    public void ChooseBuilding(Controller.GAMEOBJECT_TYPE type)
    {
        // 准备实例化建材的参数
        if (m_PreCreate.Prefab != null)
        {
            Destroy(m_PreCreate.Prefab.gameObject);
            m_PreCreate.Prefab = null;
            m_PreCreate.Type = Controller.GAMEOBJECT_TYPE.NONE;
        }

        GameObject seleteObject = null;
        switch (type)
        {
            case Controller.GAMEOBJECT_TYPE.WOOD:
                seleteObject = woodBuilding;
                break;
            case Controller.GAMEOBJECT_TYPE.STONE:
                seleteObject = stoneBuilding;
                break;
            case Controller.GAMEOBJECT_TYPE.STEEL:
                seleteObject = steelBuilding;
                break;
            default:
                seleteObject = null;
                break;
        }

        // 实例化建材
        if (seleteObject != null)
        {
            m_PreCreate.Prefab = Instantiate(seleteObject, GameHelper.MousePoint2D(), Quaternion.identity);
            m_PreCreate.Type = type;  
        }

        // 实例化后初始化建材属性
        m_PreCreate.Prefab.GetComponent<Building>().IsWorking = false;
    }

    /// <summary>
    /// 描述：设置建材的长度
    /// </summary>
    public void SetLength(int length)
    {
        if(m_PreCreate.Prefab != null)
        {
            var script = m_PreCreate.Prefab.GetComponent<Building>();
            script.Length = length;
        }
    }

    /// <summary>
    /// 描述：设置建材的旋转角度（角度制）
    /// </summary>
    /// <param name="angle"></param>
    public void SetRotation(float angle)
    {
        if(m_PreCreate.Prefab != null)
        {
            var script = m_PreCreate.Prefab.GetComponent<Building>();
            script.Rotation = angle;
        }
    }

    /// <summary>
    /// 描述：实体化已设计好的建材
    /// </summary>
    public bool BuildBuilding()
    {
        bool status = false;
        if(m_PreCreate.Prefab != null)
        {
            var newBuilding = new Controller.Creator
            {
                Prefab = m_PreCreate.Prefab,
                Type = m_PreCreate.Type
            };
            Buildings.Add(newBuilding);

            var script = m_PreCreate.Prefab.GetComponent<Building>();
            script.MyController = this;

            if (script.IsCanWorking == true)
            {
                script.IsWorking = true;
                status = true;
            }
            else
            {
                Destroy(m_PreCreate.Prefab.gameObject);
                status = false;
            }
        }
        else
        {
            status = false;
        }

        m_PreCreate.Prefab = null;
        m_PreCreate.Type = Controller.GAMEOBJECT_TYPE.NONE;
        return status;
    }

    /// <summary>
    /// 描述：为建材恢复所有的血量，并返回修理的费用。公式：已损血量比 * 原价 * 折扣。
    /// </summary>
    public float FullHP(GameObject building)
    {
        float res = 0.0f;
        if(building.tag == "Building")
        {
            var S = building.GetComponent<Building>();
            if(S.HP < S.startHP)
            {
                float cost = GameHelper.BuildingCost(S.Type, S.Length);
                res = (S.startHP - S.HP) / S.startHP * cost * GameHelper.RepairDiscount;
                building.GetComponent<Building>().FullHP();
            }         
        }

        return res;
    }

    /// <summary>
    /// 描述：拆除建材并返还金额。
    /// </summary>
    public float DestroyBuilding(GameObject building)
    {
        float res = 0.0f;
        if(building.tag == "Building")
        {
            var S = building.GetComponent<Building>();
            float cost = GameHelper.BuildingCost(S.Type, S.Length);
            if (S.HP == S.startHP)
            {
                res = cost;
            }
            else
            {
                res = S.HP / S.startHP * cost * GameHelper.DestroyDiscount;
            }
        }

        Destroy(building);
        return res;
    }

    /// <summary>
    /// 描述：清理人物集合中已死去的人物
    /// </summary>
    private void RemoveNullBuilding()
    {
        GameHelper.RemoveNullObjectInList(ref Buildings);
    }

    //------------------------------------
    //          主逻辑管理区 ↓
    //------------------------------------

    public void Awake()
    {
        Buildings = new List<Controller.Creator>();
        m_PreCreate = new Controller.Creator();
        m_PreCreate.Prefab = null;
        m_PreCreate.Type = Controller.GAMEOBJECT_TYPE.NONE;
    }

}
