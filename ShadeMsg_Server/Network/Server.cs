using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.Threading;
using ShadeMsg.Network;

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
