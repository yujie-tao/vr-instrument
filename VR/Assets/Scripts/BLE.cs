using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArduinoBluetoothAPI;
using System;

public class BLE : MonoBehaviour
{
    public AudioSource audioSource;
    private BluetoothHelper bluetoothHelper;
    private float timer;

    private string deviceName = "Adafruit Bluefruit LE";
    private string serverUUID = "6E400001-B5A3-F393-E0A9-E50E24DCCA9E";
    private string charUUID = "6E400003-B5A3-F393-E0A9-E50E24DCCA9E";
    private int _last = 0;

    void Start()
    {
        timer = 0;
        try{
            Debug.Log("HI");
            
            BluetoothHelper.BLE = true;  //use Bluetooth Low Energy Technology
            bluetoothHelper = BluetoothHelper.GetInstance();
            bluetoothHelper.OnConnected += (helper) => {
                List<BluetoothHelperService> services = helper.getGattServices();
                foreach (BluetoothHelperService s in services)
                {
                    Debug.Log("Service : " + s.getName());
                    foreach (BluetoothHelperCharacteristic item in s.getCharacteristics())
                    {
                        Debug.Log(item.getName());
                    }
                }

                Debug.Log("Connected");
                BluetoothHelperCharacteristic c = new BluetoothHelperCharacteristic(charUUID);
                c.setService(serverUUID);
                bluetoothHelper.Subscribe(c);
                //sendData();
            };
            bluetoothHelper.OnConnectionFailed += (helper)=>{
                Debug.Log("Connection failed");
            };
            bluetoothHelper.OnScanEnded += OnScanEnded;
            bluetoothHelper.OnServiceNotFound += (helper, serviceName) =>
            {
                Debug.Log(serviceName);
            };
            bluetoothHelper.OnCharacteristicNotFound += (helper, serviceName, characteristicName) =>
            {
                Debug.Log(characteristicName);
            };
            bluetoothHelper.OnCharacteristicChanged += (helper, value, characteristic) =>
            {
                // Debug.Log(characteristic.getName());
                Debug.Log(value[0]);
                if (value[0] == 49 && _last != value[0]){
                    audioSource.Play();
                }
                _last = value[0];
            };

            // BluetoothHelperService service = new BluetoothHelperService("FFE0");
            // service.addCharacteristic(new BluetoothHelperCharacteristic("FFE1"));
            // BluetoothHelperService service2 = new BluetoothHelperService("180A");
            // service.addCharacteristic(new BluetoothHelperCharacteristic("2A24"));
            // bluetoothHelper.Subscribe(service);
            // bluetoothHelper.Subscribe(service2);
            // bluetoothHelper.ScanNearbyDevices();

            // BluetoothHelperService service = new BluetoothHelperService("19B10000-E8F2-537E-4F6C-D104768A1214");
            // service.addCharacteristic(new BluetoothHelperCharacteristic("19B10001-E8F2-537E-4F6C-D104768A1214"));
            //BluetoothHelperService service2 = new BluetoothHelperService("180A");
            //service.addCharacteristic(new BluetoothHelperCharacteristic("2A24"));
            // bluetoothHelper.Subscribe(service);
            //bluetoothHelper.Subscribe(service2);
            bluetoothHelper.ScanNearbyDevices();

        }catch(Exception ex){
            Debug.Log(ex.StackTrace);
        }
    }

    private void OnScanEnded(BluetoothHelper helper, LinkedList<BluetoothDevice> devices){
        Debug.Log("FOund " + devices.Count);
        if(devices.Count == 0){
            bluetoothHelper.ScanNearbyDevices();
            return;
        }

        foreach(var d in devices)
        {
            Debug.Log(d.DeviceName);
        }
            
        try
        {
            bluetoothHelper.setDeviceName(deviceName);
            bluetoothHelper.Connect();
            Debug.Log("Connecting");
        }catch(Exception ex)
        {
            bluetoothHelper.ScanNearbyDevices();
            Debug.Log(ex.Message);
        }

    }

    void OnDestroy()
    {
        if (bluetoothHelper != null)
            bluetoothHelper.Disconnect();
    }

    void Update(){
        if(bluetoothHelper == null)
            return;
        if(!bluetoothHelper.isConnected())
            return;
        // timer += Time.deltaTime;

        // if(timer < 5)
        //     return;
        // timer = 0;
        read();
    }

    void sendData(){
        // Debug.Log("Sending");
        // BluetoothHelperCharacteristic ch = new BluetoothHelperCharacteristic("FFE1");
        // ch.setService("FFE0"); //this line is mandatory!!!
        // bluetoothHelper.WriteCharacteristic(ch, new byte[]{0x44, 0x55, 0xff});

        Debug.Log("Sending");
        BluetoothHelperCharacteristic ch = new BluetoothHelperCharacteristic(charUUID);
        ch.setService(serverUUID); //this line is mandatory!!!
        bluetoothHelper.WriteCharacteristic(ch, "10001000"); //string: 10001000 is this binary? no, as string.
    }

    void read(){
        BluetoothHelperCharacteristic ch = new BluetoothHelperCharacteristic(charUUID);
        ch.setService(serverUUID);//this line is mandatory!!!

        bluetoothHelper.ReadCharacteristic(ch);
        //Debug.Log(System.Text.Encoding.ASCII.GetString(x));
    }

}