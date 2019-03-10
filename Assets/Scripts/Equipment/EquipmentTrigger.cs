using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentTrigger : Trigger {

    //------------------------------------
    //          外部数据管理区 ↓
    //------------------------------------

    private Equipment m_Equipment;
    /// <summary>
    /// 描述：设备主题脚本
    /// </summary>
    public Equipment Cpn_Equipment
    {
        get { return m_Equipment; }
    }

    private int m_TouchNum = 0;

    //------------------------------------
    //          伤害计算处理区 ↓
    //------------------------------------

    public override void HitToLand(GameObject other, Vector3 dv)
    {
        base.HitToLand(other, dv);

        Cpn_Equipment.HP -= dv.magnitude * GameHelper.K;
    }

    public override void HitToPerson(GameObject other, Vector3 dv)
    {
        base.HitToPerson(other, dv);

        other.GetComponent<Person>().HP -= dv.magnitude * GameHelper.K;
    }

    public override void HitToBuilding(GameObject other, Vector3 dv)
    {
        base.HitToBuilding(other, dv);

        Cpn_Equipment.HP -= dv.magnitude * GameHelper.K;
        other.GetComponent<Building>().HP -= dv.magnitude * GameHelper.K;
    }

    public override void HitToTree(GameObject other, Vector3 dv)
    {
        base.HitToTree(other, dv);

        Cpn_Equipment.HP -= dv.magnitude * GameHelper.K;
        other.GetComponent<Tree>().HP -= dv.magnitude * GameHelper.K;
    }

    public override void HitToEquipment(GameObject other, Vector3 dv)
    {
        base.HitToEquipment(other, dv);

        Cpn_Equipment.HP -= dv.magnitude * GameHelper.K;
        other.GetComponent<Equipment>().HP -= dv.magnitude * GameHelper.K;
    }

    //------------------------------------
    //          主逻辑管理区 ↓
    //------------------------------------

    public override void Awake()
    {
        base.Awake();

        m_Equipment = Father.GetComponent<Equipment>();
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (Cpn_Equipment.IsWorking == false)
        {
            ++m_TouchNum;
        }
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        if (Cpn_Equipment.IsWorking == false)
        {
            --m_TouchNum;
        }
    }

    public void FixedUpdate()
    {
        if (Cpn_Equipment.IsWorking == false)
        {
            // 设计中
            if (m_TouchNum == 0 && Cpn_Equipment.IsCanWorking == false)
                Cpn_Equipment.IsCanWorking = true;
            else if (m_TouchNum > 0 && Cpn_Equipment.IsCanWorking == true)
                Cpn_Equipment.IsCanWorking = false;
        }
    }




}
