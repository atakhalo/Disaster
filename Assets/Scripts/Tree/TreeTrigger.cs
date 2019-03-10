using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeTrigger : Trigger
{

    //------------------------------------
    //          外部数据管理区 ↓
    //------------------------------------

    private Tree m_Tree;
    /// <summary>
    /// 描述：建材主要属性脚本
    /// </summary>
    public Tree Cpn_Tree
    {
        get { return m_Tree; }
    }

    private int m_TouchNum = 0;

    //------------------------------------
    //          伤害计算处理区 ↓
    //------------------------------------

    public override void HitToPerson(GameObject other, Vector3 dv)
    {
        base.HitToPerson(other, dv);

        m_Tree.HP -= dv.magnitude * GameHelper.K;
        other.GetComponent<Person>().HP -= dv.magnitude * GameHelper.K;
    }

    public override void HitToBuilding(GameObject other, Vector3 dv)
    {
        base.HitToBuilding(other, dv);

        m_Tree.HP -= dv.magnitude * GameHelper.K;
        other.GetComponent<Building>().HP -= dv.magnitude * GameHelper.K;
    }

    public override void HitToTree(GameObject other, Vector3 dv)
    {
        base.HitToTree(other, dv);

        m_Tree.HP -= dv.magnitude * GameHelper.K;
        other.GetComponent<Tree>().HP -= dv.magnitude * GameHelper.K;
    }

    //------------------------------------
    //          主逻辑管理区 ↓
    //------------------------------------

    public override void Awake()
    {
        base.Awake();

        m_Tree = GetComponentInParent<Tree>();
    }

    public void FixedUpdate()
    {
        if (Cpn_Tree.IsWorking == false)
        {
            // 设计中
            if (m_TouchNum == 0 && Cpn_Tree.IsCanWorking == false)
                Cpn_Tree.IsCanWorking = true;
            else if (m_TouchNum > 0 && Cpn_Tree.IsCanWorking == true)
                Cpn_Tree.IsCanWorking = false;
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (Cpn_Tree.IsWorking == false)
        {
            ++m_TouchNum;
        }
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        if (Cpn_Tree.IsWorking == false)
        {
            --m_TouchNum;
        }
    }


}
