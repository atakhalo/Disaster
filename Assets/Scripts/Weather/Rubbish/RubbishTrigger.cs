using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbishTrigger : MonoBehaviour {

    public GameObject Parent;
    /// <summary>
    /// 垃圾的光环扣血效果
    /// </summary>
    public float Damage;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Land":
                {
                }
                break;
            case "Person":
                {
                }
                break;
            case "Lightning_Rod":
                {
                    float damage = (GetComponentInParent<Rigidbody2D>().velocity - GetComponentInParent<Rigidbody2D>().velocity).magnitude;
                    //GetComponentInParent<Rubbish>().HP -= damage;
                    //GetComponentInParent<Lightning_Rod>().HP -= damage;
                }
                break;
            case "Recoverer":
                {
                    float damage = (GetComponentInParent<Rigidbody2D>().velocity - GetComponentInParent<Rigidbody2D>().velocity).magnitude;
                    //GetComponentInParent<Rubbish>().HP -= damage;
                    //GetComponentInParent<Recoverer>().HP -= damage;
                }
                break;
            case "Tree":
                {
                    float damage = (GetComponentInParent<Rigidbody2D>().velocity - GetComponentInParent<Rigidbody2D>().velocity).magnitude;
                    //GetComponentInParent<Rubbish>().HP -= damage;
                    //GetComponentInParent<Tree>().HP -= damage;
                }
                break;
            case "Water":
                {

                }
                break;
            case "Rubbish":
                {

                }
                break;
            default:
                {
                }
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Person":
                {
    
                    collision.GetComponentInParent<Person>().HP -= Damage;
                }
                break;
            case "Building":
                {
                    collision.GetComponentInParent<Building>().HP -= Damage;
                }
                break;
            case "Lightning_Rod":
                {
                    //collision.GetComponent<Lightning_Rob>().HP -= Damage;
                }
                break;
            default:
                { }
                break;
        }
    }
}
