using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.Threading;
using ShadeMsg.Network;
using ShadeMsg.Security;

namespace ShadeMsg_Server.Network
{
    class Client
    {
        private string password;
        private Socket socket;
        private BackgroundWorker backgroundReceiver;

        public delegate void NewPacket_Delegate(Packet packet,Socket socket);
        public event NewPacket_Delegate NewPacket;

        public Client(Socket socket,string name,string password)
        {
            this.password = password;
            this.socket = socket;
            backgroundReceiver = new BackgroundWorker();
            backgroundReceiver.DoWork += BackgroundReceiver_DoWork;
            backgroundReceiver.RunWorkerAsync();
        }

        private void BackgroundReceiver_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                byte[] data = new byte[1024];
                
                if(socket.Connected)
                {
                    try
                    {
                        socket.Receive(data);
                    }
                    catch
                    {
                        Console.WriteLine("Disconected: {0}", socket.RemoteEndPoint.ToString());
                    }
                    
                }
                string data_string = Encoding.UTF8.GetString(data).Trim('\0');
                NewPacket(PacketEncryption.DecryptPacket(data_string, password),socket);
            }
        }
    }
}
