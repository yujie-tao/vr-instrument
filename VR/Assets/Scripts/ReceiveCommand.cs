using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveCommand : MonoBehaviour
{
	public OSC osc;
    public AudioSource audioSource;
    // private GameObject cube;

    // Start is called before the first frame update
    void Start()
    {
    	osc.SetAddressHandler("/sound", playAudio);
        // cube = GameObject.Find("Cube");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void playAudio(OscMessage message){
        if (message.GetInt(0) == 1){
            Debug.Log(message.GetInt(0));
            audioSource.Play();
            // cube.GetComponent<Renderer>().material.color = new Color(0, 255, 0); 
        }
    }
}
