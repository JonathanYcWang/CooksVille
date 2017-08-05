using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnFruit : MonoBehaviour {
    public GameObject fruit;
    int spawnNum = 1;

    public void spawn()
    {
        for(int i=0; i < spawnNum; i++)
        {
            Vector3 fruitPos = new Vector3(this.transform.position.x + Random.Range(0.3f, 0.3f),
                this.transform.position.y + Random.Range(0.2f, 0.2f),
                this.transform.position.z + Random.Range(0.3f, 0.3f));
			PhotonNetwork.Instantiate(fruit.name, fruitPos, Quaternion.identity, 0);
        }
    }
		
}
