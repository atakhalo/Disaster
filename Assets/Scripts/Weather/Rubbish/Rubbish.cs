using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubbish : MonoBehaviour {

    public float HP;
    public float DyingRate;

    private Vector2 m_LastVelocity;
    private bool m_IsDying;
    private SpriteRenderer m_SpriteRender;

    // Use this for initialization
    void Start ()
    {
        m_IsDying = false;
        m_SpriteRender = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Dying();
	}

    private void FixedUpdate()
    {
        m_LastVelocity = GetComponent<Rigidbody2D>().velocity;
    }

    /// <summary>
    /// 垃圾开始自动销毁
    /// </summary>
    public void StartDestory()
    {
        m_IsDying = true;
    }

    /// <summary>
    /// 垃圾销毁函数
    /// </summary>
    private void Dying()
    {
        if (m_IsDying)
        {
            Color color = m_SpriteRender.color;
            color.a -= Time.deltaTime * DyingRate;
            if (color.a < 0.0f)
            {
                Destroy(this.gameObject);
            }
            else
            {
                m_SpriteRender.color = color;
            }
        }
    }
}
