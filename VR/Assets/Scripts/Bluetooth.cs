using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArduinoBluetoothAPI;
using System;

public class Bluetooth : MonoBehaviour
{
    string message;
    private BluetoothHelper BTHelper;

    private string x;

    void Start () {

        try{
            Debug.Log("Hello");
            x="";
            BTHelper = BluetoothHelper.GetInstance("Yujie Tao");
            Debug.Log(BTHelper.isDevicePaired());
            BTHelper.setTerminatorBasedStream("\n");
            BTHelper.OnConnected += OnBluetoothConnected; //OnBluetoothConnected is a function defined later on
            BTHelper.OnConnectionFailed += OnBluetoothConnectedFailed;

            BTHelper.Connect();
            Debug.Log(BTHelper.isConnected());
            // BTHelper.OnDataReceived += () => {
            //  //this is called when you receive data FROM your arduino
            //  string receivedData;
            //  receivedData = BTHelper.Read(); // returns a string
            //  //since you are sending an array, convert the string to array :
            //  char[] data = receivedData.ToCharArray();

            //  //do Whatever you want
            // };

            BTHelper.OnDataReceived += (helper) => { 
                try{
                    string xx = helper.Read();
                    char[] data = xx.ToCharArray();
                    Debug.Log(data);
                }catch(Exception ex){
                    x += ex.Message;
                }
            };
            
        }catch(Exception ex){
            Debug.Log(ex);
            x = ex.ToString();
        }
    }

    void Update(){
        //  if(BTHelper.Available){
        //     string msg = BTHelper.Read();
        //     Debug.Log(msg);
        // } else {
        //     Debug.Log("No incoming message");
        // }
        // BTHelper.Connect();
        foreach(var d in BTHelper.getPairedDevicesList())
        {
            Debug.Log(d.DeviceName);
        }
        // Debug.Log(BTHelper.getPairedDevicesList().Count);
    }

    void OnBluetoothConnectedFailed(BluetoothHelper helper){
        Debug.Log("Connection Failed");
    }

    void OnBluetoothConnected(BluetoothHelper helper)
    {
        try{
            helper.StartListening();
            helper.SendData("Hi arduino!");
            Debug.Log("Device Connected");
            
        }catch (Exception ex){
            x += ex.ToString();
            Debug.Log(ex.Message);
        }
        
    }


    void OnDestroy()
    {
        if(BTHelper!=null)
            BTHelper.Disconnect();
    }

}
