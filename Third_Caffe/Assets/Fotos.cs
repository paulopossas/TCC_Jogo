using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Net;

public class Fotos : MonoBehaviour
{
    // Dados socket
    internal Boolean socketReady = false;
    public TcpClient mySocket;
    NetworkStream theStream;
    StreamWriter theWriter;
    StreamReader theReader;
#if UNITY_ANDROID
    String Host = "192.168.1.103"; 
#else
    String Host = "localhost";
#endif

    Int32 Port = 1234;

    //Dados pro jogo
    private Texture2D screenShot;
    public GameObject oi;
    private WebCamTexture webcamTexture;
    float tempo;
    public Text mostra;
    String classe="99";

    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        webcamTexture = new WebCamTexture();
        webcamTexture.requestedHeight = 224;
        webcamTexture.requestedWidth = 224;
        //webcamTexture.requestedFPS = 2;
        Renderer renderer = oi.GetComponent<Renderer>();
        renderer.material.mainTexture = webcamTexture;
        webcamTexture.deviceName = devices[0].name;
        webcamTexture.Play();
        screenShot = new Texture2D(webcamTexture.width, webcamTexture.height);//, TextureFormat.RGBA32, false);
        setupSocket();
        Thread _rede = new Thread(Rede);
        _rede.Start();
    }

    void Update()
    {
        Movimento_windows(classe);
        mostra.text = classe;
    }


    public void setupSocket()
    {
        try
        {
            mySocket = new TcpClient(Host, Port);
            theStream = mySocket.GetStream();
            theWriter = new StreamWriter(theStream);
            theReader = new StreamReader(theStream);
            socketReady = true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket error: " + e);
        }
    }


    public void Rede()
    {
        while (true)
        {
            screenShot.SetPixels(webcamTexture.GetPixels());
            byte[] bytes = screenShot.EncodeToJPG(100);
            String rpcText = System.Convert.ToBase64String(bytes);
            if (rpcText.Length > 4000)
            {
                if (theStream.CanWrite)
                {
                    theWriter.Write(rpcText.Length);
                    theWriter.Flush();
                }
                if (theStream.CanRead)
                {
                    byte[] bytesRecebidos = new byte[mySocket.ReceiveBufferSize];
                    theStream.Read(bytesRecebidos, 0, (int)mySocket.ReceiveBufferSize);
                    string ok = Encoding.UTF8.GetString(bytesRecebidos);
                    Debug.Log(ok);
                }
                if (theStream.CanWrite)
                {
                    theWriter.Write(rpcText);
                    theWriter.Flush();
                }
                if (theStream.CanRead)
                {
                    byte[] bytesRecebidos = new byte[mySocket.ReceiveBufferSize];
                    theStream.Read(bytesRecebidos, 0, (int)mySocket.ReceiveBufferSize);
                    classe = Encoding.UTF8.GetString(bytesRecebidos);
                    Debug.Log(classe);
                }
            }
        }
    }

//#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

    [DllImport("ControleLibrary", EntryPoint = "keypress",
       CallingConvention = CallingConvention.StdCall)]
    public static extern int keypress(string key);

    [DllImport("ControleLibrary", EntryPoint = "keyrelease",
        CallingConvention = CallingConvention.StdCall)]

    public static extern int keyrelease(string key);

    private void Movimento_windows(string classe)
    {
        if (classe.Substring(0, 1).Equals("1"))
            PressKey("Left");
        else
            ReleaseKey("Left");

        if (classe.Substring(0, 1).Equals("2"))
            PressKey("Right");
        else
            ReleaseKey("Right");

        if (classe.Substring(0, 1).Equals("3"))
            PressKey("Up");
        else
            ReleaseKey("Up");

        if (classe.Substring(0, 1).Equals("4"))
            PressKey("Down");
        else
            ReleaseKey("Down");
        
        if (classe.Substring(0, 1).Equals("5"))
            PressKey("Select");
        else
            ReleaseKey("Select");

        if (classe.Substring(0, 1).Equals("6"))
            PressKey("Take");
        else
            ReleaseKey("Take");
            
        if (classe.Substring(0, 1).Equals("7"))
            PressKey("Jump");
        else
            ReleaseKey("Jump");
            
    }

    private void PressKey(string key)
    {
        if (key == "Return")
        {
            keypress("" + (char)0x0d);
        }
        else if (key == "Shift_R")
        {
            keypress("" + (char)0xa1);
        }
        else if (key == "Up")
        {
            keypress("" + (char)0x57);
        }
        else if (key == "Down")
        {
            keypress("" + (char)0x53);
        }
        else if (key == "Left")
        {
            keypress("" + (char)0x41);
        }
        else if (key == "Right")
        {
            keypress("" + (char)0x44);
        }
        else if (key == "Jump")
        {
            keypress("" + (char)0x20);
        }
        else if (key == "Select")
        {
            keypress("" + (char)0x4B);
        }
        else if (key == "Take")
        {
            keypress("" + (char)0x4C);
        }
        else
        {
            keypress(key.ToUpper());
        }
    }

    private void ReleaseKey(string key)
    {
        if (key == "Return")
        {
            keyrelease("" + (char)0x0d);
        }
        else if (key == "Shift_R")
        {
            keyrelease("" + (char)0xa1);
        }
        else if (key == "Up")
        {
            keyrelease("" + (char)0x57);
        }
        else if (key == "Down")
        {
            keyrelease("" + (char)0x53);
        }
        else if (key == "Left")
        {
            keyrelease("" + (char)0x41);
        }
        else if (key == "Right")
        {
            keyrelease("" + (char)0x44);
        }
        else if (key == "Jump")
        {
            keyrelease("" + (char)0x20);
        }
        else if (key == "Select")
        {
            keyrelease("" + (char)0x4B);
        }
        else if (key == "Take")
        {
            keyrelease("" + (char)0x4C);
        }
        else
        {
            keyrelease(key.ToUpper());
        }
    }
//#endif
}