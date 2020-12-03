/*
 Alex Mazursky

 Builds on:
  Example Bluetooth Serial Passthrough Sketch
 by: Jim Lindblom
 SparkFun Electronics
 date: February 26, 2013
 license: Public domain

 This example sketch converts an RN-42 bluetooth module to
 communicate at 9600 bps (from 115200), and passes any serial
 data between Serial Monitor and bluetooth module.
 */
#include <SoftwareSerial.h>  

int bluetoothTx = 2;  // TX-O pin of bluetooth mate, Arduino D2
int bluetoothRx = 3;  // RX-I pin of bluetooth mate, Arduino D3

SoftwareSerial bluetooth(bluetoothTx, bluetoothRx);

// mic pin
int analogPin = A0;
int val;  // variable to store the value read

// Low-pass filter just to clean some of the noise
#include <filters.h>
const float cutoffFreq   = 200.0;  //Cutoff frequency in Hz
const float samplingTime = 0.001; //Sampling time in seconds.
IIR::ORDER  order  = IIR::ORDER::OD1; // Order (OD1 to OD4)
Filter filter(cutoffFreq, samplingTime, order);
float filteredVal;

// thresholds
const int thresholdHigh = 800;
const int thresholdLow = 300;
bool blow = false; // store last state

// timings
const int debounce = 20; // (ms)
unsigned long currentTime;
unsigned long lastTime;

void setup(){
  Serial.begin(115200);  // Begin the serial monitor at 9600bps

  bluetooth.begin(115200);  // The Bluetooth Mate defaults to 115200bps
  bluetooth.print("$");  // Print three times individually
  bluetooth.print("$");
  bluetooth.print("$");  // Enter command mode
  delay(100);  // Short delay, wait for the Mate to send back CMD
  bluetooth.println("U,9600,N");  // Temporarily Change the baudrate to 9600, no parity
  // 115200 can be too fast at times for NewSoftSerial to relay the data reliably
  bluetooth.begin(9600);  // Start bluetooth serial at 9600
}

void loop(){
  // read and filter the mic
  val = analogRead(analogPin);  // read the input pin
  filteredVal = filter.filterIn(val);
  currentTime = millis();
  // check thresholds to see if a blow 
  if(filteredVal >= thresholdHigh || filteredVal <= thresholdLow){
    // send 1 for blow duration
    bluetooth.println(1);
    blow = true;
    lastTime = millis();
  }
  else if(blow == true && currentTime-lastTime >= debounce){
    bluetooth.println(0);
    blow = false;
  }
  //Serial.println(filteredVal);          // debug value
 }
