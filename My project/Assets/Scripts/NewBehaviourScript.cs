using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;

public class NewBehaviourScript : MonoBehaviour
{

    public string port = "COM19";
    public int buadrate = 9600;

    public bool isRotating = false;
    public bool isMoving = false;

    private SerialPort sp;
    public float ax, ay, az;   //Aceleraciones en los 3 ejes
    public float preax = 0, preay = 0, preaz = 0;   //Aceleraciones en los 3 ejes
    private float gx, gy, gz;   //Velocidades angulares
    public double ang_x = 0, ang_y = 0, ang_z = 0;
    private double preAng_x = 0, preAng_y = 0, preAng_z = 0;
    public Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Open_Serial();
    }
        // Update is called once per frame

    void Update()
    {
        preAng_x = ang_x;
        preAng_y = ang_y;
        preAng_z = ang_z;

        preax = ax;
        preay = ay;
        preaz = az;   //Aceleraciones en los 3 ejes
        string serialstr = Read_Serial();
        
        if(serialstr != null)
        {
            //Debug.Log(serialstr);
            getRawData (serialstr);
            if(isRotating)
            {
                getAngles(preAng_x, preAng_y, preAng_z);
                setAngles();
            }
            if(isMoving)
            {
               rb.AddForce((ax - preax) * 2,-(az - preaz), (ay - preay)*2,ForceMode.Impulse); 
               //rb.AddForce((ax) * 5,-(az), (ay)*5,ForceMode.Force); 
            }
            //rb.AddForce(-ay,-az,ax,ForceMode.Force);
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
            string[]Aceleraciones = data[1].Split(",");
            Debug.Log(data[1]);
            ax = float.Parse(Aceleraciones[0]);
            Debug.Log(ax);
            ay = float.Parse(Aceleraciones[1]);
            az = float.Parse(Aceleraciones[2]);
        }
        else
            if(data[0] == "W")    //Velocidad
            {
                string[]VelocidadesAng = data[1].Split(",");
                //Debug.Log(data[1]); 
                gx = float.Parse(VelocidadesAng[0]);
                gy = float.Parse(VelocidadesAng[1]);
                gz = float.Parse(VelocidadesAng[2]);
            }
            
    }

    public void getAngles(double preX, double preY, double preZ)
    {
        double gyro_angle_x, gyro_angle_y, gyro_angle_z;
        float ac_angle_x, ac_angle_y, ac_angle_z;
        float alfa = 0.87F;
    
        ac_angle_x = (float)(Mathf.Atan(ay / Mathf.Sqrt(Mathf.Pow(ax, 2) + Mathf.Pow(az, 2)))) * (float)(180.0/3.1416);
        ac_angle_y = (float)(Mathf.Atan(-1 * ax / Mathf.Sqrt(Mathf.Pow(ay, 2) + Mathf.Pow(az, 2)))) * (float)(180.0/3.1416);
        ac_angle_z = (float)(Mathf.Atan( Mathf.Sqrt(Mathf.Pow(ay, 2) + Mathf.Pow(ax, 2)))/ Mathf.Sqrt(Mathf.Pow(az, 2) + Mathf.Pow(ax, 2))) * (float)(180.0/3.1416);

        gyro_angle_x = preX + gx*Time.deltaTime;
        gyro_angle_y = preY + gy*Time.deltaTime;
        gyro_angle_z = preZ + gz*Time.deltaTime;
        
        
        ang_x = alfa * gyro_angle_x + (1 - alfa) * ac_angle_x;
        ang_y = alfa * gyro_angle_y + (1 - alfa) * ac_angle_y;
        ang_z = alfa * gyro_angle_z + alfa * ac_angle_z;
    }

    public void  setAngles()
    {
        Quaternion rotationY = Quaternion.AngleAxis ((float)ang_y*-1,
            new Vector3 (1f, 0f, 0f));
        Quaternion rotationX = Quaternion.AngleAxis ((float)ang_x,
            new Vector3 (0f, 0f, 1f));
        Quaternion rotationZ = Quaternion.AngleAxis ((float)ang_z*-1,
            new Vector3 (0f, 1f, 0f));

        this.transform.rotation = rotationY * rotationX * rotationZ;
    }
}