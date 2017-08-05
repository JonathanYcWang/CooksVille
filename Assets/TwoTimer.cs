using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwoTimer : MonoBehaviour {
    Image fillImg;
    public float timeAmt;
    public float time;
	// Use this for initialization
	void Start () {
        fillImg = this.GetComponent<Image>();
        time = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (time < timeAmt)
        {
            time += Time.deltaTime;
            fillImg.fillAmount = time / timeAmt;
        }
        
	}
}
