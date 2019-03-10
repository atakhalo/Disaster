//*****************************************************************************
//  作者：卢俊廷
//  说明：该脚本为UI提供相关接口
//  
//  使用说明：
//  
//*****************************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PersonController : MonoBehaviour {

    //------------------------------------
    //          预构体管理区 ↓
    //------------------------------------


    GameScene gameScene;

    /// <summary>
    /// 描述：人物预制体
    /// </summary>
    public GameObject PersonPrefab = null;

    //------------------------------------
    //         关键数据管理区 ↓
    //------------------------------------

    /// <summary>
    /// 描述：已创建的人物
    /// </summary>
    public List<Controller.Creator> Persons;

    /// <summary>
    /// 人物初始位置
    /// </summary>
    public Vector2[] Position;

    /// <summary>
    /// 描述：当前创建人物的总血量
    /// </summary>
    public float StartHP = 100;

    //------------------------------------
    //         关键接口管理区 ↓
    //------------------------------------

    /// <summary>
    /// 描述：关卡开始时创建人物
    /// </summary>
    public void CreatePerson()
    {
        foreach (Vector2 pos in Position)
        {
            var newPerson = new Controller.Creator
            {
                Prefab = Instantiate(PersonPrefab, pos, Quaternion.identity),
                Type = Controller.GAMEOBJECT_TYPE.PERSON
            };
            var script = newPerson.Prefab.GetComponent<Person>();
            script.MyController = this;
            script.IsDesign = true;
            script.IsCanSetUp = true;
            script.IsSelete = false;
            script.StartHP = StartHP;

            Persons.Add(newPerson);
        }
    }

    /// <summary>
    /// 描述：设置人物在【设计版面】/【灾害版面】状态（无敌可拖动状态）
    /// </summary>
    public void SetAllPersonInDesign(bool isInDesign)
    {
        RemoveNullPerson();

        for (int i=0;i<Persons.Count;++i)
        {
            var script = Persons[i].Prefab.GetComponent<Person>();
            script.IsDesign = isInDesign;
        }
    }

    /// <summary>
    /// 描述：获取当前所有人物的剩余总血量
    /// </summary>
    public float GetNowHP()
    {
        float hp = 0;
        foreach(Controller.Creator target in Persons)
        {
            if (target.Prefab != null)
            {
                hp += target.Prefab.GetComponent<Person>().HP;
            }
        }
        return hp;
    }

    /// <summary>
    /// 描述：获取该关卡中的所有人物的初始总血量
    /// </summary>
    public float GetAllHP()
    {
        return StartHP * Position.Length;
    }

    /// <summary>
    /// 描述：清理人物集合中已死去的人物
    /// </summary>
    private void RemoveNullPerson()
    {
        GameHelper.RemoveNullObjectInList(ref Persons);
    }

    //------------------------------------
    //          主逻辑管理区 ↓
    //------------------------------------

    public void Awake()
    {
        Persons = new List<Controller.Creator>();
        gameScene = GameObject.Find("GameScene").GetComponent<GameScene>();
    }

    private void Update()
    {
        RemoveNullPerson();
        if (Persons.Count == 0)
            gameScene.Die();
    }

    //// 模拟UI测试
    //public void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        CreatePerson();
    //    }

    //    if (Input.GetKeyDown(KeyCode.Z))
    //    {
    //        SetAllPersonInDesign(false);
    //    }

    //    if (Input.GetKeyDown(KeyCode.X))
    //    {
    //        SetAllPersonInDesign(true);
    //    }
    //}

}
