using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour {

    /// <summary>
    /// 默认龙卷风在左下角，力向右为正
    /// </summary>
    public Vector2 Force;
    public float Damage_EverySecond;
    [HideInInspector]
    public float HP;

    private float m_Hp;
    // Use this for initialization
    void Start ()
    {
        m_Hp = HP;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Hit(1.0f);
	}

    /// <summary>
    /// 龙卷风的自身的伤害
    /// </summary>
    /// <param name="rate">相对于龙卷风每秒自己扣血的百分比</param>
    public void Hit(float rate)
    {
        m_Hp -= Damage_EverySecond * Time.deltaTime * rate;
        Color  color = GetComponent<SpriteRenderer>().color;
        color.a = m_Hp / HP;
        GetComponent<SpriteRenderer>().color = color;
        if (m_Hp < 0.0f)
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// 龙卷风自身速度衰减百分比
    /// </summary>
    /// <param name="rate"></param>
    public void VelocityChange(float rate)
    {
        if(rate > 1.0f)
        {
            rate = 1.0f;
        }
        else if(rate < 0.0f)
        {
            rate = 0.0f;
        }
        else
        {
            Vector2 velocity = GetComponent<Rigidbody2D>().velocity;
            velocity.x *= (1.0f - rate * Time.deltaTime);
            GetComponent<Rigidbody2D>().velocity = velocity;
        }      
    }

    /// <summary>
    /// 直接改变龙卷风的速度
    /// </summary>
    /// <param name="velocity"></param>
    public void VelocityChange(Vector2 velocity)
    {
        GetComponent<Rigidbody2D>().velocity = velocity;
    }
}
