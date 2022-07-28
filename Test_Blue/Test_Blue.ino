#include "BluetoothSerial.h"

BluetoothSerial SerialBT;

void setup() {
  // put your setup code here, to run once:
  SerialBT.begin("ESP32");
}

void loop() {
  // put your main code here, to run repeatedly:
  SerialBT.println("Hola");
  delay(1000);
}
