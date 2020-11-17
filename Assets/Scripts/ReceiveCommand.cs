using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveCommand : MonoBehaviour
{
	public OSC osc;
    // Start is called before the first frame update
    void Start()
    {
    	osc.SetAddressHandler("/motor", testOSC);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void testOSC(OscMessage message){
        Debug.Log(message.GetInt(0));
    }
}
