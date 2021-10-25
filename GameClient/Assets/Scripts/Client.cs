using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class Client : MonoBehaviour
{

    public static Client instance;
    public static int dataBufferSize = 4096;
    public string ip = "127.0.0.1";
    public int port = 26956;
    public int myId = 0;
    public TCP tcp;

    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
        }
        else if (instance!=this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        tcp = new TCP();
    }

    public void ConnectToServer()
    {
        tcp.Connect();
    }
    public class TCP 
    {
        public TcpClient socket;
        private NetworkStream stream;
        private byte[] recieveBuffer;

        public void Connect() {
            socket = new TcpClient {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize,
            };
            recieveBuffer = new byte[dataBufferSize];
            socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
        }

        private void ConnectCallback(IAsyncResult result) {
            socket.EndConnect(result);

            if (!socket.Connected)
            {
                return;
            }

            stream = socket.GetStream();

            stream.BeginRead(recieveBuffer, 0, dataBufferSize, ReceiveCallback, null);

        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int byteLength = stream.EndRead(result);
                if (byteLength <= 0)
                {
                    //TODO : disconnect
                    return;
                }

                byte[] data = new byte[byteLength];
                Array.Copy(recieveBuffer, data, byteLength);

                //TODO: handle data
                stream.BeginRead(recieveBuffer, 0, dataBufferSize, ReceiveCallback, null);

            }
            catch (Exception ex)
            {

                //TODO:disconnect 
            }
        }
    }


}
