using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentController : MonoBehaviour {

    //------------------------------------
    //          预构体管理区 ↓
    //------------------------------------

    /// <summary>
    /// 描述：垃圾回收车预制体
    /// </summary>
    public GameObject Recoverer = null;
    /// <summary>
    /// 描述：避雷针预制体
    /// </summary>
    public GameObject LightningRob = null;

    //------------------------------------
    //         关键数据管理区 ↓
    //------------------------------------

    /// <summary>
    /// 描述：场景中所用的设备
    /// </summary>
    public List<Controller.Creator> Equipments;

    /// <summary>
    /// 描述：选中的设备
    /// </summary>
    private Controller.Creator m_PreCreate;

    //------------------------------------
    //         关键接口管理区 ↓
    //------------------------------------

    /// <summary>
    /// 描述：选择一种设备
    /// </summary>
    public void ChooseEquipment(Controller.GAMEOBJECT_TYPE type)
    {
        // 准备实例化设备的参数
        if (m_PreCreate.Prefab != null)
        {
            Destroy(m_PreCreate.Prefab.gameObject);
            m_PreCreate.Prefab = null;
            m_PreCreate.Type = Controller.GAMEOBJECT_TYPE.NONE;
        }
       

        GameObject seleteObject = null;
        switch (type)
        {
            case Controller.GAMEOBJECT_TYPE.RECOVERER:
                seleteObject = Recoverer;
                break;
            case Controller.GAMEOBJECT_TYPE.LIGHTNING_ROD:
                seleteObject = LightningRob;
                break;
            default:
                seleteObject = null;
                break;
        }

        // 实例化设备
        if (seleteObject != null)
        {
            m_PreCreate.Prefab = Instantiate(seleteObject, GameHelper.MousePoint2D(), Quaternion.identity);
            m_PreCreate.Type = type;
        }

        // 实例化后初始化设备属性
        var script = m_PreCreate.Prefab.GetComponent<Equipment>();
        script.IsWorking = false;
        script.IsCanWorking = true;
       
    }

    /// <summary>
    /// 描述：实体化已设计好的设备
    /// </summary>
    public bool BuildEquipment()
    {
        bool res = false;
        if (m_PreCreate.Prefab != null)
        {
            var newEquipment = new Controller.Creator
            {
                Prefab = m_PreCreate.Prefab,
                Type = m_PreCreate.Type
            };
            Equipments.Add(newEquipment);

            var script = m_PreCreate.Prefab.GetComponent<Equipment>();
            script.MyController = this;

            if (script.IsCanWorking == true)
            {
                script.IsWorking = true;
                res = true;
            }
            else
            {
                Destroy(m_PreCreate.Prefab.gameObject);
                res = false;
            }
        }

        m_PreCreate.Prefab = null;
        m_PreCreate.Type = Controller.GAMEOBJECT_TYPE.NONE;
        return res;
    }

    /// <summary>
    /// 描述：修理设备并返回修理费用。公式：损坏百分比 * 原价 * 折扣
    /// </summary>
    public float FullHP(GameObject equipment)
    {
        float res = 0.0f;

        if(equipment.tag == "Equipment")
        {
            var S = equipment.GetComponent<Equipment>();
            if(S.HP < S.StartHP)
            {
                float cost = EquipmentCost(S.Type);
                res = (S.StartHP - S.HP) / S.StartHP * cost * GameHelper.RepairDiscount;
                S.FullHP();
            }
        }

        return res;



    }

    /// <summary>
    /// 描述：拆除设备并返回额外金钱。
    /// </summary>
    public float DetroyEquipment(GameObject equipment)
    {
        float res = 0.0f;
        if(equipment.tag == "Equipment")
        {
            var S = equipment.GetComponent<Equipment>();
            float cost = EquipmentCost(S.Type);
            if(S.HP == S.StartHP)
            {
                res = cost;
            }
            else
            {
                res = S.HP / S.StartHP * cost * GameHelper.DestroyDiscount;
            }
        }

        Destroy(equipment);
        return res;
    }

    /// <summary>
    /// 描述：清理设备集合中已完全损坏的设备
    /// </summary>
    private void RemoveNullBuilding()
    {
        GameHelper.RemoveNullObjectInList(ref Equipments);
    }

    /// <summary>
    /// 描述：返回对应设备的原价
    /// </summary>
    private float EquipmentCost(Controller.GAMEOBJECT_TYPE type)
    {
        float cost = 0.0f;
        switch (type)
        {
            case Controller.GAMEOBJECT_TYPE.LIGHTNING_ROD:
                {
                    cost = GameHelper.BaseCost_LightningRob;
                }
                break;
            case Controller.GAMEOBJECT_TYPE.RECOVERER:
                {
                    cost = GameHelper.BaseCost_Recoverer;
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
        Equipments = new List<Controller.Creator>();

        m_PreCreate.Prefab = null;
        m_PreCreate.Type = Controller.GAMEOBJECT_TYPE.NONE;
    }



}
