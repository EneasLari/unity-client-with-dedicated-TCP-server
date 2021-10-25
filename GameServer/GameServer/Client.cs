using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    class Client
    {
        public static int dataBufferSize = 4096;

        public int id;
        public TCP tcp;


        public Client(int clientId) {
            id = clientId;
            tcp = new TCP(id);
        }

        public class TCP 
        {
            public TcpClient socket;

            private readonly int id;

            private NetworkStream stream;

            private byte[] recieveBuffer;
            public TCP(int id) {
                this.id = id;
            }

            public void Connect(TcpClient socket) {
                this.socket = socket;
                socket.ReceiveBufferSize = dataBufferSize;
                socket.SendBufferSize = dataBufferSize;

                stream = socket.GetStream();
                recieveBuffer = new byte[dataBufferSize];

                stream.BeginRead(recieveBuffer, 0, dataBufferSize, ReceiveCallback, null);

                //TODO: send a welcome message

            }

            private void ReceiveCallback(IAsyncResult result)
            {
                try
                {
                    int byteLength = stream.EndRead(result);
                    if (byteLength <= 0) {
                        //TODO : disconnect the client
                        return;
                    }

                    byte[] data = new byte[byteLength];
                    Array.Copy(recieveBuffer, data, byteLength);

                    //TODO: handle data
                    stream.BeginRead(recieveBuffer, 0, dataBufferSize, ReceiveCallback, null);

                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Error recieving TCP data : {ex}");
                    //TODO:disconnect the client
                }
            }
        }
    }
}
