using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;

public class NewBehaviourScript : MonoBehaviour
{

    public string port = "COM5";
    public int buadrate = 9600;

    private SerialPort sp;
    private float ax, ay, az;   //Aceleraciones en los 3 ejes
    private float gx, gy, gz;   //Velocidades angulares
    private double ang_x, ang_y, ang_z;

    // Start is called before the first frame update
    void Start()
    {
        Open_Serial();
        ang_x = 0;
        ang_y = 0;
        ang_z = 0;
    }
        // Update is called once per frame

    void Update()
    {
        double preAng_x = ang_x, preAng_y = ang_y, preAng_z = ang_z;
        string serialstr = Read_Serial();
        
        if(serialstr != null)
        {
            //Debug.Log(serialstr);
            getRawData (serialstr);
            getAngles(preAng_x, preAng_y, preAng_z);
            setAngles();
        }


    }


    public void Open_Serial()
    {
        sp = new SerialPort(port, buadrate);
        sp.ReadTimeout = 100;
        sp.Open();
        Debug.Log("Conectado");
    }

    public string Read_Serial(int timeout = 50)
    {
        string msg;

        sp.ReadTimeout = timeout;
        try 
        {
            msg = sp.ReadLine();
            return msg;
        }
        catch(TimeoutException)
        {
            return null;
        }
    }

    public void getRawData (string rawData)
    {
        string []data = rawData.Split(": ");
        if(data[0] == "A")    //Aceleracion
        {
            string[]Aceleraciones = data[1].Split(", ");
            ax = float.Parse(Aceleraciones[0]);
            ay = float.Parse(Aceleraciones[1]);
            az = float.Parse(Aceleraciones[2]);
        }
        else                    //Velocidad
        {
            string[]VelocidadesAng = data[1].Split(", ");
            gx = float.Parse(VelocidadesAng[0]);
            gy = float.Parse(VelocidadesAng[1]);
            gz = float.Parse(VelocidadesAng[2]);
        }
    }

    public void getAngles(double preX, double preY, double preZ)
    {
        double gyro_angle_x, gyro_angle_y, gyro_angle_z;
        float ac_angle_x, ac_angle_y, ac_angle_z;
        float alfa = 0.86F;
    
        ac_angle_x = (float)(Mathf.Atan(ay / Mathf.Sqrt(Mathf.Pow(ax, 2) + Mathf.Pow(az, 2)))) * (float)(180.0/3.1416);
        ac_angle_y = (float)(Mathf.Atan(-1 * ax / Mathf.Sqrt(Mathf.Pow(ay, 2) + Mathf.Pow(az, 2)))) * (float)(180.0/3.1416);
       

        gyro_angle_x = preX + gx*Time.deltaTime;
        gyro_angle_y = preY + gy*Time.deltaTime;
        gyro_angle_z = preZ + gz*Time.deltaTime;
        
        
        ang_x = alfa * gyro_angle_x + (1 - alfa) * ac_angle_x;
        ang_y = alfa * gyro_angle_y + (1 - alfa) * ac_angle_y;
        ang_z = gyro_angle_z;
    }

    public void  setAngles()
    {
        Quaternion rotationY = Quaternion.AngleAxis ((float)ang_y,
            new Vector3 (1f, 0f, 0f));
        Quaternion rotationX = Quaternion.AngleAxis ((float)ang_x,
            new Vector3 (0f, 0f, 1f));
        Quaternion rotationZ = Quaternion.AngleAxis ((float)ang_z,
            new Vector3 (0f, 1f, 0f));

        this.transform.rotation = rotationY * rotationX * rotationZ;
    }
}
