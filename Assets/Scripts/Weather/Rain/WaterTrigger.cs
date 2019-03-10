using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger : Trigger {

    //------------------------------------
    //           伤害管理区 ↓
    //------------------------------------

    public override void HitToLand(GameObject other, Vector3 dv)
    {
        base.HitToLand(other, dv);

        Destroy(Father);
    }

    public override void HitToPerson(GameObject other, Vector3 dv)
    {
        base.HitToPerson(other, dv);

        float damage = Mathf.Abs(GameHelper.PH - 7.0f) * 10.0f;
        other.GetComponent<Person>().HP -= damage;

        Destroy(Father);
    }

    public override void HitToBuilding(GameObject other, Vector3 dv)
    {
        base.HitToBuilding(other, dv);

        float damage = Mathf.Abs(GameHelper.PH - 7.0f) * 10.0f;
        other.GetComponent<Building>().HP -= damage;

        Destroy(Father);
    }

    public override void HitToEquipment(GameObject other, Vector3 dv)
    {
        base.HitToEquipment(other, dv);

        float damage = Mathf.Abs(GameHelper.PH - 7.0f) * 10.0f;
        other.GetComponent<Equipment>().HP -= damage;

        Destroy(Father);
    }

    public override void HitToTree(GameObject other, Vector3 dv)
    {
        base.HitToTree(other, dv);

        float damage = Mathf.Abs(GameHelper.PH - 7.0f) * 0.1f;
        other.GetComponent<Tree>().HP -= damage;

        Destroy(Father);
    }

    public override void TouchWithOther(GameObject other, Vector3 dv)
    {
        base.TouchWithOther(other, dv);

        Destroy(Father, 1.5f);
    }





    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Destroy(Parent.gameObject);
    //    switch (collision.tag)
    //    {
    //        case "Land":
    //            {

    //            }
    //            break;
    //        case "Person":
    //            {
    //                float damage = GameHelper.PH - 7.0f;
    //                if(damage < 0.0f)
    //                {
    //                    damage = -damage;
    //                }
    //                damage *= 10.0f;
    //                collision.GetComponent<Person>().HP -= damage;
    //            }
    //            break;
    //        case "Building":
    //            {
    //                float damage = GameHelper.PH - 7.0f;
    //                if (damage < 0.0f)
    //                {
    //                    damage = -damage;
    //                }
    //                damage *= 10.0f;
    //                collision.GetComponent<Building>().HP -= damage;
    //            }
    //            break;
    //        case "Lightning_Rod":
    //            {
    //                float damage = GameHelper.PH - 7.0f;
    //                if (damage < 0.0f)
    //                {
    //                    damage = -damage;
    //                }
    //                damage *= 10.0f;
    //                collision.GetComponent<Lightning_Rod>().HP -= damage;

    //            }
    //            break;
    //        case "Recoverer":
    //            {
    //                float damage = GameHelper.PH - 7.0f;
    //                if (damage < 0.0f)
    //                {
    //                    damage = -damage;
    //                }
    //                damage *= 10.0f;
    //                collision.GetComponent<Recoverer>().HP -= damage;

    //            }
    //            break;
    //        case "Tree":
    //            {
    //                float damage = GameHelper.PH - 7.0f;
    //                if (damage < 0.0f)
    //                {
    //                    damage = -damage;
    //                }
    //                damage *= 10.0f;
    //                collision.GetComponent<Person>().HP -= damage;

    //            }
    //            break;
    //        default:
    //            { }
    //            break;
    //    }
    //}
}
