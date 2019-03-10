using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 描述：存放游戏中实用的方法
/// </summary>
public class GameHelper{

    /// <summary>
    /// 建材木头原价
    /// </summary>
    public static float BaseCost_Wood = 100.0f;
    /// <summary>
    /// 建材石头原价
    /// </summary>
    public static float BaseCost_Stone = 150.0f;
    /// <summary>
    /// 建材钢材原价
    /// </summary>
    public static float BaseCost_Steel = 200.0f;
    /// <summary>
    /// 果树原价
    /// </summary>
    public static float BaseCost_FruitTree = 400.0f;
    /// <summary>
    /// 橡树原价
    /// </summary>
    public static float BaseCost_OakTree = 500.0f;
    /// <summary>
    /// 避雷针原价
    /// </summary>
    public static float BaseCost_LightningRob = 1000.0f;
    /// <summary>
    /// 垃圾回收车原价
    /// </summary>
    public static float BaseCost_Recoverer = 1000.0f;

    public static float BuildingCost(Controller.GAMEOBJECT_TYPE type, int length)
    {
        switch(type)
        {
            case Controller.GAMEOBJECT_TYPE.WOOD:
                return BaseCost_Wood * (1.05f - 0.05f * length) * length;
            case Controller.GAMEOBJECT_TYPE.STONE:
                return BaseCost_Stone * (1.05f - 0.05f * length) * length;
            case Controller.GAMEOBJECT_TYPE.STEEL:
                return BaseCost_Steel * (1.05f - 0.05f * length) * length;
            case Controller.GAMEOBJECT_TYPE.OAK_TREE:
                return BaseCost_OakTree;
            case Controller.GAMEOBJECT_TYPE.FRUIT_TREE:
                return BaseCost_FruitTree;
            case Controller.GAMEOBJECT_TYPE.LIGHTNING_ROD:
                return BaseCost_LightningRob;
            case Controller.GAMEOBJECT_TYPE.RECOVERER:
                return BaseCost_Recoverer;
            default:
                return 0.0f;
        }
    }

    public static float K = 0.0f;
    public static float PH = 7.0f;
    /// <summary>
    /// 描述：修理费用折扣
    /// </summary>
    public static float RepairDiscount = 0.8f;
    /// <summary>
    /// 描述：拆除返还折扣
    /// </summary>
    public static float DestroyDiscount = 0.5f;

    /// <summary>
    /// 描述：获取当前鼠标位置对应 2D 空间的位置
    /// </summary>
    public static Vector3 MousePoint2D()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float dz = 0 - ray.origin.z;
        Vector3 dd = ray.direction / ray.direction.z * dz;

        return ray.origin + dd;
    }

    /// <summary>
    /// 描述：删除 Creator 里面所有的 null 对象。
    /// </summary>
    public static void RemoveNullObjectInList(ref List<Controller.Creator> list)
    {
        List<int> index = new List<int>();
        for (int i = 0; i < list.Count; ++i)
        {
            if (list[i].Prefab == null)
                index.Add(i);
        }

        for (int i = 0; i < index.Count; ++i)
        {
            list.RemoveAt(index[i] - i);
        }
    }
    /// <summary>
    /// 描述：删除GameObject List里面所有的 null 对象
    /// </summary>
    /// <param name="list"></param>
    public static void RemoveNullGameObjectInList(ref List<GameObject> list)
    {
        List<int> index = new List<int>();
        for (int i = 0; i < list.Count; ++i)
        {
            if (list[i] == null)
                index.Add(i);
        }

        for (int i = 0; i < index.Count; ++i)
        {
            list.RemoveAt(index[i] - i);
        }
    }
    
    /// <summary>
    /// 描述：设置渲染精灵的颜色
    /// </summary>
    public static void SpriteSetColor(GameObject item, float r, float g, float b)
    {
        SpriteRenderer sprite = item.GetComponent<SpriteRenderer>();
        if (sprite == null)
            return;

        Color sColor = sprite.color;
        sColor.r = r;
        sColor.g = g;
        sColor.b = b;
        sprite.color = sColor;
    }

    /// <summary>
    /// 描述：设置渲染精灵的透明度
    /// </summary>
    public static void SpriteSetColor(GameObject item, float alpha)
    {
        SpriteRenderer sprite = item.GetComponent<SpriteRenderer>();
        if (sprite == null)
            return;

        Color sColor = sprite.color;
        sColor.a = alpha;
        sprite.color = sColor;
    }
 
}
