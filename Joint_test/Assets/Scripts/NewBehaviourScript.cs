using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;

public class NewBehaviourScript : MonoBehaviour
{
    NewFemurBehaviourScript Femur;
    NewTibiaBehaviourScript1 Tibia;
    public string port = "COM16";
    public int buadrate = 115200;
    public bool isShowMsg = false;

    private SerialPort sp;

    
    // Start is called before the first frame update
    void Start()
    {
        Femur = GameObject.FindGameObjectWithTag("FEMUR").GetComponent<NewFemurBehaviourScript>();   
        Tibia = GameObject.FindGameObjectWithTag("TIBIA").GetComponent<NewTibiaBehaviourScript1>();   
        Open_Serial();
    }
    
    // Update is called once per frame

    void Update()
    {
        string serialstr = Read_Serial();
         string[]prefix = serialstr.Split(": ");
        if(serialstr != null && (prefix[0] == "A" || prefix[0] == "W"))
            Femur.setSerialData(serialstr);
        if(serialstr != null && (prefix[0] == "S" || prefix[0] == "E"))
            Tibia.setSerialData(serialstr);

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
}