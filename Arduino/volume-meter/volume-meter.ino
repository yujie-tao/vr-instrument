#include <SoftwareSerial.h>  

int bluetoothTx = 2;  // TX-O pin of bluetooth mate, Arduino D2
int bluetoothRx = 3;  // RX-I pin of bluetooth mate, Arduino D3

SoftwareSerial bluetooth(bluetoothTx, bluetoothRx);

// mic pin
int analogPin = A0;
float val;
int sampleRaw;

// Low-pass filter just to clean some of the noise
#include <filters.h>
const float cutoffFreq   = 10.0;  //Cutoff frequency in Hz
const float samplingTime = 0.0005; //Sampling time in seconds.
IIR::ORDER  order  = IIR::ORDER::OD1; // Order (OD1 to OD4)
Filter filter(cutoffFreq, samplingTime, order);
float filteredVal;

// thresholds
const int threshold = 24;
bool blow = false; // store last state

// timings
const int debounce = 500; // (ms)
unsigned long currentTime;
unsigned long lastTime;

void setup() {
  // Open serial communications and wait for port to open:
  // A baud rate of 115200 is used instead of 9600 for a faster data rate
  // on non-native USB ports
  Serial.begin(115200);

  bluetooth.begin(115200);  // The Bluetooth Mate defaults to 115200bps
  bluetooth.print("$");  // Print three times individually
  bluetooth.print("$");
  bluetooth.print("$");  // Enter command mode
  delay(100);  // Short delay, wait for the Mate to send back CMD
  bluetooth.println("U,9600,N");  // Temporarily Change the baudrate to 9600, no parity
  // 115200 can be too fast at times for NewSoftSerial to relay the data reliably
  bluetooth.begin(9600);  // Start bluetooth serial at 9600
}

#define SAMPLES 4 // make it a power of two for best DMA performance

void loop() {
  // read a bunch of samples:
  int samples[SAMPLES];

  for (int i=0; i<SAMPLES; i++) {
    float sample = 0; 
    while ((sample == 0) || (sample == -1) ) {
      sample = analogRead(analogPin);
      //sample = filter.filterIn(sampleRaw);
    }
    // convert to 18 bit signed
    //sample >>= 14; 
    samples[i] = sample;
  }

  // ok we hvae the samples, get the mean (avg)
  float meanval = 0;
  for (int i=0; i<SAMPLES; i++) {
    meanval += samples[i];
  }
  meanval /= SAMPLES;
  //Serial.print("# average: " ); Serial.println(meanval);

  // subtract it from all samples to get a 'normalized' output
  for (int i=0; i<SAMPLES; i++) {
    samples[i] -= meanval;
    //Serial.println(samples[i]);
  }

  // find the 'peak to peak' max
  float maxsample, minsample;
  minsample = 100000;
  maxsample = -100000;
  for (int i=0; i<SAMPLES; i++) {
    minsample = min(minsample, samples[i]);
    maxsample = max(maxsample, samples[i]);
  }
  
  val = maxsample - minsample;
  val = filter.filterIn(val);
  Serial.println(val);          // debug value
  
  currentTime = millis();
  
  // check thresholds to see if a blow 
  if(int(val) >= threshold && currentTime-lastTime >= debounce){
    // send 1 for blow duration
    bluetooth.println(1);
    blow = true;
    lastTime = millis();
  }
  else if(blow == true && currentTime-lastTime >= debounce){
    bluetooth.println(0);
    blow = false;
  }
}
