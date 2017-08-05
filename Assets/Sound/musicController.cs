using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class musicController : MonoBehaviour
{

    //output audio source
    //lobby music is defaulted
    public AudioClip musicKitchen;
    public AudioClip musicPlaza;
    public AudioClip musicFarm;

    private bool playPlaza, playKitchen, playFarm;


    new AudioSource audio;


    // Use this for initialization
    void Start()
    {
        playPlaza = false;
        playKitchen = false;
        playFarm = false;
        audio = GameObject.FindGameObjectWithTag("musicManager").GetComponent<AudioSource>();
        audio.loop = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionStay(Collision col){
        if (GetComponent<PhotonView>().isMine)
        {
            if (col.gameObject.tag == "ground" && !playPlaza)
            {
                playPlaza = true;
                playKitchen = false;
                playFarm = false;
                pPlaza();
            }
            else if (col.gameObject.tag == "m_farm" && !playFarm)
            {
                playFarm = true;
                playKitchen = false;
                playPlaza = false;
                pFarm();
            }
            else if (col.gameObject.tag == "m_kitchen" && !playKitchen)
            {
                playKitchen = true;
                playPlaza = false;
                playFarm = false;
                pKitchen();
            }
        }
    }

    public void pKitchen()
    {Debug.Log("kitchen");
        audio.clip = musicKitchen;
        audio.Play();
        //audio.Stop();
        //audio.PlayOneShot(musicKitchen, 1.2F);
    }

    public void pPlaza()
    {Debug.Log("plaza");
        audio.clip = musicPlaza;
        audio.Play();
        //audio.Stop();
        //audio.PlayOneShot(musicPlaza, 1.2F);
    }

    public void pFarm()
    {Debug.Log("farm");
        audio.clip = musicFarm;
        audio.Play();
        //audio.Stop();
        //audio.PlayOneShot(musicFarm, 1.2F);
    }
}
