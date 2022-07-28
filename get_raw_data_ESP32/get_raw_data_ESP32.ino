#include <Adafruit_MPU6050.h>
#include "BluetoothSerial.h"

BluetoothSerial SerialBT;


#define NUM_MPU 3

int mpuSelect = 0; 
int pinSelect[NUM_MPU] = {15, 2, 4};


Adafruit_MPU6050 mpu;

Adafruit_Sensor *mpu_temp, *mpu_accel, *mpu_gyro;

sensors_event_t accel;
sensors_event_t gyro;

void setup() 
{
  SerialBT.begin("ESP32");
  Serial.begin(9600);
  //while (!Serial)
  //  delay(10); // will pause Zero, Leonardo, etc until serial console opens
  if (!mpu.begin()) {
      Serial.println("Failed");
      while (1) {
        delay(10);
      }
    }
  Serial.println("Conectado");
  SerialBT.println("Conectado");
  mpu_accel = mpu.getAccelerometerSensor();
  mpu_gyro = mpu.getGyroSensor();
  
  for(int i = 0; i < NUM_MPU; i++)
    pinMode(pinSelect[i], OUTPUT);
  
  
}

void Seleccionar_mpu();
void Obtener_aceleracion(int N_mpu);
void Obtener_velocidadAng(int N_mpu);
void Manda_serial(int N_mpu);

void loop() 
{
  Seleccionar_mpu();
  Obtener_aceleracion();
  Obtener_velocidadAng();
  Manda_serial();
  mpuSelect++;
  if(mpuSelect > NUM_MPU)
      mpuSelect = 0;
}

void Seleccionar_mpu()
{
  for (int i = 0; i < NUM_MPU; i++)
    if(i == mpuSelect)
      digitalWrite(pinSelect[i], LOW);
    else
      digitalWrite(pinSelect[i], HIGH);
  delay(10);
  
}

void Obtener_aceleracion()
{
  mpu_accel->getEvent(&accel);
}

void Obtener_velocidadAng()
{
  mpu_gyro->getEvent(&gyro);
}

void Manda_serial()
{
  String str_aceleracion;
  String str_velocidadAng;

  switch (mpuSelect)
  {
    case 0:
      str_aceleracion = "A: " + String (accel.acceleration.x, 3) + "," + String (accel.acceleration.z, 3) + "," + String (accel.acceleration.y, 3);
      str_velocidadAng = "W: " + String (gyro.gyro.x, 3) + "," + String (gyro.gyro.z, 3) + "," + String (gyro.gyro.y, 3);
      Serial.println(str_aceleracion);
      SerialBT.println(str_aceleracion);
      Serial.println(str_velocidadAng);
      SerialBT.println(str_velocidadAng);
    break;
    case 1:
      str_aceleracion = "S: " + String (accel.acceleration.x, 3) + "," + String (accel.acceleration.z, 3) + "," + String (accel.acceleration.y, 3);
      str_velocidadAng = "E: " + String (gyro.gyro.x, 3) + "," + String (gyro.gyro.z, 3) + "," + String (gyro.gyro.y, 3);
      Serial.println(str_aceleracion);
      SerialBT.println(str_aceleracion);
      Serial.println(str_velocidadAng);
      SerialBT.println(str_velocidadAng);
    break;
    default:
    break;
  }
  
  
  //delay(10);
}
