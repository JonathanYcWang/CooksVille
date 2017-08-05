using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class seController : MonoBehaviour {

    //output audio source
    public AudioClip seWash;
    public AudioClip seCut;
    public AudioClip seCook;
    public AudioClip seInteract;
    public AudioClip seShake;
    public AudioClip seGrind;

    new AudioSource audio;


	// Use this for initialization
	void Start () {
        audio = GameObject.FindGameObjectWithTag("seManager").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        //interact button
        if (Input.GetKeyDown("e"))
        {
            playInteract();
        }
        if(audio.time > 2f){
                 audio.Stop();
        }
	}

    public void playInteract()
    {
        //audio.PlayOneShot(seInteract, 2f);
        audio.clip = seInteract;
        audio.Play();
    }

    public void playWash()
    {
        audio.clip = seWash;
        audio.Play();
        //audio.PlayOneShot(seWash, 1f);
    }

    public void playCut()
    {
        //audio.PlayOneShot(seCut, 1f);
        //audio.PlayOneShot(seCut, 1f);
        audio.clip = seCut;
        audio.Play();
        audio.Play();
    }

    public void playCook()
    {
        //audio.PlayOneShot(seCook, 2f);
        audio.clip = seCook;
        audio.Play();
    }

    public void playShake()
    {
        audio.clip = seShake;
        audio.Play();
    }

    public void playGrind()
    {
        audio.clip = seGrind;
        audio.Play();
    }
}
