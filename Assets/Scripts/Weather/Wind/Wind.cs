using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour {

    public Vector2 WindForce;
    public float Wind_Duration;                             //风的持续时间

    private float timer1;
	// Use this for initialization
	void Start ()
    {
        timer1 = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer1 += Time.deltaTime;
        if(timer1 > Wind_Duration)
        {
            DestroyObject(this.gameObject);
        }
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(WindForce);
    }
 
}
