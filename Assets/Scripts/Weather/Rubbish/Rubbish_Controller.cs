using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubbish_Controller : MonoBehaviour {

    /// <summary>
    /// 存垃圾的Prefab
    /// </summary>
    public GameObject[] Prefab_Rubbish;
    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    /// <summary>
    /// 生成一个垃圾
    /// </summary>
    /// <param name="position"></param>
    /// <param name="velocity"></param>
    public  void CreateRubbish(Vector3 position, Vector2 velocity, float HP, float range)
    {
        int types = Prefab_Rubbish.Length;
        int num = (int)Mathf.Round(HP);
        for( int i = 0; i < num; i++)
        {
            GameObject rubbish = Instantiate(Prefab_Rubbish[Random.Range(0, types)], position + new Vector3(Random.Range(0.0f, range), Random.Range(0.0f, range), 0.0f), Quaternion.identity);
            rubbish.GetComponent<Rigidbody2D>().velocity = velocity;
        }
        

    }
}
