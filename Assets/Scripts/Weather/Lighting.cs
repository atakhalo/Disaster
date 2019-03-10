using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting : MonoBehaviour {

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
        if(collision.tag != "Lightning_Rod" && collision.tag != "Land" && collision.tag != "Water")
        {
            Destroy(collision.GetComponentInParent<Transform>().gameObject);
        }
        Destroy(this.gameObject);
    }
}
