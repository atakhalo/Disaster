using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoverer : MonoBehaviour {

    public float Work_Interval = 5.0f;

    private Rubbish_Controller m_Rubbish_Controller;
    private float timer;
	// Use this for initialization
	void Start ()
    {
        timer = 0.0f;
        m_Rubbish_Controller = GameObject.Find("Controller").GetComponent<Controller>().Script_Rubbish;
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
		if(timer > Work_Interval)
        {
            GameObject.FindGameObjectWithTag("Rubbish").GetComponent<Rubbish>().StartDestory();
        }
	}

}
