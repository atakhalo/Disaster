using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonTrigger : Trigger {

    /// <summary>
    /// 描述：人物身体部位枚举
    /// </summary>
    public enum PartType
    {
        /// <summary>
        /// 头
        /// </summary>
        HEAD,
        /// <summary>
        /// 身
        /// </summary>
        BODY,
        /// <summary>
        /// 脚
        /// </summary>
        FOOT
    }

    //------------------------------------
    //           伤害处理区 ↓
    //------------------------------------

    public override void HitToLand(GameObject other, Vector3 dv)
    {
        base.HitToLand(other, dv);

        Cpn_Person.HP -= dv.magnitude * GameHelper.K;
    }

    public override void HitToPerson(GameObject other, Vector3 dv)
    {
        base.HitToPerson(other, dv);

        other.GetComponent<Person>().HP -= dv.magnitude * GameHelper.K;
        Cpn_Person.HP -= dv.magnitude * GameHelper.K;
        
    }

    public override void TouchWithOther(GameObject other, Vector3 dv)
    {
        base.TouchWithOther(other, dv);

        switch(other.tag)
        {
            case "Building":
                {
                    if(TriggerType != PartType.FOOT)
                        Cpn_Person.HP -= 10.0f;
                }
                break;
            default:
                break;
        }
    }

    //------------------------------------
    //           数据管理区 ↓
    //------------------------------------

    /// <summary>
    /// 描述：人物当前部位
    /// </summary>
    public PartType TriggerType;

    private Person m_Person;
    /// <summary>
    /// 描述：存放人物整体逻辑数据的脚本
    /// </summary>
    public Person Cpn_Person
    {
        get { return m_Person; }
    }

    private int m_TouchNum = 0;

    //------------------------------------
    //          主逻辑管理区 ↓
    //------------------------------------

    public override void Awake()
    {
        base.Awake();

        m_Person = gameObject.GetComponentInParent<Person>();
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if(Cpn_Person.IsDesign == true && Cpn_Person.IsSelete == true)
        {
            ++m_TouchNum;
        }
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        if(Cpn_Person.IsDesign == true && Cpn_Person.IsSelete == true)
        {
            --m_TouchNum;
        }
    }

    public void FixedUpdate()
    {
        if(Cpn_Person.IsDesign == true && m_TouchNum == 0)
        {
            Cpn_Person.IsCanSetUp = true;
        }
        else if(Cpn_Person.IsDesign == true && m_TouchNum > 0)
        {
            Cpn_Person.IsCanSetUp = false;
        }
    }
}
