using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoTrigger : MonoBehaviour {

    public GameObject Parent;

    private Vector3 force;
	// Use this for initialization
	void Start ()
    {
        force = Parent.GetComponent<Tornado>().Force;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.transform.position.x - transform.position.x < 0)                        //如果物体在龙卷风左边
        {
            collision.gameObject.GetComponentInParent<Rigidbody2D>().AddForce(new Vector2(-force.x, force.y));
        }
        else
        {
            collision.gameObject.GetComponentInParent<Rigidbody2D>().AddForce(force);
        }
        switch (collision.tag)
        {
            case "Land":
                {
                }
                break;
            case "Person":
                {
                }
                break;
            case "Building":
                {
                    Parent.GetComponent<Tornado>().Hit(1.5f);
                    //建筑对龙卷风的减速效果
                    Parent.GetComponent<Tornado>().VelocityChange(0.05f);
                }
                break;
            case "Tree":
                {
                    Parent.GetComponent<Tornado>().Hit(3.0f);
                    //树对龙卷风的减速效果
                    Parent.GetComponent<Tornado>().VelocityChange(0.1f);
                }
                break;
            case "Lightning_Rob":
                {
                }
                break;
            case "Recoverer":
                {
                }
                break;
            case "Water":
                { }
                break;
            case "Rubbish":
                { }
                break;
            case "Tornado":
                { }
                break;
            default:
                {
                }
                break;
        }
    }
}
