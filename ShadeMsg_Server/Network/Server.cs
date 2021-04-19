using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.Threading;
using ShadeMsg.Network;
using ShadeMsg.Security;
using ShadeMsg.Network.Packets;

namespace ShadeMsg_Server.Network
{
    class Server
    {
        private string password;
        private int port;
        private TcpListener tcpListener;
        private BackgroundWorker backgroundListener;

        public delegate void NewPacket_Delegate(Packet packet);
        public event NewPacket_Delegate NewPacket;

        public delegate void NewSocket_Delegate(Socket socket);
        public event NewSocket_Delegate NewSocketConnected;

        public Server(int port,string password)
        {
            this.port = port;
            this.password = password;      
        }

        public void Start()
        {
            backgroundListener = new BackgroundWorker();
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();

            backgroundListener.DoWork += BackgroundListener_DoWork;
            backgroundListener.RunWorkerAsync();
        }

        public void Send(Packet packet,Client client)
        {
            BackgroundWorker backgroundSender = new BackgroundWorker();
            backgroundSender.DoWork += (obj,e) => BackgroundSender_DoWork(packet,client);
            backgroundSender.RunWorkerAsync();
        }

        private void BackgroundSender_DoWork(Packet packet, Client client)
        {
            if (client.socket.Connected)
            {
                Socket socket = client.socket;
                string crypted_packet = PacketEncryption.EncryptPacket(packet, password);

                if(!string.IsNullOrEmpty(crypted_packet))
                {
                    byte[] data = Encoding.UTF8.GetBytes(PacketEncryption.EncryptPacket(packet, password));
                    socket.Send(data);
                }
            }
        }

        private void BackgroundListener_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                Socket socket = tcpListener.AcceptSocket();
                NewSocketConnected(socket);
                Thread.Sleep(100);
            }
        }
    }
}
