using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSC_EMS : MonoBehaviour
{
    // OSC
    public OSC osc;

    public string osc_addr_ems = "/ems";

    // EMS - each joint depending on its calibration has its own EMS parameters (channel, intensity, pulse_count, pulse_width, pulse_delay)
    public struct EMSPulse
    {
        public int channel, intensity, pulse_count, pulse_width;
        public float pulse_delay;
    }

    public EMSPulse[] pulse = new EMSPulse[10]; // pulse[0] = thumb flex, pulse[1] = thumb extend, pulse[2] = index flex, pulse[3] = index extend, ..., pulse[9] = pinky extend

    // Testing functions
    public bool debug;
    public bool send_message;
    public int index_ems;
    
    void Start()
    {

    }
    
    void Update()
    {
        if (debug)
        {
            if (send_message)
            {
                SendEMSPulse(index_ems);
                send_message = false;
            }
        } 
    }

    public void Calibrate(int index, int channel, int intensity, int pulse_count, int pulse_width, float pulse_delay)
    {
        pulse[index].channel = channel;
        pulse[index].intensity = intensity;
        pulse[index].pulse_count = pulse_count;
        pulse[index].pulse_width = pulse_width;
        pulse[index].pulse_delay = pulse_delay;
    }

    public void SendEMSPulse(int index)
    {
        OscMessage message = new OscMessage();

        message.address = osc_addr_ems;
        message.values.Add(pulse[index].channel);
        message.values.Add(pulse[index].intensity);
        message.values.Add(pulse[index].pulse_count);
        message.values.Add(pulse[index].pulse_width);
        message.values.Add(pulse[index].pulse_delay);

        osc.Send(message);

        Debug.Log("Sending pulse at CH " + pulse[index].channel + " intensity " + pulse[index].intensity +
        " pulse count " + pulse[index].pulse_count + " pulse width " + pulse[index].pulse_width +
        " delay " + pulse[index].pulse_delay);
    }
}
