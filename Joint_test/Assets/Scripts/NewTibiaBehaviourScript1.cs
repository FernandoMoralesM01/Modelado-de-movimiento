using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTibiaBehaviourScript1 : MonoBehaviour
{
    
    private float ax, ay, az;   //Aceleraciones en los 3 ejes
    private float gx, gy, gz;   //Velocidades angulares
    public double ang_x, ang_y, ang_z;
    private double preAng_x = 0, preAng_y = 0, preAng_z = 0;
    public Rigidbody rb_tibia;
    public bool isMoving;
    // Start is called before the first frame update
    void Start()
    {
        rb_tibia = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        preAng_x = ang_x; 
        preAng_y = ang_y; 
        preAng_z = ang_z;
    
    }

    public void setSerialData(string serialstr)
    {
       
        getRawData (serialstr);
        getAngles(preAng_x, preAng_y, preAng_z);
        if(isMoving)
            setAngles();
        
    }

    private void getRawData (string rawData)
    {
        string []data = rawData.Split(": ");
        if(data[0] == "S")    //Aceleracion
        { 
            //Debug.Log(data[1]);
            string[]Aceleraciones = data[1].Split(",");
            Debug.Log(Aceleraciones[0]);
            ax = float.Parse(Aceleraciones[0]);
            ay = float.Parse(Aceleraciones[1]);
            az = float.Parse(Aceleraciones[2]);
        }   
        else                  
        {
            if(data[0] == "E")    //Velocidad
            {
                //Debug.Log(data[1]);
                string[]VelocidadesAng = data[1].Split(",");
                gx = float.Parse(VelocidadesAng[0]);
                gy = float.Parse(VelocidadesAng[1]);
                gz = float.Parse(VelocidadesAng[2]); 
            }
        }
        
    }

    private void getAngles(double preX, double preY, double preZ)
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
        ang_z = alfa * gyro_angle_z + (1 - alfa) * ac_angle_z;

    }

    public void  setAngles()
    {
        

        Quaternion rotationY = Quaternion.AngleAxis ((float)ang_y,
            new Vector3 (1f, 0f, 0f));
        Quaternion rotationX = Quaternion.AngleAxis ((float)ang_x,
            new Vector3 (0f, 0f, 1f));
        Quaternion rotationZ = Quaternion.AngleAxis ((float)ang_z,
            new Vector3 (0f, 1f, 0f));

        
        rb_tibia.transform.rotation = rotationY * rotationX * rotationZ;
    }
}
