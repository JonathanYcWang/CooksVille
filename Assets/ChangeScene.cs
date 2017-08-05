using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour {
    void Start()
    {
		
    }
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "doorToKitchen")
        {
            transform.position = new Vector3(77,1,113);
        }
        else if (collision.gameObject.tag == "doorToCity")
        {
            transform.position = new Vector3(114,1,79);
        }
        else if (collision.gameObject.tag == "doorToFarm")
        {
            transform.position = new Vector3(10,1,33);
        }
    }
}
