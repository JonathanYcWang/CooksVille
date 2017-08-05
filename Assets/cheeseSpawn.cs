using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cheeseSpawn : MonoBehaviour
{
    public GameObject cheese;
    int spawnNum = 1;


    public void spawn()
    {
        for (int i = 0; i < spawnNum; i++)
        {
			GameObject c = PhotonNetwork.Instantiate(cheese.name, transform.position, Quaternion.Euler(new Vector3(10, 90, 180)), 0);
	        c.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
    }


}
