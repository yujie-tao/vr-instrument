using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMotorState : MonoBehaviour
{
    public OSC osc;

    public string nameMotor = "/state";

    uint motorMode;                 // 0 = manual, 1 = angle target
    uint[] motors = new uint[10];   // motors[0] = thumb MCP, motors[1] = thumb DIP, motors[2] = index MCP, motors[3] = index PIP, ..., motors[10] = pinky PIP
    uint motorBytes;                // decimal representation of motor states
    public string motorHex;         // Hex represenation of motor states

    // Testing functions
    public bool debugMode;
    public bool sendMessage;
    public bool unlockAll;
    public bool lock_Motor;
    public bool unlock_Motor;
    public int motorIndex;

    // Start is called before the first frame update
    void Start()
    {
        // Setting mode to manual
        motorMode = 0;

        // unlocking all motors
        unlockAllMotors();
    }

    // Update is called once per frame
    void Update()
    {
        if (debugMode)
        {
            if (sendMessage) // this needs to be 4 digit hex, otherwise will break the OSC_server
            {
                SendMotors(motorHex);
                sendMessage = false;
            }
            if (unlockAll)
            {
                unlockAllMotors();
                unlockAll = false;
            }
            if (lock_Motor)
            {
                lockMotor(motorIndex);
                lock_Motor = false;
            }
            if (unlock_Motor)
            {
                unlockMotor(motorIndex);
                unlock_Motor = false;
            }
        }
        
    }

    public void SendMotors(String hexMessage)
    {
        OscMessage message = new OscMessage();

        message.address = nameMotor;
        message.values.Add(hexMessage);
        osc.Send(message);

        Debug.Log("Sending OSC: " + hexMessage);
    }

    public void convertMotorStateToHex()
    {
        // set motor mode:
        motorBytes = motorMode << 11;

        // set motor states
        for (int i = 0; i < motors.Length; i++)
        {
            motorBytes = motors[i] << (motors.Length - i - 1) | motorBytes;
            Debug.Log("converting... " + motorBytes);
        }
        Debug.Log("end of conversion " + motorBytes);
        // convert uint to hex
        motorHex = motorBytes.ToString("X4");
    }
    
    // sets motors array to 0 and send OSC command
    public void unlockAllMotors()
    {
        for (int i = 0; i < motors.Length; i++)
        {
            motors[i] = 0;
        }
        convertMotorStateToHex();
        SendMotors(motorHex);
    }

    // unlock 1 motor and send OSC command
    public void unlockMotor(int index)
    {
        motors[index] = 0;
        convertMotorStateToHex();
        SendMotors(motorHex);
    }

    // lock 1 motor and send OSC command
    public void lockMotor(int index)
    {
        motors[index] = 1;
        convertMotorStateToHex();
        SendMotors(motorHex);
    }
}