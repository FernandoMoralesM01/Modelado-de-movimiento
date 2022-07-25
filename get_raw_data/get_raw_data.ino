#include "I2Cdev.h"
#include "MPU6050.h"
#include "Wire.h"
#include "stdio.h"

#define NUM_MPU 3

MPU6050 MPU_1(0x68);
MPU6050 MPU_2(0x69);

int16_t ax, ay, az;
int16_t gx, gy, gz;

float aceleracion[NUM_MPU][3];
float velocidad_ang[NUM_MPU][3];

void setup()
{
  Serial.begin(9600);         //Iniciando puerto serial
  Serial.print("H");
  Wire.begin();               //Iniciando I2C
  MPU_1.initialize();    //Iniciando el sensor
  if (MPU_1.testConnection()) Serial.println("s");
  else Serial.println("no");
  //delay(2000);
  MPU_2.initialize();    //Iniciando el sensor
  if (MPU_2.testConnection()) Serial.println("s");
  else Serial.println("no");
  //delay(2000);
 
}   

void Obtener_acelarcion (int n);
void Obtener_velocidadAng (int n);
void Manda_Serial(int n);

void loop()
{
  //MPU_1.getAcceleration(&ax, &ay, &az);
  //MPU_1.getRotation(&gx, &gy, &gz);
  MPU_1.getMotion6(&ax, &ay, &az, &gx, &gy, &gz);
  
  Obtener_acelarcion (0);
  Obtener_velocidadAng (0);
  Manda_Serial(0);

  //delay(100);
  
  //MPU_2.getAcceleration(&ax, &ay, &az);
  //MPU_2.getRotation(&gx, &gy, &gz);
  MPU_1.getMotion6(&ax, &ay, &az, &gx, &gy, &gz);
  
  Obtener_acelarcion (1);
  Obtener_velocidadAng (1);
  Manda_Serial(1);
 
}


void Obtener_acelarcion (int n)
{
  aceleracion [n][0] = ax * (9.81 / 16384.0);
  aceleracion [n][1] = ay * (9.81 / 16384.0);
  aceleracion [n][2] = az * (9.81 / 16384.0);
}

void Obtener_velocidadAng (int n)
{
  velocidad_ang [n][0] = gx * (250.0 / 32768.0);
  velocidad_ang [n][1] = gy * (250.0 / 32768.0);
  velocidad_ang [n][2] = gz * (250.0 / 32768.0);
}

void Manda_Serial(int n)
{
  String str_aceleracion;
  String str_velocidadAng;
  if(n == 0)
  {
    str_aceleracion = "A: " + String (aceleracion[n][2], 3) + "," + String (aceleracion[n][1], 3) + "," + String (aceleracion[n][0], 3);
    str_velocidadAng = "W: " + String (velocidad_ang[n][2], 3) + "," + String (velocidad_ang[n][1], 3) + "," + String (velocidad_ang[n][0], 3);
  }
  else
  {
    str_aceleracion = "p: " + String (aceleracion[n][2], 3) + "," + String (aceleracion[n][1], 3) + "," + String (aceleracion[n][0], 3);
    str_velocidadAng = "l: " + String (velocidad_ang[n][2], 3) + "," + String (velocidad_ang[n][1], 3) + "," + String (velocidad_ang[n][0], 3);
  }
  
  Serial.println(str_aceleracion);
  Serial.println(str_velocidadAng);
}
