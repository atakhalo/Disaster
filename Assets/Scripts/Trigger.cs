using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

    //------------------------------------
    //          控件管理区 ↓
    //------------------------------------

    public GameObject Father;

    //------------------------------------
    //          控件管理区 ↓
    //------------------------------------

    private Rigidbody2D m_FatherRigidbody;
    /// <summary>
    /// 描述：父亲的2D刚体
    /// </summary>
    public Rigidbody2D FatherRigidbody
    {
        get { return m_FatherRigidbody; }
    }

    private Collider2D m_Collider2D;
    /// <summary>
    /// 描述：触发器的碰撞盒
    /// </summary>
    public Collider2D Cpn_Collider2D
    {
        get { return m_Collider2D; }
    }


    //------------------------------------
    //          主逻辑管理区 ↓
    //------------------------------------

    public virtual void Awake()
    {
        Father = GetComponentsInParent<Transform>()[1].gameObject;

        m_FatherRigidbody = Father.GetComponent<Rigidbody2D>();
        m_Collider2D = GetComponent<Collider2D>();
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        Rigidbody2D otherRB = other.GetComponent<Rigidbody2D>();

        Vector3 dv;
        if (otherRB != null)
        {
            dv = FatherRigidbody.velocity - other.GetComponent<Rigidbody2D>().velocity;
        }
        else
        {
            dv = FatherRigidbody.velocity;
        }

        switch (other.tag)
        {
            case "Land":
                HitToLand(other,dv);
                break;
            case "Person":
                HitToPerson(other,dv);
                break;
            case "Building":
                HitToBuilding(other,dv);
                break;
            case "Tree":
                HitToTree(other,dv);
                break;
            case "Equipment":
                HitToEquipment(other,dv);
                break;
            case "Rubbish":
                HitToRubbish(other,dv);
                break;
            case "Disastor":
                HitToDisaster(other,dv);
                break;
            default:
                break;
        }
    }

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    public virtual void OnTriggerStay2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        Rigidbody2D otherRB = other.GetComponent<Rigidbody2D>();

        Vector3 dv;
        if (otherRB != null)
        {
            dv = FatherRigidbody.velocity - other.GetComponent<Rigidbody2D>().velocity;
        }
        else
        {
            dv = FatherRigidbody.velocity;
        }

        TouchWithOther(other, dv);
    }

    //------------------------------------
    //          重写接口区 ↓
    //------------------------------------

    public virtual void HitToLand(GameObject other, Vector3 dv) { }
    public virtual void HitToPerson(GameObject other, Vector3 dv) { }
    public virtual void HitToBuilding(GameObject other, Vector3 dv) { }
    public virtual void HitToTree(GameObject other, Vector3 dv) { }
    public virtual void HitToEquipment(GameObject other, Vector3 dv) { }
    public virtual void HitToRubbish(GameObject other, Vector3 dv) { }
    public virtual void HitToDisaster(GameObject other, Vector3 dv) { }

    public virtual void TouchWithOther(GameObject other, Vector3 dv) { }
    
}
