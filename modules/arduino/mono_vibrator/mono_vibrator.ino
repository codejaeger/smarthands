int state=0;
#define VIBRA 3

void setup() {
  Serial.begin(9600); // Default communication rate of the Bluetooth module
  pinMode(VIBRA, OUTPUT);
  digitalWrite(VIBRA,HIGH);
}

void loop() {
  if (Serial.available() > 0) { // Checks whether data is comming from the serial port
    {
      state = Serial.read(); // Reads the data from the serial port
      Serial.println(state);
      Serial.flush();
      digitalWrite(VIBRA, HIGH);
      delay(1000);
      digitalWrite(VIBRA, LOW);
      delay(1000);
    }
    Serial.println(Serial.available());
  }
}
