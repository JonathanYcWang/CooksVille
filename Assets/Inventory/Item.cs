using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public string id;
    public Sprite spriteNeutral;
    public Sprite spriteHighlighted;
    public Sprite spriteDisabled;

	// Use this for initialization
	void Start () {
        id = this.transform.name.Substring(0, this.transform.name.Length - 7);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
 
}
