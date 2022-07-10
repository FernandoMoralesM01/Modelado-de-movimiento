#include "I2Cdev.h"
#include "MPU6050.h"
#include "Wire.h"

MPU6050 segmento_1;
MPU6050 segmento_2;
MPU6050 segemento_3;

int ax, ay, az;
int gx, gy, gz;

float aceleracion[3][3];
float velocidad_ang[3][3];

void setup() 
{
  Serial.begin(57600);         //Iniciando puerto serial
  Wire.begin();               //Iniciando I2C  
  segmento_1.initialize();    //Iniciando el sensor
  if (segmento_1.testConnection()) Serial.println("Sensor iniciado correctamente");
  else Serial.println("Error al iniciar el sensor");
}

void Obtener_acelarcion (int n);
void Obtener_velocidadAng (int n);

void loop() 
{
    segmento_1.getAcceleration(&ax, &ay, &az);
    segmento_1.getRotation(&gx, &gy, &gz);

    Obtener_acelarcion (0);
    Obtener_velocidadAng (0);
    for (int i = 0; i < 3; i++)
    {
      Serial.print(aceleracion [0][i]);
      Serial.print("/");
      Serial.print(velocidad_ang [0][i]);
      Serial.print("\t-\t");
    }
    Serial.print("\n");
}

void Obtener_acelarcion (int n)
{
  aceleracion [n][0] = ax * (9.81/16384.0);
  aceleracion [n][1] = ay * (9.81/16384.0);
  aceleracion [n][2] = az * (9.81/16384.0);
}

void Obtener_velocidadAng (int n)
{
  velocidad_ang [n][0] = gx * (250.0/32768.0);
  velocidad_ang [n][1] = gy * (250.0/32768.0);
  velocidad_ang [n][2] = gz * (250.0/32768.0);
}
