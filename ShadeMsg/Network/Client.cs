using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using ShadeMsg.Security;
using System.Net.Sockets;
using System.ComponentModel;
using System.Threading;

namespace ShadeMsg.Network
{
    class Client
    {
        private string server;
        private int port;
        private string password;
        private TcpClient tcpClient;
        private NetworkStream networkStream;
        private BackgroundWorker background_receive;

        public delegate void NewPacket_Delegate(Packet packet);
        public event NewPacket_Delegate NewPacket;

        // Event: Connected;
        // Event: NewMessageFromServer
        // Event: ConnectionEnd

        public Client(string server,int port,string password)
        {
            this.password = password;
            this.server = server;
            this.port = port;
            tcpClient = new TcpClient();
            background_receive = new BackgroundWorker();
        }

        public void Connect()
        {
            while(!tcpClient.Connected)
            {
                tcpClient.Connect(server, port);
                networkStream = tcpClient.GetStream();
                
                Thread.Sleep(1000);
            }
            background_receive.DoWork += Background_receive_DoWork;
            background_receive.RunWorkerAsync();
        }

        private void Background_receive_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                
                if(networkStream.DataAvailable)
                {
                    byte[] data = new byte[1024];
                    networkStream.Read(data, 0, data.Length);
                    NewPacket(PacketEncryption.DecryptPacket(Encoding.UTF8.GetString(data), password));
                }
                Thread.Sleep(1000);
            }
        }

        public void Send(Packet packet)
        {
            if(tcpClient.Connected)
            {
                byte[] data = Encoding.UTF8.GetBytes(PacketEncryption.EncryptPacket(packet, password));
                networkStream.Write(data, 0, data.Length);
            }
        }

    }
}
