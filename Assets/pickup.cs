using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pickup : MonoBehaviour {

    public GameObject inventoryPanel;
    public GameObject[] inventoryIcons;
	List<GameObject> inventoryList;
	private int number = 0;
    
    void Start()
    {
    }

    void OnCollisionEnter(Collision collision)
	{
        
		if (GetComponent<PhotonView>().isMine) {
			
			GameObject i;
			if (collision.gameObject.tag == "red" && number < 5) {
				i = Instantiate (inventoryIcons [0]);
				i.transform.SetParent (inventoryPanel.transform);
				number++;
			} else if (collision.gameObject.tag == "pink" && number < 5) {
				i = Instantiate (inventoryIcons [1]);
				i.transform.SetParent (inventoryPanel.transform);
				number++;
			}
		}
	}
}
