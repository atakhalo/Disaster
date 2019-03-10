using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTrigger : Trigger
{
    //------------------------------------
    //           控件管理区 ↓
    //------------------------------------

    private BoxCollider2D m_BoxCollider2D;
    /// <summary>
    /// 描述：触发器的 Trigger 碰撞盒
    /// </summary>
    public BoxCollider2D Cpn_BoxCollider2D
    {
        get { return m_BoxCollider2D; }
    }

    //------------------------------------
    //          外部数据管理区 ↓
    //------------------------------------

    private int m_TouchNum = 0;

    private Building m_Building;
    /// <summary>
    /// 描述：建材主要属性脚本
    /// </summary>
    public Building Cpn_Building
    {
        get { return m_Building; }
    }

    //------------------------------------
    //          伤害计算处理区 ↓
    //------------------------------------

    public override void HitToLand(GameObject other, Vector3 dv)
    {
        base.HitToLand(other, dv);

        Cpn_Building.HP -= dv.magnitude * GameHelper.K;
    }

    public override void HitToBuilding(GameObject other, Vector3 dv)
    {
        base.HitToBuilding(other, dv);

        Cpn_Building.HP -= dv.magnitude * GameHelper.K;
        other.GetComponent<Building>().HP -= dv.magnitude * GameHelper.K;
    }

    public override void HitToPerson(GameObject other, Vector3 dv)
    {
        base.HitToPerson(other, dv);

        Cpn_Building.HP -= dv.magnitude * GameHelper.K;
        other.GetComponent<Person>().HP -= dv.magnitude * GameHelper.K;
    }

    //------------------------------------
    //          主逻辑管理区 ↓
    //------------------------------------

    public override void Awake()
    {
        base.Awake();

        m_Building = GetComponentInParent<Building>();
        m_BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void FixedUpdate()
    {
        if (m_TouchNum == 0 && Cpn_Building.IsCanWorking == false)
        {
            Cpn_Building.IsCanWorking = true;
        }
        else if (m_TouchNum > 0 && Cpn_Building.IsCanWorking == true)
        {
            Cpn_Building.IsCanWorking = false;
        }
    }

    public override void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        if (Cpn_Building.IsWorking == false)
        {
            --m_TouchNum;
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (Cpn_Building.IsWorking == false)
        {
            ++m_TouchNum;
        }
    }
}
