using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {


	public bool triggered;
	private bool notOpened;
	// Use this for initialization
	void Start () {
		triggered = false;
		notOpened = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (notOpened) {
			if (triggered) {
				GetComponent<Animator> ().SetBool ("doorOpen", true);
				notOpened = false;
				StartCoroutine (wait (10));
				return;
			}
		}
		triggered = false;
	}

	IEnumerator wait(float f)
	{
		yield return new WaitForSeconds(f);
		GetComponent<Animator> ().SetBool ("doorOpen", true);
		notOpened = false;
	}
}
