int analogPin = A1;
int val;  // variable to store the value read

void setup() {
  Serial.begin(115200);           //  setup serial
}

void loop() {
  val = analogRead(analogPin);  // read the input pin
  Serial.println(val);          // debug value
}
