using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameCountDownSript : MonoBehaviour {
	public float StartTime;

	private float minutes;
	private float seconds;
	private bool keepCounting = true;

	public Text countdownText;
	public Font textFont;

	void Start(){

	}


	// Update is called once per frame
	void Update()
	{
		if (keepCounting) {

			float t =  StartTime - Time.time ;

			minutes = ((int)t / 60);
			seconds = (t % 60);
			countdownText.font = textFont;
			countdownText.text = "Game Time " + minutes.ToString() + ":" + seconds.ToString ("f0");



		}



	}


}

