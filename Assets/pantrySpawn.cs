using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pantrySpawn : MonoBehaviour
{
    public GameObject pantryItem;
    int spawnNum = 1;

    public void spawn()
    {
		Debug.Log ("1");
        for (int i = 0; i < spawnNum; i++)
        {
			Debug.Log ("2");
			PhotonNetwork.Instantiate(pantryItem.name, transform.position, Quaternion.Euler(new Vector3(10, 90, 180)), 0);
			//PhotonNetwork.Instantiate(fruit.name, fruitPos, Quaternion.identity, 0);
        }
    }
}
