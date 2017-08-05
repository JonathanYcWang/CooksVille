using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class waiting : MonoBehaviour {
	
	private float wait;
	public Text countdown;
	// Use this for initialization
	void Start () {
		wait = 3f;
	}
	
	// Update is called once per frame
	void Update () {
		countdown.text = string.Format("Game starting in : {0} s", Convert.ToInt32(wait));
		wait -= Time.deltaTime;
        Debug.Log("waiting...");
		if (wait < 0){
			this.gameObject.SetActive(false);
		}
	}
}
