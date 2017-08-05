using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarTimer : MonoBehaviour {

	private Image fillImg;
	public float timeAmt;
	public float time;
	public GameObject bar;
	public GameObject barRed;
	public GameObject barBlue;
	private bool active;
	private Transform player;
    public Text countDown;
	// Use this for initialization
	void Start () {
		fillImg = bar.GetComponent<Image> ();
		bar.SetActive(false);
		time = 0;
        countDown.text = "";
	}

	// Update is called once per frame
	void Update () {
		if (active){
			if (time <= timeAmt) {
				time += Time.deltaTime;
				fillImg.fillAmount = time / timeAmt;
                countDown.text = string.Format("{0}s", Convert.ToInt32(timeAmt - time));
            }
            else
            {
                countDown.text = "READY";
            }
			transform.LookAt (player.position + new Vector3(0,transform.position.y,0));
		}
	}

	public void startTimer(){
		active = true;
		time = 0;
		fillImg.fillAmount = 0;
		bar.SetActive(true);
	}

	public void stopTimer(){
		active = false;
		bar.SetActive(false);
        countDown.text = "";
	}

	public void setTimer(float t){
		timeAmt = t;
	}

	public void setPlayer(Transform p){
		player = p;
	}

	public void setImg(string teamColor){
		if (teamColor == "red") {
			bar = barRed;
            countDown.color = Color.red;
		} else {
			bar = barBlue;
            countDown.color = new Color(0, 210, 255);
        }
		fillImg = bar.GetComponent<Image> ();
	}
}
